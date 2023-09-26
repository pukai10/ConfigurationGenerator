using System;
using CommandLineOption;
using AurogonTools;
using AurogonCodeGenerator;
using System.Collections.Generic;
using System.Xml;
using System.Reflection;
using AurogonXmlConvert;
using static Org.BouncyCastle.Math.EC.ECCurve;

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

            // CSharpCodeGenerator codeGenerator = new CSharpCodeGenerator("Story");
            // codeGenerator.AddNameSpace("System");
            //// codeGenerator.SetNameSpace("AurogonRes");
            // codeGenerator.AddInterface("IPackage");
            // codeGenerator.AddProperty("ID", typeof(uint).Name, 3);
            // codeGenerator.AddProperty("ID2", typeof(int).Name);
            // codeGenerator.AddField("ID3", typeof(ushort).Name);
            // codeGenerator.AddProperty("ID4", typeof(short).Name, 2);
            // codeGenerator.AddField("ID5", typeof(byte).Name);
            // codeGenerator.AddField("ID6", typeof(sbyte).Name);
            // codeGenerator.AddField("ID7", typeof(ulong).Name);
            // codeGenerator.AddProperty("ID8", typeof(long).Name);
            // codeGenerator.AddProperty("Name", typeof(string).Name, 6);
            // codeGenerator.AddProperty("IsOpen", typeof(bool).Name, 1);
            // codeGenerator.AddProperty("Speed", typeof(float).Name);
            // codeGenerator.AddProperty("Speed2", typeof(double).Name);
            // codeGenerator.AddProperty("stAwardInfo", "AwardInfo");

            // var fields = codeGenerator.CodeFields;
            // int tabCount = codeGenerator.GetTabCount();
            // PackStatement statement = new PackStatement(tabCount + 2, fields);
            // UnPackStatement unStatement = new UnPackStatement(tabCount + 2, fields);

            // logger.Debug(statement.GenerateCode());

            // PackMethod packMethod = new PackMethod("Pack", statement.GenerateCode(),tabCount + 1);
            // UnPackMethod unPackMethod = new UnPackMethod("UnPack", unStatement.GenerateCode(),tabCount + 1);

            // codeGenerator.AddMethod(packMethod);
            // codeGenerator.AddMethod(unPackMethod);

            // codeGenerator.GeneratorCodeToSave(AppDomain.CurrentDomain.BaseDirectory);

            //ConfigGeneration config = new ConfigGeneration();
            //config.Name = "AurogonGame";
            //config.ExcelNodes = new List<ExcelNode>();

            //ExcelNode excelNode = new ExcelNode();
            //excelNode.Name = "Test.xlsx";
            //excelNode.SheetNodes = new List<ExcelSheetNode>
            //{
            //    new ExcelSheetNode() { Name = "Test.xlsx - test1", BinaryFile = "./test1.bytes", MetaFile = "./Test.xml", SheetName = "Test1", StructName = "Test1" }
            //};

            //config.ExcelNodes.Add(excelNode);

            //XmlUtility.ToXml<ConfigGeneration>(config, AppDomain.CurrentDomain.BaseDirectory + "gamecfg.xml");

            ConfigMeta configMeta = new ConfigMeta();
            configMeta.NameSpace = "AurogonRes";
            configMeta.Structs = new List<ConfigStruct>();
            ConfigStruct configStruct = new ConfigStruct();
            configStruct.Name = "Test1";
            configStruct.Desc = "Test1 cfg";
            configStruct.Properties = new List<StructProperty>()
            {
                new StructProperty(){ PropertyName = "ID", PropertyType = "uint32", CName = "ID",Desc = "hero id"},
                new StructProperty(){ PropertyName = "Name",PropertyType = "string", Size = 64, CName = "Name", Desc = "hero name"},
                new StructProperty(){PropertyName = "SkillList",PropertyType = "uint32",Count = 5,CName = "Skill",Desc = "hero skill"}
            };
            configMeta.Structs.Add(configStruct);

            XmlUtility.ToXml<ConfigMeta>(configMeta, AppDomain.CurrentDomain.BaseDirectory + "gamemeta.xml");


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

    public class Setting : BaseSetting
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
