using System;
using CommandLineOption;
using AurogonTools;
using AurogonXmlConvert;
using System.IO;
using System.Collections.Generic;
using GameConfigurationMode;

namespace ConfigurationGenerator
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            args = new string[]
            {
                "--path",
                "..\\..\\..\\Config\\GameConfigConvert.xml"
            };
            AurogonVersion.SetVersion(Common.Version);

            ConfigurationSetting configSetting = CommandLineParser.Default.Parse<ConfigurationSetting>(args, true);
            ConfigurationConvert configurationConvert = new ConfigurationConvert(configSetting);
            configurationConvert.Convert();

        //    OutMessages msgs = new OutMessages("ConfigurationConvert");

        //    msgs.AddMsg("Info ≤‚ ‘1");
        //    msgs.AddMsg("Warning ≤‚ ‘1", OutMsgType.Warning);
        //    msgs.AddMsg("Info ≤‚ ‘2");
        //    msgs.AddMsg("Info ≤‚ ‘3");
        //    msgs.AddMsg("Error ≤‚ ‘1", OutMsgType.Error);
        //    msgs.AddMsg("Warning ≤‚ ‘2", OutMsgType.Warning);
        //    msgs.AddMsg("Info ≤‚ ‘4");
        //    msgs.AddMsg("Error ≤‚ ‘2", OutMsgType.Error);
        //    msgs.AddMsg("Info ≤‚ ‘5");
        //    msgs.AddMsg("Error ≤‚ ‘3", OutMsgType.Error);
        //    msgs.AddMsg("Warning ≤‚ ‘3", OutMsgType.Warning);
        //    msgs.AddMsg("Info ≤‚ ‘5");
        //    msgs.AddMsg("Info ≤‚ ‘7");
        //    msgs.AddMsg("Error ≤‚ ‘4", OutMsgType.Error);
        //    msgs.AddMsg("Warning ≤‚ ‘4", OutMsgType.Warning);
        //    msgs.AddMsg("Info ≤‚ ‘8");
        //    msgs.AddMsg("Info ≤‚ ‘9");
        //    msgs.AddMsg("Error ≤‚ ‘5", OutMsgType.Error);
        //    msgs.AddMsg("Warning ≤‚ ‘5", OutMsgType.Warning);
        //    msgs.AddMsg("Error ≤‚ ‘6", OutMsgType.Error);


        //    System.Action<List<OutMessage>> action = (List<OutMessage> msg) =>
        //{
        //    for (int i = 0; i < msg.Count; i++)
        //    {
        //        Console.WriteLine(msg[i].ToString());
        //    }
        //};

        //    var infoMsgs = msgs.InfoMsgs();
        //    Console.WriteLine("InfoMsgs");

        //    action(infoMsgs);

        //    Console.WriteLine("WarningMsgs");
        //    var waringMsgs = msgs.WarningMsgs();
        //    action(waringMsgs);

        //    Console.WriteLine("ErrorMsgs");
        //    var errors = msgs.ErrorMsgs();
        //    action(errors);






            //logger = Logger.GetLogger(new LoggerSetting() { logType = LogType.All });

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



            //string path = AppDomain.CurrentDomain.BaseDirectory + configSetting.ConfigConvertFilePath;
            //path = path.SystemPath();
            //ConfigGeneration config = XmlUtility.FromXml<ConfigGeneration>(path);

            //logger.Debug(config.ToString());
            //string configRootPath = Path.GetDirectoryName(path);
            //Dictionary<string, ExportGameConfig> exports = ConvertExportGameConfigInfos(config, configRootPath);

            //string excelPath = configRootPath + config.ExcelFilesPath;
            //string metaPath = configRootPath + config.MetaFilesPath;
            //logger.Debug($"metaPath:{metaPath}");
            //logger.Debug($"excelPath:{excelPath}");

            //foreach (var export in exports.Values)
            //{
            //    ExcelReader excel = new ExcelReader(export.ExcelFile);
            //    for (int i = 0; i < export.ExportList.Count; i++)
            //    {
            //        var exportInfo = export.ExportList[i];
            //        var sheet = excel[exportInfo.SheetName];
            //        if (sheet == null)
            //        {
            //            logger.Error($"{excel.ExcelName} not has sheet:{exportInfo.SheetName}");
            //            return;
            //        }

            //        var configMeta = XmlUtility.FromXml<ConfigMeta>(exportInfo.MetaFilePath);

            //        if (configMeta == null)
            //        {
            //            logger.Error($"{exportInfo.MetaFilePath} convert to ConfigMeta struct error,please check it");
            //            return;
            //        }

            //        exportInfo.exportRowInfoList = new List<ExportGameConfigRowInfo>();

            //        ExportGameConfigRowInfo info = new ExportGameConfigRowInfo();

            //        ConfigStruct configStruct = configMeta[exportInfo.StructName];
            //    }
            //}



            if (configSetting.HelpText)
            {
                Console.WriteLine(CommandLineParser.GetAllOptionHelpText());
            }

            if (configSetting.Version)
            {
                Console.WriteLine($"ConfigurationGenerator:{AurogonVersion.Default.Version}");
            }

            Console.ReadKey();
        }

        //private static Dictionary<string, ExportGameConfig> ConvertExportGameConfigInfos(ConfigGeneration config, string configPath)
        //{
        //    if (config == null || config.ConvertTree == null)
        //    {
        //        return null;
        //    }
        //    Dictionary<string, ExportGameConfig> dict = new Dictionary<string, ExportGameConfig>();
        //    ConfigConvertTree tree = config.ConvertTree;
        //    if (tree == null || tree.ExcelNodes == null)
        //    {
        //        return null;
        //    }

        //    for (int j = 0; j < tree.ExcelNodes.Count; j++)
        //    {
        //        var node = tree.ExcelNodes[j];
        //        if (node == null || node.SheetNodes == null)
        //        {
        //            continue;
        //        }

        //        ExportGameConfig export = null;
        //        if (!dict.TryGetValue(node.Name, out export))
        //        {
        //            export = new ExportGameConfig();
        //            export.ExcelFile = IOHelper.ConvertPath(configPath, config.ExcelFilesPath, node.ExcelName);
        //            export.ExportList = new List<ExportGameConfigInfo>();
        //        }

        //        for (int i = 0; i < node.SheetNodes.Count; i++)
        //        {
        //            var sheetNode = node.SheetNodes[i];
        //            if (sheetNode == null)
        //            {
        //                continue;
        //            }

        //            ExportGameConfigInfo info = new ExportGameConfigInfo();
        //            info.ExcelName = node.ExcelName;
        //            info.ExportBytesPath = IOHelper.ConvertPath(configPath, config.ExportFilePath, sheetNode.BinaryFile);
        //            info.MetaFilePath = IOHelper.ConvertPath(configPath, config.MetaFilesPath, sheetNode.MetaFile);
        //            info.StructName = sheetNode.StructName;
        //            info.SheetName = sheetNode.SheetName;

        //            export.AddExportInfo(info);
        //        }

        //        dict[node.ExcelName] = export;

        //        logger.Debug(export.ToString());
        //    }

        //    return dict;
        //}

        //private static void OnReigisterLogCallBack(string content, LogType logType, string stackTrace)
        //{
        //    Console.WriteLine(content, logType, stackTrace);
        //}
    }

}
