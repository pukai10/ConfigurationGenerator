
using AurogonTools;
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

        int TabCount { get; }

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
        List<string> UseNameSpaces { get; }
        List<ICodeField> CodeFields { get; }
        List<string> Interfaces { get; }
        List<ICodeMethod> Methods { get; }
        string NameSpace { get; }

        #region 添加类的组成元素
        void AddNameSpace(string nameSpace);

        void AddNameSpace(string[] nameSpaces);

        void AddInterface(string interfaceName);

        void AddInterface(string[] interfaceNames);

        void AddProperty(string propretyName, string propertyTypeName, int arrayCount = 0,string desc = "");

        void AddField(string fieldName,string fieldTypeName,int arrayCount = 0,string desc = "");

        void AddMethod(ICodeMethod codeMethod);
        #endregion

        #region 代码生成

        string GenerateNameSpaces();

        string GenerateScriptName();

        string GenerateInterfaces();

        string GenerateCodeFields();

        string GenerateCodeFieldsConstruction();

        #endregion
        string GetTabString();

        int GetTabCount();
    }

    public enum CodeScriptType
    {
        CSharp,
        CPlusPlus,
        C,
    }

    public enum AccessModifier
    {
        Private,
        Public,
        Protected
    }

    /// <summary>
    /// 代码方法接口
    /// </summary>
    public interface ICodeMethod
    {
        /// <summary>
        /// 方法名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 注释
        /// </summary>
        NotationStatement Desc { get; }

        /// <summary>
        /// 方法参数
        /// </summary>
        string ArgsContent { get; }

        string StatementContent { get; }

        AccessModifier AccessMod { get; }

        int TabCount { get; }

        string GetTabString();

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <returns></returns>
        string GenerateCode();
    }

    /// <summary>
    /// 代码方法语句接口
    /// </summary>
    public interface ICodeMethodStatement
    {
        /// <summary>
        /// 语句模板
        /// </summary>
        string Template { get; }

        /// <summary>
        /// 制表符个数
        /// </summary>
        int TabCount { get; }

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <returns></returns>
        string GenerateCode();

        /// <summary>
        /// 获取制表符
        /// </summary>
        /// <returns></returns>
        string GetTabString();
    }
}
