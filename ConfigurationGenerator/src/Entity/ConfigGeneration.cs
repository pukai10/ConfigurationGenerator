using System;
using AurogonXmlConvert;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationGenerator
{
	[XmlNodeName("ConvertList", typeof(ConfigGeneration))]
	public class ConfigGeneration
    {
        [XmlAttributionName("MetaFilePath")]
        public string MetaFilesPath { get; set; }

		[XmlAttributionName("ExcelFilePath")]
		public string ExcelFilesPath { get; set; }

        [XmlChildNodeList("ConfigConvertTree", typeof(ConfigConvertTree))]
        public List<ConfigConvertTree> ConvertTree { get; set; }

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine("");
			sb.AppendLine($"MetaFilesPath:{MetaFilesPath}");
			sb.AppendLine($"ExcelFilesPath:{ExcelFilesPath}");

            foreach (var tree in ConvertTree)
            {
				sb.AppendLine(tree.ToString());
            }

			return sb.ToString();
		}
	}

	[XmlNodeName("ConfigConvertTree", typeof(ConfigConvertTree))]
	public class ConfigConvertTree
    {

		[XmlAttributionName("Name")]
		public string Name { get; set; }

		[XmlChildNodeList("ExcelNode", typeof(ExcelNode))]
		public List<ExcelNode> ExcelNodes { get; set; }
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine($"\tConfigConvertTree Name:{Name}");
			foreach (var node in ExcelNodes)
			{
				sb.AppendLine(node.ToString());
			}
			return sb.ToString();
		}
	}


	[XmlNodeName("ExcelNode",typeof(ExcelNode))]
	public class ExcelNode
	{
		[XmlAttributionName("Name")]
		public string Name { get; set; }

		[XmlAttributionName("ExcelName")]
		public string ExcelName { get; set; }

		[XmlChildNodeList("ExcelSheetNode",typeof(ExcelSheetNode))]
		public List<ExcelSheetNode> SheetNodes { get; set; }

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine($"\t\tExcelNode Name:{Name}");
			sb.AppendLine($"\t\tExcelNode ExcelName:{ExcelName}");
			foreach (var node in SheetNodes)
			{
				sb.AppendLine(node.ToString());
			}
			return sb.ToString();
		}
	}

	[XmlNodeName("ExcelSheetNode",typeof(ExcelSheetNode))]
	public class ExcelSheetNode
    {
        [XmlAttributionName("name")]
        public string Name { get; set; }

		[XmlAttributionName("sheet")]
		public string SheetName { get; set; }

		[XmlAttributionName("struct")]
		public string StructName { get; set; }

		[XmlAttributionName("binary")]
		public string BinaryFile { get; set; }

		[XmlAttributionName("meta")]
		public string MetaFile { get; set; }

        public override string ToString()
        {
			return $"\t\t\tName:{Name} SheetName:{SheetName} StructName:{StructName} BinaryFile:{BinaryFile} MetaFile:{MetaFile}";
        }
    }
}

