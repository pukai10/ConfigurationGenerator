using System;
using System.Collections.Generic;
using AurogonTools;

namespace AurogonCodeGenerator
{
    public class CodeMethod : ICodeMethod
    {
        private readonly string m_name;
        public string Name => m_name;

        private readonly string m_desc;
        public string Desc => m_desc;

        private readonly List<KeyValue<string, string>> m_args;
        public List<KeyValue<string, string>> Args => m_args;

        public CodeMethod(string name):this(name,null,string.Empty)
        {

        }

        public CodeMethod(string name,List<KeyValue<string,string>> args):this(name,args,string.Empty)
        {
        }

        public CodeMethod(string name,List<KeyValue<string,string>> args, string desc)
        {
            m_name = name;
            m_args = args;
            m_desc = desc;
        }

        public void AddMethodContent()
        {

        }

        public string GenerateCode()
        {
            throw new NotImplementedException();
        }
    }
}

