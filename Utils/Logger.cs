using System;
using System.IO;
using WiseMigrateTests.Utils;  

namespace WiseMigrateTests.Utils
{
    public static class Logger
    {
        private static readonly string LogDirectory = ConfigReader.Get("LogPath");
        private static readonly string LogPath;

        static Logger()
        {
            Directory.CreateDirectory(LogDirectory);
            LogPath = Path.Combine(LogDirectory, $"test-run-{DateTime.Now:yyyyMMdd-HHmmss}.log");
        }

        public static void Info(string message) => Log("INFO", message);
        public static void Error(string message) => Log("ERROR", message);
        public static void Warn(string message) => Log("WARN", message);

        private static void Log(string level, string message)
        {
            var entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}{Environment.NewLine}";
            try { File.AppendAllText(LogPath, entry); }
            catch { Console.WriteLine($"[LOG FAIL] {entry.Trim()}"); }
        }
    }
}