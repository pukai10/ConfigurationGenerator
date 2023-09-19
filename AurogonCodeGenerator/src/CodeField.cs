using System;

namespace AurogonCodeGenerator
{
    public class CodeField : ICodeField,IEquatable<CodeField>
    {
        protected readonly string m_name;
        public string Name => m_name;

        protected readonly string m_desc;
        public string Desc => m_desc;

        protected readonly CodeFieldType m_fieldType;
        public CodeFieldType FieldType => m_fieldType;

        protected readonly string m_typeName;
        public string TypeName => m_typeName;

        protected readonly string m_baseTypeName;
        public string BaseTypeName => m_baseTypeName;

        protected readonly int m_size = 0;
        public int Size => m_size;

        protected bool m_isArray = false;
        public bool IsArray => m_isArray;

        public CodeField(string name, string fieldTypeName) : this(name, fieldTypeName, 0)
        {

        }

        public CodeField(string name, string fieldTypeName, int size) : this(name, fieldTypeName, size, string.Empty)
        {

        }

        public CodeField(string name, string fieldTypeName, int size, string desc)
        {
            m_name = name;
            m_desc = desc;
            m_typeName = fieldTypeName;
            m_size = size;
            m_isArray = m_size > 0;
            m_fieldType = ConvertCodeFieldType();
        }


        /// <summary>
        /// 构造实现
        /// </summary>
        /// <returns></returns>
        public virtual string GenerateCodeConstruct()
        {
            string typeName = GetCodeType();
            if (FieldType == CodeFieldType.Object)
            {
                return m_isArray ? $"\t\t{Name} = new {typeName}[{Size}];" : $"new {typeName}();";
            }
            else
            {
                return m_isArray ? $"\t\t{Name} = new {typeName}[{Size}];" : "";   //其他类型不是数组就不写初始化了
            }
        }

        public virtual string GenerateCode()
        {
            string typeName = GetCodeType();
            return m_isArray ? $"\tpublic {typeName}[] {Name};" : $"\tpublic {typeName} {Name};";
        }

        public virtual string GetCodeType()
        {
            switch (FieldType)
            {
                case CodeFieldType.UInt32:
                    return "uint";
                case CodeFieldType.Int32:
                    return "int";
                case CodeFieldType.UInt16:
                    return "ushort";
                case CodeFieldType.Int16:
                    return "short";
                case CodeFieldType.Byte:
                    return "byte";
                case CodeFieldType.SByte:
                    return "sbyte";
                case CodeFieldType.UInt64:
                    return "ulong";
                case CodeFieldType.Int64:
                    return "long";
                case CodeFieldType.String:
                    return "string";
                case CodeFieldType.Boolean:
                    return "bool";
                case CodeFieldType.Single:
                    return "float";
                case CodeFieldType.Double:
                    return "double";
                case CodeFieldType.Decimal:
                    return "decimal";
                default:
                    return Name;
            }
        }

        public virtual CodeFieldType ConvertCodeFieldType()
        {
            return CodeTools.ConvertCodeFieldType(TypeName);
        }

        public bool Equals(CodeField other)
        {
            if(other == null)
            {
                return false;
            }

            return other.Name.Equals(Name) && other.TypeName.Equals(TypeName) && other.Size.Equals(Size);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CodeField);
        }
    }

}
