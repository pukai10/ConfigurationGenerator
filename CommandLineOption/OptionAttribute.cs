using System;

namespace CommandLineOption
{
    /// <summary>
    /// 命令参数特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class OptionAttribute : BaseAttribute
    {
        private readonly string m_shortName;
        private readonly string m_longName;

        public string shortName => m_shortName;
        public string longName => m_longName;

        public OptionAttribute(string shortName, string longName)
        {
            m_shortName = shortName;
            m_longName = longName;
        }

        public OptionAttribute(char shortName): this(shortName.OneCharToString(), string.Empty) { }

        public OptionAttribute(string longName) : this(string.Empty, longName) { }

        public OptionAttribute(char shortName,string longName) : this(shortName.OneCharToString(), longName) { }

    }


    public abstract class BaseAttribute:Attribute
    {
        private string m_helpText;
        private bool m_required;

        public string helpText
        {
            get { return m_helpText; }
            set { m_helpText = value; }
        }
        public bool required
        {
            get { return m_required; }
            set { m_required = value; }
        }

        public BaseAttribute()
        {
            m_required = false;
            m_helpText = string.Empty;
        }
    }

}
