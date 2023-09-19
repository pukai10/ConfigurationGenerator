
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AurogonCodeGenerator
{
    public class CSharpCodeGenerator : ICSharpCodeGenrator
    {
        private readonly string m_name;
        public string Name => m_name;

        private readonly string m_desc;
        public string Desc => m_desc;

        private readonly string m_baseTypeName;
        public string BaseTypeName => m_baseTypeName;

        private List<string> m_nameSpaces;
        public List<string> NameSpaces => m_nameSpaces;

        private List<CodeField> m_codeFields;
        public List<CodeField> CodeFields => m_codeFields;

        private List<string> m_interfaces;
        public List<string> Interfaces => m_interfaces;

        private CodeScriptType m_scriptType;
        public CodeScriptType ScriptType => m_scriptType;

        List<ICodeField> ICSharpCodeGenrator.CodeFields => throw new System.NotImplementedException();

        #region 构造方法

        public CSharpCodeGenerator(string name) : this(name, string.Empty) { }

        public CSharpCodeGenerator(string name, string typeName) : this(name, typeName, string.Empty) { }

        public CSharpCodeGenerator(string name, string typeName, string desc)
        {
            m_name = name;
            m_baseTypeName = typeName;
            m_desc = desc;
            m_nameSpaces = new List<string>();
            m_codeFields = new List<CodeField>();
            m_interfaces = new List<string>();
            m_scriptType = CodeScriptType.CSharp;
        }

        #endregion

        public void AddNameSpace(string nameSpace)
        {
            if (string.IsNullOrEmpty(nameSpace))
            {
                return;
            }

            m_nameSpaces.ListAdd(nameSpace);
        }

        public void AddNameSpace(string[] nameSpaces)
        {
            foreach (var nameSpace in nameSpaces)
            {
                AddNameSpace(nameSpace);
            }
        }

        public void AddInterface(string interfaceName)
        {
            if (string.IsNullOrEmpty(interfaceName))
            {
                return;
            }

            m_interfaces.ListAdd(interfaceName);
        }

        public void AddInterface(string[] interfaceNames)
        {
            foreach (var interfaceName in interfaceNames)
            {
                AddInterface(interfaceName);
            }
        }

        public void AddProperty(string propretyName, string propertyTypeName, int arrayCount = 0)
        {
            CodePropertyInfo propertyInfo = new CodePropertyInfo(propretyName, propertyTypeName, arrayCount);
            CodeFields.ListAdd(propertyInfo);
        }

        public void AddField(string fieldName, string fieldTypeName, int arrayCount = 0)
        {
            CodeField field = new CodeField(fieldName, fieldTypeName, arrayCount);
            CodeFields.ListAdd(field);
        }

        #region 代码生成

        public string GenerateCode()
        {
            string nameSpaces = GenerateNameSpaces();
            string scriptName = GenerateScriptName();
            string fields = GenerateCodeFields();
            string construct = GenerateCodeConstruct();

            StringBuilder sb = new StringBuilder();
            sb.Append(nameSpaces);
            sb.Append(scriptName);
            sb.Append("{\n");
            sb.Append(fields);
            sb.Append(construct);
            sb.Append("}");

            return sb.ToString();
        }

        public string GenerateCodeConstruct()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"\tpublic {Name}()");
            sb.AppendLine("\t{");
            string field = GenerateCodeFieldsConstruction();
            if(!string.IsNullOrEmpty(field))
            {
                sb.Append(field);
            }
            sb.AppendLine("\t}");
            return sb.ToString();
        }

        public string GenerateNameSpaces()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var nameSpace in NameSpaces)
            {
                sb.AppendLine($"using {nameSpace};");
            }

            sb.Append("\n");

            return sb.ToString();
        }

        public string GenerateScriptName()
        {
            string interfaces = GenerateInterfaces();

            if (string.IsNullOrEmpty(interfaces) && string.IsNullOrEmpty(BaseTypeName))
            {
                return $"public class {Name}\n";
            }
            else if (string.IsNullOrEmpty(BaseTypeName) && !string.IsNullOrEmpty(interfaces))
            {
                return $"public class {Name} : {interfaces}\n";
            }
            else if (!string.IsNullOrEmpty(BaseTypeName) && string.IsNullOrEmpty(interfaces))
            {
                return $"public class {Name} : {BaseTypeName}\n";
            }
            else
            {
                return $"public class {Name} : {BaseTypeName},{interfaces}\n";
            }
        }

        public string GenerateInterfaces()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Join(",", Interfaces));
            return sb.ToString();
        }

        public string GenerateCodeFields()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var field in CodeFields)
            {
                string fieldCode = field.GenerateCode();
                if(!string.IsNullOrEmpty(fieldCode))
                {
                    sb.AppendLine(fieldCode);
                }
            }
            sb.Append("\n");

            return sb.ToString();
        }

        public string GenerateCodeFieldsConstruction()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var field in CodeFields)
            {
                string fieldCode = field.GenerateCodeConstruct();
                if (!string.IsNullOrEmpty(fieldCode))
                {
                    sb.AppendLine(fieldCode);
                }
            }
            return sb.ToString();
        }
        #endregion

        public void GeneratorCodeToSave(string savePath)
        {
            string code = GenerateCode();

            string saveFile = $"{savePath}\\{Name}.cs";
            if(File.Exists(saveFile))
            {
                File.Delete(saveFile);
            }

            File.WriteAllText(saveFile, code);
        }
    }
}
