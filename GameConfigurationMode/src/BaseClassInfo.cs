using System;
using System.Collections.Generic;

namespace GameConfigurationMode
{
    [Serializable]
	public class BaseClassInfo
	{
        public string Name;

        public int Size;

        public List<string> OtherNames;

        public bool IsCustom;

        public string DefaultValue;

        public BaseClassInfo(string name, int size, List<string> otherNames,string defaultValue) : this(name, size, otherNames, false, defaultValue)
        {

        }

        public BaseClassInfo(string name, int size, List<string> otherNames, bool isCustom,string defaultValue)
        {
            Name = name;
            Size = size;
            OtherNames = otherNames;
            IsCustom = isCustom;
            DefaultValue = defaultValue;
        }

        public bool IsSelfType(string typeName)
        {
            if (Name.Equals(typeName))
            {
                return true;
            }

            if (OtherNames != null && OtherNames.Contains(typeName))
            {
                return true;
            }

            return false;
        }
    }
}

