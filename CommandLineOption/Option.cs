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
    }
}