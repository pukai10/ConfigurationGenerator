using System;
using System.Collections.Generic;
using System.Text;
using AurogonXmlConvert;

namespace GameConfigurationMode
{
	[XmlNodeName("metalib",typeof(ConfigMeta))]
	public class ConfigMeta
	{
		[XmlAttributionName("namespace")]
		public string NameSpace { get; set; }

		[XmlAttributionName("name")]
		public string Name { get; set; }

		[XmlAttributionName("version")]
		public string Version { get; set; }

		[XmlChildNodeList("constant",typeof(ConfigConst))]
		public List<ConfigConst> Constants { get; set; }

		[XmlChildNodeList("struct",typeof(ConfigStruct))]
		public List<ConfigStruct> Structs { get; set; }

		public ConfigStruct this[string structName]
		{
			get
			{
				if (Structs == null)
				{
					return null;
				}

				return Structs.Find((item) => { return item.Name == structName; });
			}

		}

		public int GetConstValue(string name)
		{
			int value = -1;
			if (string.IsNullOrEmpty(name))
			{
				return -1;
			}

			if(int.TryParse(name,out value))
			{
				return value;
			}

			if(Constants != null)
			{
				return Constants.Find((item) => item.Name == name).Value;
			}

			return -1;
		}

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

	[XmlNodeName("constant",typeof(ConfigConst))]
	public class ConfigConst
	{
		[XmlAttributionName("name")]
		public string Name { get; set; }
        [XmlAttributionName("value")]
        public int Value { get; set; }
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

		public StructProperty this[string name]
		{
			get
			{
				if (Properties == null)
				{
					return null;
				}

				for (int i = 0; i < Properties.Count; i++)
				{
					StructProperty prop = Properties[i];
					if (prop != null && prop.CName == name)
					{
						return prop;
					}
				}

				return null;
			}
		}

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
		public string Size { get; set; }

		[XmlAttributionName("count")]
		public string Count { get; set; }

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

