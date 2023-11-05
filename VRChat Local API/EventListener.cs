namespace VRChat_Local_API
{
    public class EventListener
    {
        private static string _logDirectory { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("Roaming", "LocalLow") + @"/VRChat/VRChat";
        private static FileInfo? _logFile { get; set; } = null;
        private static long _logFileCurrentLength { get; set; } = 0;

        public void Initialize()
        {
            _logFile = GetLogFile();


        }

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