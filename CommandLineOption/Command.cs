using System;
namespace CommandLineOption
{
    /// <summary>
    /// 命令行命令特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CommandAttribute : Attribute
    {
        private string name;
        /// <summary>
        /// 名字
        /// </summary>
        public string Name => name;

        private string describte;
        /// <summary>
        /// 描述
        /// </summary>
        public string Describte => describte;

        public CommandAttribute(string name,string describte = "")
        {
            this.name = name;
            this.describte = describte;
        }


        public override string ToString()
        {
            return $"Name:{Name},Descrite:{Describte}";
        }
    }
}
