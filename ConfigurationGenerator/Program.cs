using System;
using CommandLineOption;
using AurogonTools;

namespace ConfigurationGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            CommandLineParser.Default.Parse<Setting>(args);

           // TestLogger();
            Console.ReadKey();
        }

        private static void TestLogger()
        {
            ILogger logger = Logger.GetLogger("Main", new LoggerSetting() { logType = LogType.All });
            var logCallback = logger as IRegisterLogCallback;
            logCallback.RegisterLogCallback(OnReigisterLogCallBack);
            int index = 0;
            for (int i = 0; i < 2; i++)
            {
                logger.Debug($"Debug Log:{index}");
                logger.Info($"Info Log:{index}");
                logger.Warning($"Warning Log:{index}");
                logger.Error($"Error Log:{index}");
                logger.Fatal($"Fatal Log:{index}");
                index++;
            }

        }

        private static void OnReigisterLogCallBack(string content, LogType logType, string stackTrace)
        {
            Console.WriteLine(content, logType, stackTrace);
        }
    }

    public class Setting: BaseSetting
    {
        [Option("p", "path", helpText = "设置路径", required = true)]
        public string Path { get; set; }

        [Option("b", "batchmode", helpText = "batch mode start", required = false)]
        public bool IsBatchMode { get; set; }

        public int test1 { get; set; }
    }


    public class BaseSetting
    {
        [Option("p", "path", helpText = "设置路径", required = true)]
        public string Name { get; set; }

        [Option("b", "batchmode", helpText = "batch mode start", required = false)]
        public bool IsEnabed { get; set; }

        public string test2 { get; set; }

        public void Dispose()
        {

        }
    }
}
