using System;
namespace AurogonCodeGenerator
{
	public class CodePropertyInfo:CodeMemberInfo
	{
		private Type m_type = null;
		private bool m_isArray = false;
		public bool IsArray => m_isArray;

		public CodePropertyInfo(string name) :base(name)
		{
		}
		public CodePropertyInfo(string name,Type type):this(name,type,false)
		{
		}

		public CodePropertyInfo(string name, Type type,bool isArray):base(name)
		{
			m_type = type;
			m_isArray = isArray;
        }


		public string GetTypeCode()
		{
			string typeName = m_type.Name;
			switch(typeName)
			{
				case "UInt32":
					return "uint";
				case "Int32":
					return "int";
				case "UInt16":
					return "ushort";
				case "Int16":
					return "short";
				case "Byte":
					return "byte";
				case "SByte":
					return "sbyte";
				case "UInt64":
					return "ulong";
				case "Int64":
					return "long";
				case "String":
					return "string";
				case "Boolean":
					return "bool";
				case "Single":
					return "float";
				case "Double":
					return "double";
				default:
					return typeName;
			}
		}
	}
}

