using System;
using System.Collections.Generic;
using AurogonXmlConvert;

namespace ConfigurationGenerator
{
	[XmlNodeName("metalib",typeof(ConfigMeta))]
	public class ConfigMeta
	{
		[XmlAttributionName("namespace")]
		public string NameSpace { get; set; }

		[XmlAttributionName("version")]
		public string Version { get; set; }

		[XmlChildNodeList("struct",typeof(ConfigStruct))]
		public List<ConfigStruct> Structs { get; set; }
	}


	[XmlNodeName("struct",typeof(ConfigStruct))]
	public class ConfigStruct
	{
		[XmlAttributionName("name")]
		public string Name { get; set; }

		[XmlAttributionName("desc")]
		public string Desc { get; set; }

		[XmlChildNodeList("property",typeof(StructProperty))]
		public List<StructProperty> Properties { get; set; }
	}

	[XmlNodeName("property",typeof(StructProperty))]
	public class StructProperty
	{
		[XmlAttributionName("name")]
		public string PropertyName { get; set; }

		[XmlAttributionName("type")]
		public string PropertyType { get; set; }

		[XmlAttributionName("size")]
		public int Size { get; set; }

		[XmlAttributionName("count")]
		public int Count { get; set; }

		[XmlAttributionName("cname")]
		public string CName { get; set; }

		[XmlAttributionName("desc")]
		public string Desc { get; set; }
	}
}

