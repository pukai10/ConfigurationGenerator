using System;
namespace AurogonCodeGenerator
{
    public class PackMethod : CodeMethod
    {
        public PackMethod(string name, string statementContent,int tabCount) : base(name, statementContent,tabCount:tabCount)
        {
            m_name = "Pack";
            m_accessModifier = AccessModifier.Public;
            m_argsContent = "ref WriteBuffer writer";
            m_resultValue = "WRErrorType";
        }

    }

    public class UnPackMethod : CodeMethod
    {
        public UnPackMethod(string name, string statementContent, int tabCount) : base(name, statementContent, tabCount: tabCount)
        {
            m_name = "UnPack";
            m_accessModifier = AccessModifier.Public;
            m_argsContent = "ref ReadBuffer reader";
            m_resultValue = "WRErrorType";
        }
    }
}

