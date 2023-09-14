using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AurogonTools
{

    public delegate void LogCallback(string content, LogType logType, string stackTrace);

    public class Logger : ILogger, IRegisterLogCallback
    {
        #region 静态

        private static Dictionary<string, ILogger> m_loggers = new Dictionary<string, ILogger>();
        private static ILogger m_defaultLog = new Logger();
        public static ILogger Default => m_defaultLog;

        public static ILogger GetLogger()
        {
            return m_defaultLog;
        }

        public static ILogger GetLogger(string logTag)
        {
            return GetLogger(logTag, new LoggerSetting());
        }

        public static ILogger GetLogger(Type type)
        {
            return type == null ? GetLogger() : GetLogger(nameof(type), new LoggerSetting());
        }

        public static ILogger GetLogger(string logTag,LoggerSetting setting)
        {
            if(m_loggers.TryGetValue(logTag,out ILogger logger))
            {
                return logger;
            }

            ILogger newLogger = new Logger(logTag, setting);
            m_loggers.Add(logTag, newLogger);

            return newLogger;
        }

        public static ILogger GetLogger(Type type, LoggerSetting setting)
        {
            return type == null ? GetLogger() : GetLogger(nameof(type),setting);
        }

        #endregion

        #region 成员变量

        private string m_logTag = string.Empty;
        public LoggerSetting loggerSetting { get; set; }
        private event LogCallback m_logCallback = null;
        private FileInfo m_fileInfo = null;
        private StreamWriter m_streamWriter = null;
        private string m_logFilePath;
        public string logFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(m_logFilePath))
                {
                    m_logFilePath = $"{loggerSetting.logPath}\\{m_logTag}{DateTime.Now.ToString("yyyy_MM_dd")}.log";
                }

                return m_logFilePath;
            }
        }

        #endregion

        public Logger():this(string.Empty,new LoggerSetting())
        {
        }

        public Logger(string logTag,LoggerSetting setting)
        {
            m_logTag = logTag;
            loggerSetting = setting;

            m_fileInfo = new FileInfo(logFilePath);
            ClearAndReCreateFile();
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
            WriteToFile(content, logType, stackTrace);
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

        private void WriteToFile(string content, LogType logType, string stackTrace)
        {

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(content);
                sb.AppendLine(stackTrace);
                m_streamWriter = m_fileInfo.AppendText();
                m_streamWriter.Write(sb.ToString());
                m_streamWriter.Close();
                m_streamWriter.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ClearAndReCreateFile()
        {
            if(m_fileInfo.Exists)
            {
                m_fileInfo.Delete();
            }

            m_streamWriter = m_fileInfo.CreateText();
            m_streamWriter.WriteLine($"=========={DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {m_logTag} Log Start==========");
            m_streamWriter.Close();
            m_streamWriter.Dispose();
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
