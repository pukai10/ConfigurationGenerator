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

            args = new string[3]
            {
                "-p",
                "test\test.cs",
                "--batchmode"
            };

            CommandLineParser.Default.Parse<Setting>(args);
            TestLogger();
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

    [Command("export","export excels")]
    public class Setting
    {
        [Option("p","path",describte:"设置路径",required:true)]
        public string Path { get; set; }

        [Option("b","batchmode",describte:"batch mode start",required:false)]
        public bool IsBatchMode { get; set; }
    }
}
