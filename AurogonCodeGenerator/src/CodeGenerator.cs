using System;
using System.Collections.Generic;
using System.Text;

namespace AurogonCodeGenerator
{
	/// <summary>
	/// 代码生成器
	/// </summary>
	public class CodeGenerator
	{
		#region 常量

		private const string NAME_SPACE_FORMAT = "using {0};";
		private const string CODE_NAME_FORMAT = "public class {0}:{1}";
		private const string CODE_PROPERTY_FORMAT = "\tpublic {0} {1}";
		private const string CODE_NOTE_FORMAT = "//{0}";

        #endregion


        private List<string> m_nameSpaceNames = null;
		private Dictionary<string,CodePropertyInfo> m_codeProperties = null;
		private string m_codeName = string.Empty;
		private string m_baseTypeName = string.Empty;
		private List<string> m_interfaces = null;

		public CodeGenerator(string codeName)
		{
			m_codeName = codeName;
            m_nameSpaceNames = new List<string>();
			m_codeProperties = new Dictionary<string, CodePropertyInfo>();
			m_interfaces = new List<string>();
        }

		private void Add<T>(List<T> list,T t)
		{
			if(list == null)
			{
				list = new List<T>();
			}

			if(list.Contains(t) == false)
			{
				list.Add(t);
			}
		}

        #region 命名空间
        /// <summary>
        /// 添加命名空间
        /// </summary>
        /// <param name="nameSpace"></param>
        public void AddNameSpace(string nameSpace)
		{
			if(string.IsNullOrEmpty(nameSpace))
			{
				return;
			}

			Add(m_nameSpaceNames, nameSpace);
		}

		/// <summary>
		/// 批量添加命名空间
		/// </summary>
		/// <param name="nameSpaces"></param>
		public void AddNameSpaces(string[] nameSpaces)
		{
			if(nameSpaces == null || nameSpaces.Length == 0)
			{
				return;
			}

			for (int i = 0; i < nameSpaces.Length; i++)
			{
				AddNameSpace(nameSpaces[i]);
			}
		}

		public string GetNameSpaceCode()
		{
			StringBuilder sb = new StringBuilder();
			if(m_nameSpaceNames != null)
			{
				for (int i = 0; i < m_nameSpaceNames.Count; i++)
				{
					sb.AppendLine(string.Format(NAME_SPACE_FORMAT, m_nameSpaceNames[i]));
				}
			}

			return sb.ToString();
		}
		#endregion

		#region 代码

		public void AddBaseType(string baseType)
		{
			m_baseTypeName = baseType;
        }

		public void AddInterface(string interfaceName)
		{
			if(string.IsNullOrEmpty(interfaceName))
			{
				return;
			}

			Add(m_interfaces, interfaceName);
		}

		public void AddInterfaces(string[] interfaceNames)
		{
			if(interfaceNames == null)
			{
				return;
			}

			for (int i = 0; i < interfaceNames.Length; i++)
			{
				AddInterface(interfaceNames[i]);
			}
		}

		public string GetCodeName()
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine(string.Format(CODE_NAME_FORMAT, m_codeName, string.IsNullOrEmpty(m_baseTypeName) ?
											string.Join(",", m_interfaces.ToArray()) : m_baseTypeName + "," + string.Join(",", m_interfaces.ToArray())));

			return sb.ToString();
		}

		#endregion

		#region 属性
		/// <summary>
		/// 添加属性成员
		/// </summary>
		/// <param name="propertyInfoName"></param>
		/// <param name="propertyType"></param>
		public void AddPropertyInfo(string propertyInfoName,Type propertyType)
		{
			if(!m_codeProperties.ContainsKey(propertyInfoName))
			{
				m_codeProperties.Add(propertyInfoName, new CodePropertyInfo(propertyInfoName, propertyType));
			}
		}

		public string GetPropertiesCode()
		{
			StringBuilder sb = new StringBuilder();
			if(m_codeProperties != null)
			{
				foreach (var property in m_codeProperties.Values)
                {
                    sb.Append(string.Format(CODE_PROPERTY_FORMAT, property.GetTypeCode(), property.Name));
					sb.Append(" { get; set; }\n");
                }
			}

			return sb.ToString();
		}

        #endregion

        public string GenerationCode()
		{
			string nameSpaceStr = GetNameSpaceCode();
			string propertiesStr = GetPropertiesCode();
			string codeName = GetCodeName();

			StringBuilder sb = new StringBuilder();

			sb.Append(nameSpaceStr);
			sb.Append(codeName);
			sb.Append("{\n");
			sb.Append(propertiesStr);
			sb.Append("}");

			return sb.ToString();
		}
	}
}

