using System;
using System.Text;

namespace AurogonCodeGenerator
{
	public abstract class ConditionStatement: StatementBase
    {

        public ConditionStatement(int tabCount):base(tabCount)
        {
            m_template = "#TAB#if(#CONDITION#)\n#TAB#{\n#TAB#\t#CONTENT#\n#TAB#}\n";
        }

        public abstract string GetCondition();

        public abstract string GetContent();

        public override string GenerateCode()
        {
            string condition = GetCondition();
            string content = GetContent();
            string tabStr = GetTabString();
            string code = m_template.Replace("#CONDITION#", condition).Replace("#CONTENT#", content).Replace("#TAB#", tabStr);
            return code;
        }
    }
}

