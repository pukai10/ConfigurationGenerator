using System;
using System.Collections.Generic;
using System.Text;
using AurogonTools;

namespace AurogonCodeGenerator
{
    public class CodeMethod : ICodeMethod
    {
        protected string m_name;
        public string Name => m_name;

        private readonly NotationStatement m_desc;
        public NotationStatement Desc => m_desc;

        protected string m_argsContent;
        public string ArgsContent => m_argsContent;

        protected string m_statementsContent;
        public string StatementContent => m_statementsContent;

        protected AccessModifier m_accessModifier;
        public AccessModifier AccessMod => m_accessModifier;

        protected int m_tabCount = 0;
        public int TabCount => m_tabCount;

        protected string m_resultValue = string.Empty;
        protected string m_tabStr = string.Empty;

        public CodeMethod(string name,string statementContent):
            this(name, statementContent,AccessModifier.Public)
        {

        }

        public CodeMethod(string name,string statementContent,AccessModifier access = AccessModifier.Public,int tabCount = 1, string argsContent = "",string returnValue = "void",string desc = "")
        {
            m_name = name;
            m_argsContent = argsContent;
            m_accessModifier = access;
            m_desc = new NotationStatement(1, desc);
            m_statementsContent = statementContent;
            m_resultValue = returnValue;
            m_tabCount = tabCount;
            m_tabStr = GetTabString();
        }

        public string GenerateCode()
        {
            StringBuilder sb = new StringBuilder();
            if (m_desc.IsEmpty() == false)
            {
                sb.AppendLine(m_desc.GenerateCode());
            }
            sb.AppendLine($"{m_tabStr}{AccessMod.ToString().ToLower()} {m_resultValue} {Name}({ArgsContent})");
            sb.Append(m_tabStr);
            sb.AppendLine("{");
            sb.AppendLine(StatementContent);
            sb.Append(m_tabStr);
            sb.AppendLine("}");
            return sb.ToString();
        }

        public string GetTabString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < TabCount; i++)
            {
                sb.Append("\t");
            }

            return sb.ToString();
        }
    }
}

