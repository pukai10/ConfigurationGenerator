using System;
namespace AurogonCodeGenerator
{
    public class CodePropertyInfo : CodeField
    {
        public CodePropertyInfo(string name, string fieldTypeName) : base(name, fieldTypeName, 0)
        {

        }

        public CodePropertyInfo(string name, string fieldTypeName, int size) : base(name, fieldTypeName, size, string.Empty)
        {

        }

        public CodePropertyInfo(string name, string fieldTypeName, int size, string desc) : base(name, fieldTypeName, size, desc)
        {
        }


        public override string GenerateCodeConstruct()
        {
            return base.GenerateCodeConstruct();
        }

        public override string GenerateCode()
        {
            string typeName = GetCodeType();
            return (m_isArray ? $"\tpublic {typeName}[] {Name}" : $"\tpublic {typeName} {Name}") + "{ get; set;}";
        }
    }
}

