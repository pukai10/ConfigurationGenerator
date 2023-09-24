
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
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
        public List<string> UseNameSpaces => m_nameSpaces;

        private List<ICodeField> m_codeFields;
        public List<ICodeField> CodeFields => m_codeFields;

        private List<string> m_interfaces;
        public List<string> Interfaces => m_interfaces;

        private CodeScriptType m_scriptType;
        public CodeScriptType ScriptType => m_scriptType;

        private List<ICodeMethod> m_methods;
        public List<ICodeMethod> Methods => m_methods;

        private string m_nameSpace;
        public string NameSpace => m_nameSpace;

        #region 构造方法

        public CSharpCodeGenerator(string name) : this(name, string.Empty) { }

        public CSharpCodeGenerator(string name, string typeName) : this(name, typeName, string.Empty) { }

        public CSharpCodeGenerator(string name, string typeName, string desc)
        {
            m_name = name;
            m_baseTypeName = typeName;
            m_desc = desc;
            m_nameSpaces = new List<string>();
            m_codeFields = new List<ICodeField>();
            m_interfaces = new List<string>();
            m_methods = new List<ICodeMethod>();
            m_scriptType = CodeScriptType.CSharp;
        }

        #endregion

        public void SetNameSpace(string nameSpace)
        {
            m_nameSpace = nameSpace;
        }

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

        public void AddProperty(string propretyName, string propertyTypeName, int arrayCount = 0,string desc = "")
        {
            int tabCount = GetTabCount();
            CodePropertyInfo propertyInfo = new CodePropertyInfo(propretyName, propertyTypeName, arrayCount,desc,tabCount + 1);
            CodeFields.ListAdd(propertyInfo);
        }

        public void AddField(string fieldName, string fieldTypeName, int arrayCount = 0, string desc = "")
        {
            int tabCount = GetTabCount();
            CodeField field = new CodeField(fieldName, fieldTypeName, arrayCount, desc, tabCount + 1);
            CodeFields.ListAdd(field);
        }

        public void AddMethod(ICodeMethod codeMethod)
        {
            Methods.Add(codeMethod);
        }



        #region 代码生成

        public string GenerateCode()
        {
            string nameSpaces = GenerateNameSpaces();
            string scriptName = GenerateScriptName();
            string fields = GenerateCodeFields();
            string construct = GenerateCodeConstruct();
            string methods = GenerateMethodCode();
            string tabStr = GetTabString();
            StringBuilder sb = new StringBuilder();
            sb.Append(nameSpaces);

            if(!string.IsNullOrEmpty(NameSpace))
            {
                sb.Append($"namesapce {NameSpace}\n");
                sb.Append("{\n");
            }

            sb.Append(scriptName);
            sb.Append(tabStr);
            sb.Append("{\n");
            sb.Append(fields);
            sb.Append(construct);
            sb.Append(methods);
            sb.Append(tabStr);
            sb.Append("}\n");


            if (!string.IsNullOrEmpty(NameSpace))
            {
                sb.Append("}");
            }
            return sb.ToString();
        }

        public int GetTabCount()
        {
            return string.IsNullOrEmpty(NameSpace) ? 0 : 1;
        }

        public string GenerateCodeConstruct()
        {
            StringBuilder sb = new StringBuilder();

            string tabStr = GetTabString();
            sb.AppendLine($"{tabStr}\tpublic {Name}()");
            sb.Append(tabStr);
            sb.AppendLine("\t{");
            string field = GenerateCodeFieldsConstruction();
            if(!string.IsNullOrEmpty(field))
            {
                sb.Append(field);
            }
            sb.Append(tabStr);
            sb.AppendLine("\t}\n");
            return sb.ToString();
        }

        public string GenerateNameSpaces()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var nameSpace in UseNameSpaces)
            {
                sb.AppendLine($"using {nameSpace};");
            }

            sb.Append("\n");

            return sb.ToString();
        }

        public string GetTabString()
        {
            int tabCount = GetTabCount();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < tabCount; i++)
            {
                sb.Append("\t");
            }

            return sb.ToString();
        }


        public string GenerateScriptName()
        {
            string interfaces = GenerateInterfaces();
            string tabStr = GetTabString();
            if (string.IsNullOrEmpty(interfaces) && string.IsNullOrEmpty(BaseTypeName))
            {
                return $"{tabStr}public class {Name}\n";
            }
            else if (string.IsNullOrEmpty(BaseTypeName) && !string.IsNullOrEmpty(interfaces))
            {
                return $"{tabStr}public class {Name} : {interfaces}\n";
            }
            else if (!string.IsNullOrEmpty(BaseTypeName) && string.IsNullOrEmpty(interfaces))
            {
                return $"{tabStr}public class {Name} : {BaseTypeName}\n";
            }
            else
            {
                return $"{tabStr}public class {Name} : {BaseTypeName},{interfaces}\n";
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

        public string GenerateMethodCode()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var codeMethod in Methods)
            {
                sb.AppendLine(codeMethod.GenerateCode());
            }

            return sb.ToString();
        }

        #endregion

        public void GeneratorCodeToSave(string savePath)
        {
            string code = GenerateCode();

            string saveFile = $"{savePath}\\{Name}.cs";

#if SYSTEM_MACOS
            saveFile = saveFile.Replace("\\", "/");
#endif
            if (File.Exists(saveFile))
            {
                File.Delete(saveFile);
            }

            File.WriteAllText(saveFile, code);
        }
    }
}
