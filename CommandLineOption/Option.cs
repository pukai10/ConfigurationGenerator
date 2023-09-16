using System;
using System.Reflection;

namespace CommandLineOption
{
    public class OptionProperty
    {
        private readonly string m_shortName;
        private readonly string m_longName;

        private readonly string m_helpText;
        private readonly bool m_required;

        public PropertyInfo propertyInfo { get; }

        public string shortName => m_shortName;
        public string longName => m_longName;

        public string helpText => m_helpText;
        public bool required => m_required;

        public OptionProperty(PropertyInfo property,OptionAttribute attr)
        {
            propertyInfo = property;
            m_shortName = attr.shortName;
            m_longName = attr.longName;
            m_helpText = attr.helpText;
            m_required = attr.required;
        }

        public void SetPropertyValue<T>(T obj,string value)
        {
            Type propertyType = propertyInfo.PropertyType;
            if(propertyType == typeof(int))
            {
                propertyInfo.SetValue(obj, value.ToInt());
            }
            else if(propertyType == typeof(string))
            {
                propertyInfo.SetValue(obj, value);
            }
            else if(propertyType == typeof(float))
            {
                propertyInfo.SetValue(obj, value.ToFloat());
            }
            else if(propertyType == typeof(bool))
            {
                propertyInfo.SetValue(obj, value.ToBoolean(true));
            }
            else
            {
                Console.WriteLine($"property type no support,property is {propertyType.Name}");
            }
        }

        public bool IsMatch(char shortName)
        {
            return this.shortName.Equals(shortName.OneCharToString());
        }

        public bool IsMatch(string longName)
        {
            return this.longName.Equals(longName);
        }

        public bool IsMatch(string shortName,string longName)
        {
            return this.shortName.Equals(shortName) && this.longName.Equals(longName);
        }

        public override string ToString()
        {
            return $"shortName:{shortName} longName:{longName} helpText:{helpText} required:{required} property:{propertyInfo.PropertyType}";
        }
    }
}