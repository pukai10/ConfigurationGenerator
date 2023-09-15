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

            //var optProperties = from p in type.GetProperties()
            //                    let attrs = p.GetCustomAttributes()
            //                    where attrs.OfType<OptionAttribute>().Any()
            //                    select p;

           var optProperties = type.GetProperties().Where((p) => p.GetCustomAttributes().OfType<OptionAttribute>().Any()).
                                                    GroupBy((optProperty)=>new OptionProperty(optProperty,optProperty.GetCustomAttribute<OptionAttribute>()));

            for (int argIndex = 0; argIndex < args.Length; argIndex++)
            {
                var arg = args[argIndex];
                if(string.IsNullOrEmpty(arg) || !arg.StartsWith("-",StringComparison.Ordinal))
                {
                    continue;
                }

                Token token = !arg.StartsWith("--", StringComparison.Ordinal) ? ParseShortName(arg, args) : ParseLongName(arg, args);



            }
        }

        private Token ParseShortName(string arg,string[] args)
        {
            if(arg.Length == 1)
            {
                return null;
            }

            string opts = arg.Substring(1);
            for (int i = 0; i < opts.Length; i++)
            {
                Console.WriteLine(opts[i]);
            }

            return Token.Value("");
        }

        private Token ParseLongName(string arg, string[] args)
        {
            if(args.Length == 2)
            {
                return null;
            }

            string str = arg.Substring(2);

            Console.WriteLine(str);

            return Token.Value("");
        }
    }

}
