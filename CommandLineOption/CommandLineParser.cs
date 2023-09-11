﻿using System;
using System.Reflection;
using System.Collections.Generic;

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

            ParserResult parserResult = new ParserResult();
            parserResult.command = type.GetCustomAttribute<Command>();
            parserResult.options = new List<Option>();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var option = property.GetCustomAttribute<Option>();
                if(option != null)
                {
                    Console.WriteLine(option.ToString());
                    option.propertyInfo = property;
                    parserResult.options.Add(option);
                }
            }

            if(parserResult.command != null)
            {

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

                if (arg.Substring(0,2) == "--")
                {

                }
                else
                {

                }
            }
        }
    }

    public class ParserResult
    {
        public Command command;
        public List<Option> options;

    }
}
