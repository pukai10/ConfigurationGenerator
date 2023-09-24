using System;
namespace AurogonCodeGenerator
{
    /// <summary>
    /// 注释语句
    /// </summary>
    public class NotationStatement : StatementBase
    {
        private string m_content = string.Empty;

        private readonly string m_argTemplate = "/// <param name=\"#ARGNAME#\">#ARGCONTENT#</param>";
        private readonly string m_returnValueTemplate = "/// <returns>#RETURNCONTENT#</returns>";

        public NotationStatement(int tabCount,string content):base(tabCount)
        {
            m_content = content;
            m_template = "#TAB#/// <summary>\n#TAB#/// #CONTENT#\n#TAB#/// </summary>";
        }

        public override string GenerateCode()
        {
            if(IsEmpty())
            {
                return string.Empty;
            }
            string tabStr = GetTabString();
            string code = m_template.Replace("#TAB#", tabStr).Replace("#CONTENT#", m_content);
            return code;
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(m_content);
        }

    }
}

