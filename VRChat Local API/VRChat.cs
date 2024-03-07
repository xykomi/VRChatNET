using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using WebSocketSharp;
using VRChat_Local_API.Objects;
using static VRChat_Local_API.Objects.VRCEvents;

namespace VRChat_Local_API
{
    public class VRChat
    {
        private readonly WebSocket _webSocket;
        private readonly string _logDirectory;
        private long _logFileCurrentLength = 0;
        private bool _disableThreads = false;
        private string _pastLogFileContent = string.Empty;

        public bool WebSocketConnected { get; private set; } = false;
        public string LogFileContent { get; private set; } = string.Empty;
        public FileInfo? LogFile { get; private set; }

        public event EventHandler<object> OnInitialized = null!;
        public event EventHandler<OnPlayerJoined> OnPlayerJoined = null!;
        public event EventHandler<OnPlayerLeft> OnPlayerLeft = null!;
        public event EventHandler<OnPlayerBlocked> OnPlayerBlocked = null!;
        public event EventHandler<OnPlayerUnBlocked> OnPlayerUnBlocked = null!;
        public event EventHandler<OnPlayerAvatarModeration> OnPlayerAvatarModerationChanged = null!;
        public event EventHandler<OnRoomJoin> OnRoomJoined = null!;
        public event EventHandler<OnRoomLeft> OnRoomLeft = null!;
        public event EventHandler<OnNotificationReceived> OnNotificationReceived = null!;
        public event EventHandler<OnFriendLocationUpdate> OnFriendLocationUpdate = null!;
        public event EventHandler<OnFriendOffline> OnFriendOffline = null!;
        public event EventHandler<OnFriendOnline> OnFriendOnline = null!;
        public event EventHandler<OnFriendActive> OnFriendActive = null!;
        public event EventHandler<OnFriendAdd> OnFriendAdd = null!;
        public event EventHandler<OnResponseNotification> OnResponseNotification = null!;
        public event EventHandler<OnUserLocation> OnUserLocationUpdated = null!;

        public VRChat(string authCookie)
        {
            _logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("Roaming", "LocalLow"), "VRChat", "VRChat");
            _webSocket = new WebSocket($"wss://pipeline.vrchat.cloud/?authToken={authCookie}");
        }

        public void Initialize()
        {
            WebSocketListener();
            LogFile = GetLogFile();
            if (LogFile != null)
            {
                using (FileStream fileStream = new FileStream(LogFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    _logFileCurrentLength = fileStream.Length;
            }
            else
            {
                throw new FileNotFoundException("Failed to access the most recent log file");
            }

            FileEventListener();

            OnInitialized?.Invoke(this, this);
        }

        private void WebSocketListener()
        {
            _webSocket.OnOpen += OnOpen;
            _webSocket.OnMessage += OnMessage;
            _webSocket.OnClose += OnClose;
            _webSocket.Connect();
        }

        private void OnOpen(object? sender, EventArgs e)
        {
            WebSocketConnected = true;
            OnInitialized?.Invoke(this, this);
        }

        private void OnMessage(object? sender, MessageEventArgs e)
        {
            SocketMessageBase incomingData = JsonConvert.DeserializeObject<SocketMessageBase>(e.Data);

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

        private void OnClose(object? sender, CloseEventArgs e)
        {
            WebSocketConnected = false;
        }

        private void FileEventListener()
        {
            new Thread(() =>
            {
                while (!_disableThreads)
                {
                    try
                    {
                        string currentLogFileContent = ReadLogFileContent();

                        if (currentLogFileContent != _pastLogFileContent)
                        {
                            ProcessLogFileContent(currentLogFileContent);
                            _pastLogFileContent = currentLogFileContent;
                        }
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(1000);
                    }

                    Thread.Sleep(350);
                }
            }).Start();
        }

        private string ReadLogFileContent()
        {
            using (FileStream fileStream = new FileStream(LogFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default))
            {
                fileStream.Seek(_logFileCurrentLength - 1, SeekOrigin.Begin);
                return streamReader.ReadToEnd();
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

        public void Shutdown() => _disableThreads = true;

        public bool IsRunning() => Process.GetProcessesByName("VRChat").Length > 0;

        public FileInfo? GetLogFile()
        {
            foreach (var file in new DirectoryInfo(_logDirectory).GetFiles().OrderByDescending(x => x.LastWriteTime))
            {
                if (file.Name.EndsWith(".txt"))
                {
                    return file;
                }
            }
            return null;
        }
    }
}