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

        public CodePropertyInfo(string name, string fieldTypeName, int size, string desc,int tabCount = 1) : base(name, fieldTypeName, size, desc, tabCount)
        {
        }


        public override string GenerateCodeConstruct()
        {
            return base.GenerateCodeConstruct();
        }

        public override string GenerateCode()
        {
            string typeName = GetCodeType();
            return (m_isArray ? $"{m_tabStr}public {typeName}[] {Name}" : $"{m_tabStr}public {typeName} {Name}") + "{ get; set;}";
        }
    }
}

