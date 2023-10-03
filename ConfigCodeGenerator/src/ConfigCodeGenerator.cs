using System;
using CommandLineOption;
using AurogonTools;
using System.IO;
using AurogonXmlConvert;
using AurogonCodeGenerator;


namespace ConfigCodeGenerator
{
	public class ConfigCodeGenerator
	{
		[Option("meta","meta-path",helpText = "C# Meta XML 文件夹路径",required = true)]
		public string MetaPath { get; set; }

        [Option("v", "version", helpText = "当前版本号", required = false)]
        public bool IsShowVersion { get; set; }

		private ILogger m_logger = null;
		public ConfigCodeGenerator()
        {
            m_logger = Logger.GetLogger("CodeGenerator");
			MetaPath = "../../../Config/Meta";
			m_logger.Debug("ConfigCodeGenerator:" + MetaPath);

        }

		public void Parse()
		{
            m_logger.Debug(MetaPath);

			if(string.IsNullOrEmpty(MetaPath))
			{
                m_logger.Error("Meta Path is empty");
				return;
			}

			FileInfo[] infos = IOHelper.GetAllFileInfos(MetaPath);


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
            foreach (var file in infos)
			{
				m_logger.Info(file.FullName);
				var configMeta = XmlUtility.FromXml<ConfigMeta>(file.FullName);
				CSharpCodeGenerator codeGenerator = new CSharpCodeGenerator(configMeta.Name);
				codeGenerator.SetNameSpace(configMeta.NameSpace);
                codeGenerator.AddUseNameSpace("AurogonWRBuffer");

				foreach (var cfgStruct in configMeta.Structs)
				{
					ICSharpClassGenrator csharpClass = new CSharpClass(cfgStruct.Name, codeGenerator.TabCount, string.Empty,cfgStruct.Desc);
					csharpClass.AddInterface("IPackage");
					foreach (var property in cfgStruct.Properties)
					{
                        csharpClass.AddField(property.PropertyName, property.PropertyType == "string" ? "uint32":property.PropertyType, property.Count, property.Desc);
					}
                    PackStatement packStatement = new PackStatement(codeGenerator.TabCount + 2, csharpClass.CodeFields);
                    UnPackStatement unPackStatement = new UnPackStatement(codeGenerator.TabCount + 2, csharpClass.CodeFields);

                    PackMethod packMethod = new PackMethod("Pack", packStatement.GenerateCode(), codeGenerator.TabCount + 1);
                    UnPackMethod unpackMethod = new UnPackMethod("UnPack", unPackStatement.GenerateCode(), codeGenerator.TabCount + 1);

                    csharpClass.AddMethod(packMethod);
                    csharpClass.AddMethod(unpackMethod);

                    codeGenerator.AddClass(csharpClass);
				}

                codeGenerator.GeneratorCodeToSave(AppDomain.CurrentDomain.BaseDirectory);
            }
		}
	}
}

