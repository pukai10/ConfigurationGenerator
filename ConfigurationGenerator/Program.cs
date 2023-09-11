using System;
using CommandLineOption;

namespace ConfigurationGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            args = new string[3]
            {
                "-p",
                "test\test.cs",
                "--batchmode"
            };

            CommandLineParser.Default.Parse<Setting>(args);

            Console.ReadKey();
        }
    }

    [Command("export","export excels")]
    public class Setting
    {
        [Option("p","path",describte:"设置路径",required:true)]
        public string Path { get; set; }

        [Option("b","batchmode",describte:"batch mode start",required:false)]
        public bool IsBatchMode { get; set; }
    }
}
