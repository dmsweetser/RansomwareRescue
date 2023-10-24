using System;
using System.IO;

namespace RanR.POC.Logging
{
    public class FileLogger : ILogger
    {
        public string LogPath { get; set; }
        private static object LogLock = new object();

        private static readonly Lazy<FileLogger> _instance =
           new Lazy<FileLogger>(
               () => new FileLogger());

        public static FileLogger Instance { get { return _instance.Value; } }

        private FileLogger()
        {
            LogPath = System.Environment.CurrentDirectory;
        }

        public string GetPath(bool getErrorLogPath)
        {
            if (getErrorLogPath)
            {
                return String.Format("{0}RanR_{1:yyyyMMdd}_Error.log", LogPath, DateTime.Now);
            }
            else
            {
                return String.Format("{0}RanR_{1:yyyyMMdd}_Info.log", LogPath, DateTime.Now);
            }
        }

        public void LogError(string message, string stackTrace)
        {
            lock (LogLock)
            {
                File.AppendAllText(String.Format("{0}RanR_{1:yyyyMMdd}_Error.log", LogPath, DateTime.Now),
                     String.Format("Date of Error: {0}\r\nError message: {1}\r\nStack trace: {2}", DateTime.Now, message, stackTrace));
            }
        }

        public void LogInfoMessage(string message)
        {
            lock (LogLock)
            {
                File.AppendAllText(String.Format("{0}RanR_{1:yyyyMMdd}_Info.log", LogPath, DateTime.Now),
                     String.Format("{0}: {1}\r\n", DateTime.Now, message));
            }
        }
        
        public string ReadLog(bool getErrorLog)
        {
            if (getErrorLog)
            {
                return File.ReadAllText(String.Format("{0}RanR_{1:yyyyMMdd}_Error.log", LogPath, DateTime.Now));
            }
            else
            {
                return File.ReadAllText(String.Format("{0}RanR_{1:yyyyMMdd}_Info.log", LogPath, DateTime.Now));
            }
        }
    }
}
