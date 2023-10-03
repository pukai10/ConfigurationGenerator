using System;
using AurogonTools;
using CommandLineOption;

namespace ConfigCodeGenerator
{
	public class Program
	{

        static void Main(string[] args)
        {
            args = new string[]
            {
                "-v"
            };

            string version = IOHelper.ReadFileText("./version.txt", true);

            AurogonVersion.SetVersion(version);

            ConfigCodeGenerator codeGenerator = CommandLineParser.Default.Parse<ConfigCodeGenerator>(args);

            codeGenerator.Parse();
            if (CommandLineParser.Default.IsHelpText)
            {
                Console.WriteLine(CommandLineParser.GetAllOptionHelpText());
            }

            if(codeGenerator.IsShowVersion)
            {
                Console.WriteLine(AurogonVersion.Default.ToString());
            }

            AurogonVersion.Default.AddBuild();

            IOHelper.SaveFile("./version.txt", AurogonVersion.Default.Version);

            Console.ReadKey();
        }

    }
}

