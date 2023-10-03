
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Text;

namespace AurogonCodeGenerator
{
    public class CSharpCodeGenerator : ICSharpCodeGenrator
    {
        private List<string> m_useNameSpaces;
        public List<string> UseNameSpaces => m_useNameSpaces;

        private List<ICSharpClassGenrator> m_classes;
        public List<ICSharpClassGenrator> Classes => m_classes;

        private string m_namespace;
        public string NameSpace => m_namespace;

        private CodeScriptType m_scriptType = CodeScriptType.CSharp;
        public CodeScriptType ScriptType => m_scriptType;

        private string m_name;
        public string Name => m_name;

        private string m_desc;
        public string Desc => m_desc;

        public int TabCount => GetTabCount();

        public CSharpCodeGenerator(string name) : this(name, string.Empty) { }

        public CSharpCodeGenerator(string name,string desc)
        {
            m_name = name;
            m_desc = desc;
            m_scriptType = CodeScriptType.CSharp;
            m_useNameSpaces = new List<string>();
            m_classes = new List<ICSharpClassGenrator>();
            m_namespace = string.Empty;
        }

        public void AddClass(ICSharpClassGenrator csharpClass)
        {
            if(m_classes == null)
            {
                m_classes = new List<ICSharpClassGenrator>();
            }

            m_classes.Add(csharpClass);
        }

        public void AddUseNameSpace(string nameSpace)
        {
            if(m_useNameSpaces == null)
            {
                m_useNameSpaces = new List<string>();
            }

            if(m_useNameSpaces.Contains(nameSpace) == false)
            {
                m_useNameSpaces.Add(nameSpace);
            }
        }

        public void AddUseNameSpace(string[] nameSpaces)
        {
            foreach (var nameSpace in nameSpaces)
            {
                AddUseNameSpace(nameSpace);
            }
        }

        public string GenerateCode()
        {
            string nameSpaceCode = GenerateNameSpaces();

            StringBuilder sb = new StringBuilder();
            sb.Append(Desc);
            sb.Append(nameSpaceCode);

            if (!string.IsNullOrEmpty(NameSpace))
            {
                sb.Append($"namespace {NameSpace}\n");
                sb.Append("{\n");
            }

            foreach (var classItem in Classes)
            {
                sb.AppendLine(classItem.GenerateCode());
            }


            if (!string.IsNullOrEmpty(NameSpace))
            {
                sb.Append("}");
            }
            return sb.ToString();
        }

        public string GenerateCodeConstruct()
        {
            throw new System.NotImplementedException();
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

        public int GetTabCount()
        {
            return string.IsNullOrEmpty(NameSpace) ? 0 : 1;
        }

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

        public void SetNameSpace(string nameSpace)
        {
            m_namespace = nameSpace;
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
    }
}
