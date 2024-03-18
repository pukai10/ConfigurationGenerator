using System;
using System.Collections.Generic;

namespace ConfigurationGenerator
{
	public class ClassInfo
	{
		public string Name;

		public int Size;

		public List<string> OtherNames;

		public bool IsCustom;

		public ClassInfo(string name, int size,List<string> otherNames) : this(name,size, otherNames, false)
		{

		}

		public ClassInfo(string name,int size,List<string> otherNames,bool isCustom)
		{
			Name = name;
			Size = size;
			OtherNames = otherNames;
			IsCustom = isCustom;
		}

		public bool IsSelfType(string typeName)
		{
			if(Name.Equals(typeName))
			{
				return true;
			}

			if(OtherNames != null && OtherNames.Contains(typeName))
			{
				return true;
			}

			return false;
		}
	}
}

