namespace RanR.POC.Logging
{
    public interface ILogger
    {
        void LogInfoMessage(string message);
        void LogError(string message, string stackTrace);
        string ReadLog(bool getErrorLog);
        string GetPath(bool getErrorLogPath);

    }
}
