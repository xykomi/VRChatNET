using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebSocketSharp;
using VRChatLibrary.Objects;
using static VRChatLibrary.Objects.VRCEvents;

namespace VRChatLibrary
{
    public class VRChat
    {
        private readonly WebSocket _webSocket;
        private readonly string _logDirectory;
        private string _pastLogFileContent = string.Empty;
        private bool _disableThreads = false;
        private long _lastPosition = 0;
        public bool WebSocketConnected { get; private set; } = false;
        public FileInfo? LogFile { get; private set; }

        public event EventHandler<object> OnInitialized;
        public event EventHandler<OnPlayerJoined> OnPlayerJoined;
        public event EventHandler<OnPlayerLeft> OnPlayerLeft;
        public event EventHandler<OnPlayerBlocked> OnPlayerBlocked;
        public event EventHandler<OnPlayerUnBlocked> OnPlayerUnBlocked;
        public event EventHandler<OnPlayerAvatarModeration> OnPlayerAvatarModerationChanged;
        public event EventHandler<OnRoomJoin> OnRoomJoined;
        public event EventHandler<OnRoomLeft> OnRoomLeft;
        public event EventHandler<OnNotificationReceived> OnNotificationReceived;
        public event EventHandler<OnFriendLocationUpdate> OnFriendLocationUpdate;
        public event EventHandler<OnFriendOffline> OnFriendOffline;
        public event EventHandler<OnFriendOnline> OnFriendOnline;
        public event EventHandler<OnFriendActive> OnFriendActive;
        public event EventHandler<OnFriendAdd> OnFriendAdd;
        public event EventHandler<OnFriendRemoved> OnFriendRemoved;
        public event EventHandler<OnResponseNotification> OnResponseNotification;
        public event EventHandler<OnUserLocation> OnUserLocationUpdated;

        public VRChat(string authCookie)
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string localLowPath = Path.Combine(Directory.GetParent(localAppData).FullName, "LocalLow");
            _logDirectory = Path.Combine(localLowPath, "VRChat", "VRChat");
            _webSocket = new WebSocket($"wss://pipeline.vrchat.cloud/?authToken={authCookie}");
        }

        public async Task InitializeAsync()
        {
            await WebSocketListenerAsync().ConfigureAwait(false);
            LogFile = GetLogFile();
            if (LogFile == null)
            {
                throw new FileNotFoundException("Failed to access the most recent log file");
            }

            await FileEventListenerAsync().ConfigureAwait(false);
            OnInitialized?.Invoke(this, EventArgs.Empty);
        }

        private async Task WebSocketListenerAsync()
        {
            _webSocket.OnOpen += (sender, e) => WebSocketConnected = true;
            _webSocket.OnMessage += (sender, e) => ProcessWebSocketMessage(e.Data);
            _webSocket.OnClose += (sender, e) => WebSocketConnected = false;

            await Task.Run(() => _webSocket.Connect());
        }

        private void ProcessWebSocketMessage(string message)
        {
            SocketMessageBase incomingData = JsonConvert.DeserializeObject<SocketMessageBase>(message);
            if (incomingData == null) return;

            switch (incomingData.Type)
            {
                case "notification":
                    InvokeEvent(OnNotificationReceived, incomingData.Content, typeof(OnNotificationReceived));
                    break;
                case "friend-location":
                    InvokeEvent(OnFriendLocationUpdate, incomingData.Content, typeof(OnFriendLocationUpdate));
                    break;
                case "friend-offline":
                    InvokeEvent(OnFriendOffline, incomingData.Content, typeof(OnFriendOffline));
                    break;
                case "friend-online":
                    InvokeEvent(OnFriendOnline, incomingData.Content, typeof(OnFriendOnline));
                    break;
                case "friend-active":
                    InvokeEvent(OnFriendActive, incomingData.Content, typeof(OnFriendActive));
                    break;
                case "friend-add":
                    InvokeEvent(OnFriendAdd, incomingData.Content, typeof(OnFriendAdd));
                    break;
                case "response-notification":
                    InvokeEvent(OnResponseNotification, incomingData.Content, typeof(OnResponseNotification));
                    break;
                case "user-location":
                    InvokeEvent(OnUserLocationUpdated, incomingData.Content, typeof(OnUserLocation));
                    break;
                case "friend-delete":
                    InvokeEvent(OnFriendRemoved, incomingData.Content, typeof(OnFriendRemoved));
                    break;
                default:
                    Console.WriteLine($"Implementation Required = {incomingData.Type} -> {incomingData.Content}");
                    break;
            }
        }

        private void InvokeEvent(Delegate? handler, string content, Type eventType)
        {
            if (handler != null)
            {
                object eventArgs = JsonConvert.DeserializeObject(content, eventType);
                handler.DynamicInvoke(this, eventArgs);
            }
        }

        private async Task FileEventListenerAsync()
        {
            try
            {
                await Task.Run(async () =>
                {
                    while (!_disableThreads)
                    {
                        try
                        {
                            var currentLogFileContent = await ReadLogFileContentAsync();
                            if (currentLogFileContent != _pastLogFileContent)
                            {
                                ProcessLogFileContent(currentLogFileContent);
                                _pastLogFileContent = currentLogFileContent;
                            }
                        }
                        catch (IOException ex)
                        {
                            LogFile = GetLogFile();
                            _pastLogFileContent = string.Empty;
                            await Task.Delay(1000);
                        }

                        await Task.Delay(350);
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to initialize FileEventListener: {ex.Message}");
            }
        }

        private async Task<string> ReadLogFileContentAsync()
        {
            if (LogFile == null) return string.Empty;
            using (var fileStream = new FileStream(LogFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                if (_lastPosition == 0)
                {
                    _lastPosition = fileStream.Length;
                }

                if (fileStream.Length == _lastPosition)
                {
                    return string.Empty;
                }

                fileStream.Seek(_lastPosition, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(fileStream, Encoding.Default))
                {
                    string content = await streamReader.ReadToEndAsync();
                    _lastPosition = fileStream.Position;
                    return content;
                }
            }
        }

        private void ProcessLogFileContent(string logFileContent)
        {
            foreach (var line in logFileContent.Split('\n'))
            {
                if (line.Contains("OnPlayerJoined"))
                    ProcessOnPlayerJoined(line);
                else if (line.Contains("OnPlayerLeft"))
                    ProcessOnPlayerLeft(line);
                else if (line.Contains("Successfully left room"))
                    OnRoomLeft?.Invoke(this, new OnRoomLeft() { dateTime = DateTime.Now, data = line });
                else if (line.Contains("Joining wrld_"))
                    ProcessJoiningWorld(line);
                else if (line.Contains("ModerationManager"))
                    ProcessModerationManager(line);
            }
        }

        public FileInfo? GetLogFile()
        {
            return new DirectoryInfo(_logDirectory)
                .GetFiles("*.txt")
                .OrderByDescending(f => f.LastWriteTime)
                .FirstOrDefault();
        }

        public void Shutdown() => _disableThreads = true;

        public bool IsRunning() => Process.GetProcessesByName("VRChat").Any();

        private void ProcessOnPlayerJoined(string line)
        {
            string displayName = Regex.Match(line, @"OnPlayerJoined (.+)").Groups[1].Value;
            if (!string.IsNullOrEmpty(displayName))
                OnPlayerJoined?.Invoke(this, new OnPlayerJoined() { dateTime = DateTime.Now, displayName = displayName, data = line });
        }

        private void ProcessOnPlayerLeft(string line)
        {
            string displayName = Regex.Match(line, @"OnPlayerLeft (.+)").Groups[1].Value;
            if (!string.IsNullOrEmpty(displayName))
                OnPlayerLeft?.Invoke(this, new OnPlayerLeft() { dateTime = DateTime.Now, displayName = displayName, data = line });
        }

        private void ProcessJoiningWorld(string line)
        {
            string worldId = line.Split("Joining ")[1].Split(':')[0];
            int roomInstance = int.Parse(line.Split(':')[1]);
            OnRoomJoined?.Invoke(this, new OnRoomJoin() { dateTime = DateTime.Now, worldId = worldId, roomInstance = roomInstance, data = line });
        }

        private void ProcessModerationManager(string line)
        {
            string moderationData = Regex.Match(line, @"\[ModerationManager\] (.+)").Groups[1].Value;

            if (line.ToLower().Contains("avatar"))
            {
                string displayName = moderationData.Split("avatar")[0].Replace(" ", "");
                string displayData = moderationData.ToLower().Split("avatar")[1];

                if (displayData.Contains("hidden"))
                    OnPlayerAvatarModerationChanged?.Invoke(this, new OnPlayerAvatarModeration() { dateTime = DateTime.Now, displayName = displayName, moderationType = OnPlayerAvatarModeration.ModerationType.Hidden, data = line });
                if (displayData.Contains("enabled"))
                    OnPlayerAvatarModerationChanged?.Invoke(this, new OnPlayerAvatarModeration() { dateTime = DateTime.Now, displayName = displayName, moderationType = OnPlayerAvatarModeration.ModerationType.Shown, data = line });
                if (displayData.Contains("safety"))
                {
                    displayName = moderationData.Split("Avatar")[0].Replace(" ", "");
                    OnPlayerAvatarModerationChanged?.Invoke(this, new OnPlayerAvatarModeration() { dateTime = DateTime.Now, displayName = displayName, moderationType = OnPlayerAvatarModeration.ModerationType.Safety, data = line });
                }
            }
            else if (line.Contains("Requesting block on"))
            {
                string displayName = moderationData.Split("Requesting block on")[1];
                OnPlayerBlocked?.Invoke(this, new OnPlayerBlocked() { dateTime = DateTime.Now, displayName = displayName, data = line });
            }
            else if (line.Contains("Requesting unblock on"))
            {
                string displayName = moderationData.Split("Requesting unblock on")[1];
                OnPlayerUnBlocked?.Invoke(this, new OnPlayerUnBlocked() { dateTime = DateTime.Now, displayName = displayName, data = line });
            }
        }
    }
}
