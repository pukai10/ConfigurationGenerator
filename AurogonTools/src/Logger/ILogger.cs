using System;
namespace AurogonTools
{
    public interface ILogger
    {
        void Info<T>(T content);
        void Debug<T>(T content);
        void Warning<T>(T content);
        void Error<T>(T content);
        void Fatal<T>(T content);

        LoggerSetting loggerSetting { get; set; }
    }

    public interface IRegisterLogCallback
    {
        void RegisterLogCallback(LogCallback callback);
        void UnRegisterLogCallback(LogCallback callback);
    }
}
