using System;

namespace AurogonXmlConvert
{
    /// <summary>
    /// xml属性名属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class XmlAttributionNameAttribute : Attribute
    {
        /// <summary>
        /// 属性名
        /// </summary>
        public string name;

        /// <summary>
        /// construction
        /// </summary>
        /// <param name="_name"></param>
        public XmlAttributionNameAttribute(string _name)
        {
            name = _name;
        }
    }

    /// <summary>
    /// xml 节点名属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class XmlNodeNameAttribute : Attribute
    {
        /// <summary>
        /// 属性名
        /// </summary>
        public string name;
        /// <summary>
        /// 字段类型
        /// </summary>
        public Type type;
        /// <summary>
        /// construction
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_type"></param>
        public XmlNodeNameAttribute(string _name, Type _type)
        {
            name = _name;
            type = _type;
        }
    }

    /// <summary>
    /// xml子节点列表属性(注意只用在IList)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class XmlChildNodeListAttribute : Attribute
    {
        /// <summary>
        /// 属性名
        /// </summary>
        public string name;
        /// <summary>
        /// 字段类型
        /// </summary>
        public Type type;
        /// <summary>
        /// construction
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_type"></param>
        public XmlChildNodeListAttribute(string _name, Type _type)
        {
            name = _name;
            type = _type;
        }
    }
}
