using System;
namespace AurogonTools
{
    public class LoggerSetting
    {
        public LogType logType = LogType.All;
        public string logPath;
        public bool logFileEnabled;

        public LoggerSetting()
        {
            logType = LogType.All;
            logPath = AppDomain.CurrentDomain.BaseDirectory + "log";

            if(System.IO.Directory.Exists(logPath) == false)
            {
                System.IO.Directory.CreateDirectory(logPath);
            }
            logFileEnabled = false;
        }
    }
}
