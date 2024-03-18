using System;
using CommandLineOption;
using AurogonTools;
using System.IO;
using AurogonXmlConvert;
using AurogonCodeGenerator;
using GameConfigurationMode;
using System.Reflection;

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
            foreach (var file in infos)
			{
				m_logger.Info(file.FullName);

                if (file.FullName.Contains(".DS_Store"))
                {
                    continue;
                }
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
						int count = string.IsNullOrEmpty(property.Count) ? 0 : (int.TryParse(property.Count,out int _) ? int.Parse(property.Count) : configMeta.GetConstValue(property.Count));
                        csharpClass.AddField(property.PropertyName, property.PropertyType == "string" ? "uint32":property.PropertyType, count, property.Desc);
					}
                    PackStatement packStatement = new PackStatement(codeGenerator.TabCount + 2, csharpClass.CodeFields);
                    UnPackStatement unPackStatement = new UnPackStatement(codeGenerator.TabCount + 2, csharpClass.CodeFields);

                    PackMethod packMethod = new PackMethod("Pack", packStatement.GenerateCode(), codeGenerator.TabCount + 1);
                    UnPackMethod unpackMethod = new UnPackMethod("UnPack", unPackStatement.GenerateCode(), codeGenerator.TabCount + 1);

                    csharpClass.AddMethod(packMethod);
                    csharpClass.AddMethod(unpackMethod);

                    codeGenerator.AddClass(csharpClass);
				}
				m_logger.Info(codeGenerator.Name);
                codeGenerator.GeneratorCodeToSave(AppDomain.CurrentDomain.BaseDirectory);
            }
		}
	}
}

