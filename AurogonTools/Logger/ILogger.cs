using System;
namespace AurogonTools
{
    public interface ILogger
    {
        void Info(string content);
        void Debug(string content);
        void Warning(string content);
        void Error(string content);
        void Fatal(string content);

        LoggerSetting loggerSetting { get; set; }
    }

    public interface IRegisterLogCallback
    {
        void RegisterLogCallback(LogCallback callback);
        void UnRegisterLogCallback(LogCallback callback);
    }
}
