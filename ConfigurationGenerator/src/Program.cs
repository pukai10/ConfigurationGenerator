using System;
using CommandLineOption;
using AurogonTools;
using AurogonCodeGenerator;
using AurogonXmlConvert;
using System.IO;

namespace ConfigurationGenerator
{
    class Program
    {

        private static string Version = "0.1";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            args = new string[]
            {
                "--path",
                "..\\..\\..\\Config\\GameConfigConvert.xml"
            };
            ILogger logger = Logger.GetLogger(new LoggerSetting() { logType = LogType.All });

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
            //config.ConvertTree = new List<ConfigConvertTree>();
            //ConfigConvertTree tree = new ConfigConvertTree();
            //tree.ExcelNodes = new List<ExcelNode>();
            //ExcelNode excelNode = new ExcelNode();
            //excelNode.Name = "Test.xlsx";
            //excelNode.SheetNodes = new List<ExcelSheetNode>
            //{
            //    new ExcelSheetNode() { Name = "Test.xlsx - test1", BinaryFile = "./test1.bytes", MetaFile = "./Test.xml", SheetName = "Test1", StructName = "Test1" }
            //};
            //tree.ExcelNodes.Add(excelNode);
            //config.ConvertTree.Add(tree);

            //XmlUtility.ToXml<ConfigGeneration>(config, AppDomain.CurrentDomain.BaseDirectory + "gamecfg.xml");

            //ConfigMeta configMeta = new ConfigMeta();
            //configMeta.NameSpace = "AurogonRes";
            //configMeta.Structs = new List<ConfigStruct>();
            //ConfigStruct configStruct = new ConfigStruct();
            //configStruct.Name = "Test1";
            //configStruct.Desc = "Test1 cfg";
            //configStruct.Properties = new List<StructProperty>()
            //{
            //    new StructProperty(){ PropertyName = "ID", PropertyType = "uint32", CName = "ID",Desc = "hero id"},
            //    new StructProperty(){ PropertyName = "Name",PropertyType = "string", Size = 64, CName = "Name", Desc = "hero name"},
            //    new StructProperty(){PropertyName = "SkillList",PropertyType = "uint32",Count = 5,CName = "Skill",Desc = "hero skill"}
            //};
            //configMeta.Structs.Add(configStruct);

            // XmlUtility.ToXml<ConfigMeta>(configMeta, AppDomain.CurrentDomain.BaseDirectory + "gamemeta.xml");

            //ConfigMeta config = XmlUtility.FromXml<ConfigMeta>(AppDomain.CurrentDomain.BaseDirectory + "gamemeta.xml");

            //logger.Info(config.ToString());


            ConfigurationSetting configSetting = CommandLineParser.Default.Parse<ConfigurationSetting>(args, true);

            string path = AppDomain.CurrentDomain.BaseDirectory + configSetting.ConfigConvertFilePath;
            path = path.SystemPath();
            ConfigGeneration config = XmlUtility.FromXml<ConfigGeneration>(path);

            logger.Info(config.ToString());

            string configRootPath = Path.GetDirectoryName(path);

            string excelPath = configRootPath + config.ExcelFilesPath;

            logger.Debug(excelPath);
            PrintDirAllFiles(excelPath.SystemPath());

            var files = IOHelper.GetAllFileInfos(excelPath.SystemPath());
            foreach (var file in files)
            {
                ExcelReader excel = new ExcelReader(file.FullName);
                logger.Debug(excel.ToString());
            }

            string metaPath = configRootPath + config.MetaFilesPath;

            logger.Debug(metaPath);
            PrintDirAllFiles(metaPath.SystemPath());

            if (configSetting.HelpText)
            {
                Console.WriteLine(CommandLineParser.GetAllOptionHelpText());
            }

            if(configSetting.Version)
            {
                Console.WriteLine($"ConfigurationGenerator:{Version}");
            }

            Console.ReadKey();
        }

        private static void PrintDirAllFiles(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            var files = info.GetFiles("*.*",SearchOption.AllDirectories);
            foreach (var file in files)
            {
                Console.WriteLine(file.FullName);
            }
        }

        private static void OnReigisterLogCallBack(string content, LogType logType, string stackTrace)
        {
            Console.WriteLine(content, logType, stackTrace);
        }
    }

}
