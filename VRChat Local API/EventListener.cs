using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Text;
using VRChat_Local_API.Objects;
using static VRChat_Local_API.Objects.VRChatEvents;

namespace VRChat_Local_API
{
    public class EventListener
    {
        private static string _logDirectory { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("Roaming", "LocalLow") + @"/VRChat/VRChat";
        private FileInfo? _logFile { get; set; } = null;
        private long _logFileCurrentLength { get; set; } = 0;
        private bool _disableThreads { get; set; } = false;

        public string LogFileContent { get; set; } = string.Empty;
        private string PastLogFileContent { get; set; } = "x";

        public event EventHandler<VRChatEvents.OnPlayerJoined> OnPlayerJoined = null!;
        public event EventHandler<VRChatEvents.OnPlayerLeft> OnPlayerLeft = null!;

        public void Initialize(EventListenerConfig configuration)
        {
            if (configuration.RequireGameRunning && !IsProcessRunning())
                throw new Exception("VRChat isn't running check the active config");

            _logFile = GetLogFile();

            if (_logFile != null)
                using (FileStream fileStream = new FileStream(_logFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    _logFileCurrentLength = fileStream.Length;
            else
                throw new Exception("Failed to access the most recent log file");

            StartEventThread();
        }

        private void StartEventThread()
        {
            new Thread(() => 
            {
                while (!_disableThreads)
                {
                    using (FileStream fileStream = new FileStream(_logFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        fileStream.Seek(_logFileCurrentLength - 1, SeekOrigin.Begin);

                        using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default))
                            LogFileContent = streamReader.ReadToEnd();

                        if (LogFileContent != PastLogFileContent)
                        {
                            foreach (var line in LogFileContent.Replace(PastLogFileContent, "").Split('\n'))
                            {
                                if (line.Contains("OnPlayerJoined"))
                                {
                                    string displayName = Regex.Match(line, @"OnPlayerJoined (.+)").Groups[1].Value;
                                    if (displayName == string.Empty)
                                        continue;

                                    OnPlayerJoined?.Invoke(this, new VRChatEvents.OnPlayerJoined() { dateTime = DateTime.Now, displayName = displayName, data = line });
                                }

                                if (line.Contains("OnPlayerLeft"))
                                {

                                    string displayName = Regex.Match(line, @"OnPlayerLeft (.+)").Groups[1].Value;
                                    if (displayName == string.Empty)
                                        continue;

                                    OnPlayerLeft?.Invoke(this, new VRChatEvents.OnPlayerLeft() { dateTime = DateTime.Now, displayName = displayName, data = line });
                                }
                            }

                            PastLogFileContent = LogFileContent;
                        }
                    }
                }
            }).Start();
        }

        public void Shutdown() { _disableThreads = true; }

        public bool IsProcessRunning() => Process.GetProcessesByName("VRChat").Length > 0;

        public static FileInfo? GetLogFile()
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