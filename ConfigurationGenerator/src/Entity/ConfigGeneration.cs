using System;
using AurogonXmlConvert;
using System.Collections.Generic;

namespace ConfigurationGenerator
{
	[XmlNodeName("ConvertList",typeof(ConfigGeneration))]
	public class ConfigGeneration
    {
        [XmlAttributionName("MetaFilePath")]
        public string MetaFilesPath { get; set; }

		[XmlAttributionName("ExcelFilePath")]
		public string ExcelFilesPath { get; set; }

        [XmlChildNodeList("ConfigConvertTree", typeof(ExcelNode))]
        public List<ConfigConvertTree> ConvertTree { get; set; }
    }

	[XmlNodeName("ConfigConvertTree", typeof(ConfigConvertTree))]
	public class ConfigConvertTree
    {

		[XmlAttributionName("Name")]
		public string Name { get; set; }

		[XmlChildNodeList("ExcelNode", typeof(ExcelNode))]
		public List<ExcelNode> ExcelNodes { get; set; }
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
    }
}

