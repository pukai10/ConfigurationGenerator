

using System.Collections.Generic;

namespace AurogonCodeGenerator
{
    /// <summary>
    /// 代码字段类型
    /// </summary>
    public enum CodeFieldType
    {
        /// <summary>
        /// 未知类型
        /// </summary>
        Unkown = 0,
        Byte,
        SByte,
        UInt16,
        Int16,
        UInt32,
        Int32,
        UInt64,
        Int64,
        Single,
        Double,
        Decimal,
        String,
        Boolean,
        Object,
    }

    public interface ICodeField: ICodeBase
    {
        CodeFieldType FieldType { get; }

        int Size { get; }

        bool IsArray { get; }

        string GetCodeType();

        CodeFieldType ConvertCodeFieldType();
    }

    public interface ICodeBase
    {
        string Name { get; }
        string Desc { get; }
        string BaseTypeName { get; }

        /// <summary>
        /// 代码创建
        /// </summary>
        /// <returns></returns>
        string GenerateCode();

        /// <summary>
        /// 构造的实现
        /// </summary>
        /// <returns></returns>
        string GenerateCodeConstruct();
    }

    public interface ICodeGenerator: ICodeBase
    {
        CodeScriptType ScriptType { get; }

        #region 代码保存

        void GeneratorCodeToSave(string savePath);

        #endregion
    }

    public interface ICSharpCodeGenrator: ICodeGenerator
    {
        List<string> NameSpaces { get; }
        List<ICodeField> CodeFields { get; }
        List<string> Interfaces { get; }

        #region 添加类的组成元素
        void AddNameSpace(string nameSpace);

        void AddNameSpace(string[] nameSpaces);

        void AddInterface(string interfaceName);

        void AddInterface(string[] interfaceNames);

        void AddProperty(string propretyName, string propertyTypeName, int arrayCount = 0);

        void AddField(string fieldName,string fieldTypeName,int arrayCount = 0);
        #endregion

        #region 代码生成

        string GenerateNameSpaces();

        string GenerateScriptName();

        string GenerateInterfaces();

        string GenerateCodeFields();

        string GenerateCodeFieldsConstruction();

        #endregion

    }

    public enum CodeScriptType
    {
        CSharp,
        CPlusPlus,
        C,
    }


}
