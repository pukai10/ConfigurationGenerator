using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace CommandLineOption
{
    /// <summary>
    /// 命令行解析类
    /// </summary>
    public class CommandLineParser
    {
        private static readonly Lazy<CommandLineParser> m_default = new Lazy<CommandLineParser>(()=> new CommandLineParser());
        public static CommandLineParser Default => m_default.Value;

        public CommandLineParser()
        {

        }

        public void Parse<T>(string[] args)
        {
            if(args == null)
            {
                throw new ArgumentNullException("args");
            }

            Type type = typeof(T);

            T t = Activator.CreateInstance<T>();

            var optProperties = from p in type.GetProperties()
                          let attrs = p.GetCustomAttributes()
                          where attrs.OfType<OptionAttribute>().Any()
                          select p;

            foreach (var property in optProperties)
            {
                Console.WriteLine(property.Name);
            }


            for (int argIndex = 0; argIndex < args.Length; argIndex++)
            {
                var arg = args[argIndex];
                if(string.IsNullOrEmpty(arg))
                {
                    continue;
                }

                if(arg.Substring(0,1) != "-")
                {
                    continue;
                }

                if (arg.Length < 2)
                {
                    throw new Exception($"arg length less 2,please check {argIndex}:{arg}");
                }

                Token token = arg.StartsWith("-", StringComparison.Ordinal) ? ((!arg.StartsWith("--", StringComparison.Ordinal) ? ParseShortName() : ParseLongName())) : Token.Value(arg);


            }
        }

        private Token ParseShortName()
        {
            return Token.Value("");
        }

        private Token ParseLongName()
        {
            return Token.Value("");
        }
    }

}
