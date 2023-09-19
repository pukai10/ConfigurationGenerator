using System;
namespace AurogonCodeGenerator
{
	public abstract class ForStatement:StatementBase
	{
		public ForStatement(int tabCount):base(tabCount)
		{
            m_template = "#TAB#for(int i = 0;i < #COUNT#; i++)\n#TAB#{\n#TAB#\t#CONTENT#\n#TAB#}\n";
		}

        public abstract int GetCount();

        public abstract string GetContent();

        public override string GenerateCode()
        {
            int count = GetCount();
            string content = GetContent();
            string tabStr = GetTabString();
            string code = m_template.Replace("#TAB#", tabStr).Replace("#COUNT#", count.ToString()).Replace("#CONTENT#", content);
            return code;
        }
    }
}

