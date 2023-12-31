﻿using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace AurogonCodeGenerator
{
	public class CSharpClass:ICSharpClassGenrator
	{

        private readonly string m_name;
        public string Name => m_name;

        private readonly string m_desc;
        public string Desc => m_desc;

        private readonly string m_baseTypeName;
        public string BaseTypeName => m_baseTypeName;

        private List<ICodeField> m_codeFields;
        public List<ICodeField> CodeFields => m_codeFields;

        private List<string> m_interfaces;
        public List<string> Interfaces => m_interfaces;

        private CodeScriptType m_scriptType;
        public CodeScriptType ScriptType => m_scriptType;

        private List<ICodeMethod> m_methods;
        public List<ICodeMethod> Methods => m_methods;

        private int m_tabCount = 0;
        public int TabCount => m_tabCount;

        #region 构造方法

        public CSharpClass(string name,int tabCount) : this(name, tabCount,string.Empty) { }

        public CSharpClass(string name, int tabCount, string typeName) : this(name, tabCount,typeName, string.Empty) { }

        public CSharpClass(string name, int tabCount,string typeName, string desc)
        {
            m_name = name;
            m_baseTypeName = typeName;
            m_desc = desc;
            m_tabCount = tabCount;
            m_codeFields = new List<ICodeField>();
            m_interfaces = new List<string>();
            m_methods = new List<ICodeMethod>();
            m_scriptType = CodeScriptType.CSharp;
        }

        #endregion

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

        public void AddProperty(string propretyName, string propertyTypeName, int arrayCount = 0, string desc = "")
        {
            int tabCount = TabCount;
            CodePropertyInfo propertyInfo = new CodePropertyInfo(propretyName, propertyTypeName, arrayCount, desc, tabCount + 1);
            CodeFields.ListAdd(propertyInfo);
        }

        public void AddField(string fieldName, string fieldTypeName, int arrayCount = 0, string desc = "")
        {
            int tabCount = TabCount;
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
            string scriptName = GenerateScriptName();
            string fields = GenerateCodeFields();
            string construct = GenerateCodeConstruct();
            string methods = GenerateMethodCode();
            string tabStr = GetTabString();
            StringBuilder sb = new StringBuilder();
            sb.Append(scriptName);
            sb.Append(tabStr);
            sb.Append("{\n");
            sb.Append(fields);
            sb.Append(construct);
            sb.Append(methods);
            sb.Append(tabStr);
            sb.Append("}\n");

            return sb.ToString();
        }

        public string GenerateCodeConstruct()
        {
            StringBuilder sb = new StringBuilder();

            string tabStr = GetTabString();
            sb.AppendLine($"{tabStr}\tpublic {Name}()");
            sb.Append(tabStr);
            sb.AppendLine("\t{");
            string field = GenerateCodeFieldsConstruction();
            if (!string.IsNullOrEmpty(field))
            {
                sb.Append(field);
            }
            sb.Append(tabStr);
            sb.AppendLine("\t}\n");
            return sb.ToString();
        }

        public string GetTabString()
        {
            int tabCount = TabCount;
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
                if (!string.IsNullOrEmpty(fieldCode))
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
    }
}

