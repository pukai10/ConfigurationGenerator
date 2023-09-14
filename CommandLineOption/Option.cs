using System;
using System.Reflection;

namespace CommandLineOption
{
    /// <summary>
    /// 命令参数特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property,AllowMultiple =false,Inherited = true)]
    public class OptionAttribute : Attribute
    {
        private string name;
        /// <summary>
        /// 名字
        /// </summary>
        public string Name => name;

        private string alias;
        /// <summary>
        /// 别名
        /// </summary>
        public string Alias => alias;

        private string describte;
        /// <summary>
        /// 描述
        /// </summary>
        public string Describte => describte;

        private bool required = false;
        /// <summary>
        /// 是否必须
        /// </summary>
        public bool Required => required;


        public OptionAttribute(string alias,string name,string describte = "",bool required = false)
        {
            this.alias = alias;
            this.name = name;
            this.describte = describte;
            this.required = required;
        }

        public override string ToString()
        {
            return $"-{alias} --{Name} {Describte} {Required}";
        }
    }

}
