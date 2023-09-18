using System;
using CommandLineOption;
using AurogonTools;
using AurogonCodeGenerator;

namespace ConfigurationGenerator
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ILogger logger = Logger.GetLogger(new LoggerSetting() { logType = LogType.All });
            args = new string[10]
            {
                "-",
                "--",
                "-abc",
                "1.73",
                "Aurogonpu",
                "-p",
                "d:/test/test.log",
                "--name=test.log",
                "--accept=",
                "--accept=11=",
            };


            //Setting setting = CommandLineParser.Default.Parse<Setting>(args,false);

            //logger.Info(setting.ToString());

            //ExcelReader excel = new ExcelReader("/Users/aurogonpu/Projects/excel/story.xlsx");

            CodeGenerator codeGenerator = new CodeGenerator("Story");
            codeGenerator.AddNameSpace("System");
            codeGenerator.AddInterface("IPackage");
            codeGenerator.AddPropertyInfo("ID", typeof(uint));
            codeGenerator.AddPropertyInfo("ID2", typeof(int));
            codeGenerator.AddPropertyInfo("ID3", typeof(ushort));
            codeGenerator.AddPropertyInfo("ID4", typeof(short));
            codeGenerator.AddPropertyInfo("ID5", typeof(byte));
            codeGenerator.AddPropertyInfo("ID6", typeof(sbyte));
            codeGenerator.AddPropertyInfo("ID7", typeof(ulong));
            codeGenerator.AddPropertyInfo("ID8", typeof(long));
            codeGenerator.AddPropertyInfo("Name", typeof(string));
            codeGenerator.AddPropertyInfo("IsOpen", typeof(bool));
            codeGenerator.AddPropertyInfo("Speed", typeof(float));
            codeGenerator.AddPropertyInfo("Speed2", typeof(double));

            string code = codeGenerator.GenerationCode();

            logger.Debug(code);

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
        [Option("a", "accept", helpText = "accept", required = true)]
        public float floatValue { get; set; }

        [Option("b", "batchmode", helpText = "batch mode start", required = false)]
        public bool IsBatchMode { get; set; }

        [Option("p", "path", helpText = "设置路径", required = true)]
        public string Path { get; set; }

        public int test1 { get; set; }

        public override string ToString()
        {
            return $"Setting ->floatValue:{floatValue} Path:{Path} IsBatchMode:{IsBatchMode} {base.ToString()}";
        }
    }


    public class BaseSetting
    {
        [Option("m", "name", helpText = "设置路径", required = true)]
        public string Name { get; set; }


        [Option("d", "disable", helpText = "disable mode start", required = false)]
        public bool IsEnabed { get; set; }

        public string test2 { get; set; }

        public override string ToString()
        {
            return $"Name:{Name} IsEnabled:{IsEnabed}";
        }
    }
}
