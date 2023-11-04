using System;
using AurogonTools;
using CommandLineOption;

namespace ConfigCodeGenerator
{
	public class Program
	{

        static void Main(string[] args)
        {
            AurogonVersion.SetVersion(Common.Version);

            ConfigCodeGenerator codeGenerator = CommandLineParser.Default.Parse<ConfigCodeGenerator>(args);

            codeGenerator.Parse();
            //if (CommandLineParser.Default.IsHelpText)
            //{
            //    Console.WriteLine(CommandLineParser.GetAllOptionHelpText());
            //}

            Console.ReadKey();
        }

    }
}

