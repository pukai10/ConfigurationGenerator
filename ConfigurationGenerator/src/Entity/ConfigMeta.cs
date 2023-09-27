using System;
using System.Collections.Generic;
using System.Text;
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
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine($"NameSpace:{NameSpace} Version:{Version}");
			foreach (var str in Structs)
			{
				sb.AppendLine($"\t{str.ToString()}");
			}
			return sb.ToString();
		}
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

        public override string ToString()
        {
			StringBuilder sb = new StringBuilder();
			sb.AppendLine($"Name:{Name} Desc:{Desc}");
            foreach (var property in Properties)
            {
				sb.AppendLine($"\t{property.ToString()}");
            }
            return sb.ToString();
        }
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

        public override string ToString()
        {
			return $"PropertyName:{PropertyName} PropertyType:{PropertyType} Size:{Size} Count:{Count} CName:{CName} Desc:{Desc}";
        }
    }
}

