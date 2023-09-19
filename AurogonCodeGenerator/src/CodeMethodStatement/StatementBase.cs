using System;
using System.Text;

namespace AurogonCodeGenerator
{
	public abstract class StatementBase:ICodeMethodStatement
	{

        protected string m_template;
        public string Template => m_template;

        protected int m_tabCount = 0;
        public int TabCount => m_tabCount;

        public StatementBase(int tabCount)
        {
            m_tabCount = tabCount;
        }

        public abstract string GenerateCode();

        public virtual string GetTabString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < m_tabCount; i++)
            {
                sb.Append("\t");
            }

            return sb.ToString();
        }
    }
}

