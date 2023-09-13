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
            ILogger logger = Logger.GetLogger("Main");
            logger.loggerSetting = new LoggerSetting() { logType = LogType.All };
            logger.Debug("Debug Log");
            logger.Info("Info Log");
            logger.Warning("Warning Log");
            logger.Error("Error Log");
            logger.Fatal("Fatal Log");
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
