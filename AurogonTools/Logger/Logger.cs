using System;
using System.Collections.Generic;

namespace AurogonTools
{

    public delegate void LogCallback(string content, LogType logType, string stackTrace);

    public class Logger : ILogger, IRegisterLogCallback
    {
        private static Dictionary<string, ILogger> m_loggers = new Dictionary<string, ILogger>();
        private static ILogger m_defaultLog = new Logger();

        public static ILogger GetLogger()
        {
            return m_defaultLog;
        }

        public static ILogger GetLogger(string logTag)
        {
            if(m_loggers.TryGetValue(logTag,out ILogger logger))
            {
                return logger;
            }

            ILogger newLogger = new Logger(logTag);
            m_loggers.Add(logTag, newLogger);

            return newLogger;
        }

        public static ILogger GetLogger(Type type)
        {

            return type == null ? GetLogger() : GetLogger(nameof(type));
        }


        private string m_logTag = string.Empty;
        public LoggerSetting loggerSetting { get; set; }
        private event LogCallback m_logCallback = null;

        public Logger()
        {

        }
        public Logger(string logTag)
        {
            m_logTag = logTag;
        }

        public void Debug(string content)
        {
            PrintLog(content,LogType.Debug);
        }

        public void Error(string content)
        {
            PrintLog(content,LogType.Error);
        }

        public void Fatal(string content)
        {
            PrintLog(content,LogType.Fatal);
        }

        public void Info(string content)
        {
            PrintLog(content,LogType.Info);
        }

        public void Warning(string content)
        {
            PrintLog(content,LogType.Warning);
        }

        private void PrintLog(string content,LogType logType)
        {
            InitLogSetting();
            if(loggerSetting.logType > logType)
            {
                return;
            }
            StackTracer stackTracer = new StackTracer(2);
            string stackTrace = stackTracer.GetStackTrace(string.Format("{0,35}",""));

            content = string.Format("{0} {1,-10} {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), logType, content);
            ConsoleLog(content, logType, stackTrace);
            if (m_logCallback != null)
            {
                m_logCallback(content, logType, stackTrace);
            }

        }

        private void ConsoleLog(string content, LogType logType, string stackTrace)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            switch(logType)
            {
                case LogType.All:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(content);
                    break;
                case LogType.Debug:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(content);
                    break;
                case LogType.Info:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(content);
                    break;
                case LogType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(content);
                    break;
                case LogType.Error:
                case LogType.Fatal:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(content);
                    Console.WriteLine(stackTrace);
                    break;
            }


            Console.ForegroundColor = oldColor;

        }

        private void InitLogSetting()
        {
            if(loggerSetting == null)
            {
                loggerSetting = new LoggerSetting();
                loggerSetting.logPath = AppDomain.CurrentDomain.BaseDirectory;
                loggerSetting.logType = LogType.Info;
            }
        }

        public void RegisterLogCallback(LogCallback callback)
        {
            m_logCallback += callback;
        }

        public void UnRegisterLogCallback(LogCallback callback)
        {
            m_logCallback -= callback;
        }
    }
}
