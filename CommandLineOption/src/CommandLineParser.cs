﻿using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using AurogonTools;
using System.Text;

namespace CommandLineOption
{
    /// <summary>
    /// 命令行解析类
    /// </summary>
    public class CommandLineParser
    {
        private static readonly Lazy<CommandLineParser> m_default = new Lazy<CommandLineParser>(() =>
        {
            return InitParser();
        });

        private static CommandLineParser InitParser()
        {
            var parser = new CommandLineParser();

            Type type = typeof(CommandLineParser);

            var optProperties = type.GetProperties().Where((p) => p.GetCustomAttributes().OfType<OptionAttribute>().Any()).Select((opt) => new OptionProperty(opt, opt.GetCustomAttribute<OptionAttribute>()));

            m_defaultOptionProperties = new List<OptionProperty>();

            foreach (var property in optProperties)
            {
                if (m_defaultOptionProperties.Contains(property) == false)
                {
                    m_defaultOptionProperties.Add(property);
                }
            }

            return parser;
        }

        public static CommandLineParser Default => m_default.Value;

        private static List<OptionProperty> m_allOptionProperties = new List<OptionProperty>();
        private static List<OptionProperty> m_defaultOptionProperties = new List<OptionProperty>();

        private ILogger m_logger;

        [Option("h","help",helpText = "所有命令参数帮助文档")]
        public bool IsHelpText { get; set; }

        public static void AddOptionProperties(IEnumerable<OptionProperty> optionProperties)
        {
            foreach (var property in optionProperties)
            {
                if(m_allOptionProperties.Contains(property) == false)
                {
                    m_allOptionProperties.Add(property);
                }
            }
        }

        public static string GetOptionHelpText(string option)
        {
            foreach (var optionProperty in m_allOptionProperties)
            {
                if(optionProperty.shortName.Equals(option) || optionProperty.longName.Equals(option))
                {
                    return optionProperty.GetHelpText();
                }
            }

            return $"没有找到您输入的: {option} 命令";
        }

        public static string GetAllOptionHelpText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("下面列出列所有命令");
            foreach (var optionProperty in m_allOptionProperties)
            {
                sb.AppendLine(optionProperty.GetHelpText());
            }
            return sb.ToString();
        }

        public CommandLineParser()
        {
            m_logger = Logger.GetLogger("CommandLineParser", new LoggerSetting() { logType = LogType.Error });
        }

        public T Parse<T>(string[] args,bool throwException = false)
        {
            if(args == null)
            {
                throw new ArgumentNullException("args");
            }

            Type type = typeof(T);

            T obj = Activator.CreateInstance<T>();
            List<string> errors = new List<string>();
            var optProperties = type.GetProperties().Where((p) => p.GetCustomAttributes().OfType<OptionAttribute>().Any()).Select((opt) => new OptionProperty(opt,opt.GetCustomAttribute<OptionAttribute>()));
            AddOptionProperties(optProperties);
            for (int argIndex = 0; argIndex < args.Length; argIndex++)
            {
                var arg = args[argIndex];
                if(string.IsNullOrEmpty(arg) || !arg.StartsWith("-",StringComparison.Ordinal))
                {
                    continue;
                }

                if(!arg.StartsWith("--", StringComparison.Ordinal))
                {
                    ParseShortName<T>(obj,arg, argIndex, args, optProperties,errors);
                }
                else
                {
                    ParseLongName<T>(obj,arg,argIndex, args, optProperties,errors);
                }

            }

            if(errors!= null && errors.Count > 0)
            {
                foreach (var errorStr in errors)
                {
                    m_logger.Error(errorStr);
                }

                if(throwException)
                {
                    throw new Exception(string.Join("\n", errors.ToArray()));
                }
            }
            
            return obj;
        }

        private void ParseShortName<T>(T obj,string arg,int index,string[] args,IEnumerable<OptionProperty> optionProperties,List<string> errors)
        {
            if(arg.Length == 1)
            {
                errors.Add("命令行格式错误，命令操作不能只有‘-’");
                return;
            }

            string opts = arg.Substring(1);

            var optProperty = optionProperties.Where(p => p.IsMatch(opts)).SingleOrDefault();
            bool isDefaltProperty = false;
            if(optProperty == null)
            {
                optProperty = m_defaultOptionProperties.Where(p => p.IsMatch(opts)).SingleOrDefault();
                isDefaltProperty = optProperty != null;
            }
            if(optProperty != null)
            {
                if (optProperty.required)
                {
                    if (args.Length > (index + 1) && args[index + 1].StartsWith("-",StringComparison.Ordinal) == false)
                    {
                        if(isDefaltProperty)
                        {
                            optProperty.SetPropertyValue<CommandLineParser>(m_default.Value, args[index + 1]);
                        }
                        else
                        {
                            optProperty.SetPropertyValue<T>(obj, args[index + 1]);
                        }
                    }
                    else
                    {
                        errors.Add($"命令行参数错误，命令:{opts}是必须要有参数的");
                    }
                }
                else
                {
                    if (isDefaltProperty)
                    {
                        optProperty.SetPropertyValue<CommandLineParser>(m_default.Value, "true");
                    }
                    else
                    {
                        optProperty.SetPropertyValue<T>(obj, "true");
                    }
                }
                return;
            }


            int argIndex = 0;
            for (int i = 0; i < opts.Length; i++)
            {
                isDefaltProperty = false;
                var opt = optionProperties.Where(p => p.IsMatch(opts[i])).SingleOrDefault();

                if(opt == null)
                {
                    opt = m_defaultOptionProperties.Where(p => p.IsMatch(opts[i])).SingleOrDefault();
                    isDefaltProperty = opt != null;
                }

                if(opt != null)
                {
                    if (opt.required)
                    {
                        argIndex++;
                        if (args.Length > (index + argIndex) && args[index + argIndex].StartsWith("-", StringComparison.Ordinal) == false)
                        {
                            if(isDefaltProperty)
                            {
                                opt.SetPropertyValue<CommandLineParser>(m_default.Value, args[index + argIndex]);
                            }
                            else
                            {
                                opt.SetPropertyValue<T>(obj, args[index + argIndex]);
                            }
                        }
                        else
                        {
                            errors.Add($"命令行参数错误，命令:{opt}是必须要有参数的");
                        }
                    }
                    else
                    {
                        if(isDefaltProperty)
                        {
                            opt.SetPropertyValue<CommandLineParser>(m_default.Value, "true");
                        }
                        else
                        {
                            opt.SetPropertyValue<T>(obj, "true");
                        }
                    }
                }
            }

        }

        private void ParseLongName<T>(T obj,string arg,int index, string[] args, IEnumerable<OptionProperty> optionProperties, List<string> errors)
        {
            if(arg.Length == 2)
            {
                errors.Add("命令行格式错误，命令操作不能只有‘--’");
                return;
            }

            string str = arg.Substring(2);

            if(str.Contains("="))
            {
                string[] strs = str.Split('=');
                if(strs == null || strs.Length != 2)
                {
                    errors.Add("命令行参数格式错误，命令行参数使用=时，出现多个=，或者=后没有参数");
                    return;
                }

                string opt = strs[0];
                string value = strs[1];

                var optProperty = optionProperties.Where(p => p.IsMatch(opt)).SingleOrDefault();
                if (optProperty != null)
                {
                    if (optProperty.required)
                    {
                        optProperty.SetPropertyValue<T>(obj, value);
                    }
                    else
                    {
                        optProperty.SetPropertyValue<T>(obj, "true");
                    }
                }
                else
                {
                    //不包含的命令操作，可能是其他命令的操作，这里不做处理也不报错
                }
            }
            else
            {
                var optProperty = optionProperties.Where(p => p.IsMatch(str)).SingleOrDefault();
                if(optProperty != null)
                {
                    if (optProperty.required)
                    {
                        if (args.Length > (index + 1) && args[index + 1].StartsWith("-", StringComparison.Ordinal) == false)
                        {
                            optProperty.SetPropertyValue<T>(obj, args[index + 1]);
                        }
                        else
                        {
                            errors.Add($"命令行参数错误，命令:{str}是必须要有参数的");
                        }
                    }
                    else
                    {
                        optProperty.SetPropertyValue<T>(obj, "true");
                    }
                }
                else
                {
                    //不包含的命令操作，可能是其他命令的操作，这里不做处理也不报错
                }
            }
        }
    }

}
