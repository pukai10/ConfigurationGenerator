using System;
using System.Text;
using System.Collections.Generic;
using AurogonTools;

namespace AurogonCodeGenerator
{
    public class PackStatement : StatementBase
    {
        private string m_arrayTemplate;
        private List<ICodeField> m_args;
        public PackStatement(int tabCount,List<ICodeField> args):base(tabCount)
        {
            m_args = args;
            m_template = "#TAB#ret = writer.Write#ARGTYPE#(#ARGNAME##OTHERARGS#);\n#TAB#if(ret != WRErrorType.NO_ERROR) return ret;\n";
            m_arrayTemplate = "#TAB#for(int i = 0; i < #ARRAY_SIZE#; i++)\n#TAB#{\n#TAB#\tret = writer.Write#ARGTYPE#(#ARGNAME#[i]#OTHERARGS#);\n#TAB#\tif(ret != WRErrorType.NO_ERROR) return ret;\n#TAB#}\n";
        }

        private string GetArgsStatement()
        {
            StringBuilder sb = new StringBuilder();
            string tabStr = GetTabString();
            if (m_args != null)
            {
                for(int i = 0; i < m_args.Count; i++)
                {
                    var arg = m_args[i];
                    string content = string.Empty;
                    string otherArgs = string.Empty;
                    if(arg.FieldType == CodeFieldType.Object)
                    {
                        otherArgs = $", ref writer";
                    }

                    if(arg.IsArray)
                    {
                        content = m_arrayTemplate.Replace("#TAB#", tabStr).Replace("#ARRAY_SIZE#", arg.Size.ToString()).Replace("#ARGNAME#", arg.Name).Replace("#OTHERARGS#", otherArgs).Replace("#ARGTYPE#", GetArgType(arg.FieldType));
                    }
                    else
                    {
                        content = m_template.Replace("#TAB#", tabStr).Replace("#ARGNAME#", arg.Name).Replace("#OTHERARGS#", otherArgs).Replace("#ARGTYPE#", GetArgType(arg.FieldType));
                    }
                    sb.AppendLine(content);
                }
            }

            return sb.ToString();
        }

        private string GetArgType(CodeFieldType fieldType)
        {
            switch (fieldType)
            {
                case CodeFieldType.Object:
                    return string.Empty;
                case CodeFieldType.Single:
                    return "Float";
                case CodeFieldType.String:
                    return "UInt32";
                default:
                    return fieldType.ToString();
            }
        }

        public override string GenerateCode()
        {
            StringBuilder sb = new StringBuilder();
            string tabStr = GetTabString();
            sb.Append($"{tabStr}if(writer == null)\n");
            sb.Append(tabStr);
            sb.Append("{\n");
            sb.Append($"{tabStr}\treturn WRErrorType.PACKOBJECT_WRITER_IS_NULL;\n");
            sb.Append($"{tabStr}");
            sb.Append("}\n");


            sb.Append($"{tabStr}WRErrorType ret = WRErrorType.NO_ERROR;\n");

            sb.Append(GetArgsStatement());

            sb.Append($"{tabStr}return ret;");

            return sb.ToString();
        }
    }

    public class UnPackStatement : StatementBase
    {
        private string m_arrayTemplate;
        private List<ICodeField> m_args;
        public UnPackStatement(int tabCount, List<ICodeField> args) : base(tabCount)
        {
            m_args = args;
            m_template = "#TAB#ret = reader.Read#ARGTYPE#(ref #ARGNAME##OTHERARGS#);\n#TAB#if(ret != WRErrorType.NO_ERROR) return ret;\n";
            m_arrayTemplate = "#TAB#for(int i = 0; i < #ARRAY_SIZE#; i++)\n#TAB#{\n#TAB#\tret = reader.Read#ARGTYPE#(ref #ARGNAME#[i]#OTHERARGS#);\n#TAB#\tif(ret != WRErrorType.NO_ERROR) return ret;\n#TAB#}\n";
        }

        private string GetArgsStatement()
        {
            StringBuilder sb = new StringBuilder();
            string tabStr = GetTabString();
            if (m_args != null)
            {
                for (int i = 0; i < m_args.Count; i++)
                {
                    var arg = m_args[i];
                    string content = string.Empty;
                    string otherArgs = string.Empty;
                    if (arg.FieldType == CodeFieldType.Object)
                    {
                        otherArgs = $", ref reader";
                    }

                    if (arg.IsArray)
                    {
                        content = m_arrayTemplate.Replace("#TAB#", tabStr).Replace("#ARRAY_SIZE#", arg.Size.ToString()).Replace("#ARGNAME#", arg.Name).Replace("#OTHERARGS#", otherArgs).Replace("#ARGTYPE#", GetArgType(arg.FieldType));
                    }
                    else
                    {
                        content = m_template.Replace("#TAB#", tabStr).Replace("#ARGNAME#", arg.Name).Replace("#OTHERARGS#", otherArgs).Replace("#ARGTYPE#", GetArgType(arg.FieldType));
                    }
                    sb.AppendLine(content);
                }
            }

            return sb.ToString();
        }

        private string GetArgType(CodeFieldType fieldType)
        {
            switch (fieldType)
            {
                case CodeFieldType.Object:
                    return string.Empty;
                case CodeFieldType.Single:
                    return "Float";
                case CodeFieldType.String:
                    return "UInt32";
                default:
                    return fieldType.ToString();
            }
        }

        public override string GenerateCode()
        {
            StringBuilder sb = new StringBuilder();
            string tabStr = GetTabString();
            sb.Append($"{tabStr}if(reader == null)\n");
            sb.Append(tabStr);
            sb.Append("{\n");
            sb.Append($"{tabStr}\treturn WRErrorType.UNPACKOBJECT_READER_IS_NULL;\n");
            sb.Append($"{tabStr}");
            sb.Append("}\n");


            sb.Append($"{tabStr}WRErrorType ret = WRErrorType.NO_ERROR;\n");

            sb.Append(GetArgsStatement());

            sb.Append($"{tabStr}return ret;");

            return sb.ToString();
        }
    }
}

