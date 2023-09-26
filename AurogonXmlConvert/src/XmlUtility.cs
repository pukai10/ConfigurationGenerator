using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace AurogonXmlConvert
{
    /// <summary>
    /// xml utility 
    /// </summary>
    public static class XmlUtility
    {
        /// <summary>
        /// xml 转结构
        /// </summary>
        /// <typeparam name="T">实体结构</typeparam>
        /// <param name="xmlFilePath">xml 路径</param>
        /// <returns></returns>
        public static T FromXml<T>(string xmlFilePath) where T : class
        {
            if (!File.Exists(xmlFilePath))
            {
                throw new Exception("file is not exist,filePath:" + xmlFilePath);
            }

            XmlDocument document = new XmlDocument();
            document.Load(xmlFilePath);
            return FromXml<T>(document);
        }

        /// <summary>
        /// xml 转结构
        /// </summary>
        /// <typeparam name="T">实体结构</typeparam>
        /// <param name="document">加载xml的XmlDocument</param>
        /// <returns></returns>
        public static T FromXml<T>(XmlDocument document) where T : class
        {
            if (document == null)
            {
                throw new Exception("XmlDocument is null");
            }

            Type type = typeof(T);
            object obj = Activator.CreateInstance(type);

            XmlNodeNameAttribute xmlNodeNameAttri = type.GetCustomAttribute<XmlNodeNameAttribute>(true);
            if (xmlNodeNameAttri == null)
            {
                throw new Exception(string.Format("{0} not has XmlNodeNameAttribute", type.Name));
            }

            XmlNode xmlRootNode = document.SelectSingleNode(xmlNodeNameAttri.name);

            if (xmlRootNode == null)
            {
                throw new Exception("xml has not root:" + xmlNodeNameAttri.name);
            }

            Deserialize(type, obj, xmlRootNode);
            return obj as T;
        }

        /// <summary>
        /// 结构转xml
        /// </summary>
        /// <typeparam name="T">实体结构</typeparam>
        /// <param name="t">结构体</param>
        /// <param name="saveXmlPath">保存的xml路径</param>
        /// <returns>是否成功</returns>
        public static bool ToXml<T>(T t, string saveXmlPath) where T : class
        {
            if (File.Exists(saveXmlPath))
            {
                File.Delete(saveXmlPath);
            }

            XmlDocument document = ToXml<T>(t);
            if (document != null)
            {
                document.Save(saveXmlPath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 结构转xml
        /// </summary>
        /// <typeparam name="T">实体结构</typeparam>
        /// <param name="t">结构体</param>
        /// <returns>XmlDocument</returns>
        public static XmlDocument ToXml<T>(T t) where T : class
        {
            Type type = typeof(T);
            object obj = t as object;
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDec = doc.CreateXmlDeclaration("1.0", "utf-8", "yes");
            doc.AppendChild(xmlDec);
            XmlNodeNameAttribute rootAttr = type.GetCustomAttribute<XmlNodeNameAttribute>(true);
            if (rootAttr == null)
            {
                throw new Exception($"{type} not has {nameof(XmlNodeNameAttribute)}");
            }

            XmlElement rootElement = doc.CreateElement(rootAttr.name);
            Serialize(type, obj, rootElement, doc);
            doc.AppendChild(rootElement);
            return doc;
        }

        /// <summary>
        /// 扩展PropertyInfo
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetValueByType(this PropertyInfo propertyInfo, object obj, string value)
        {
            string errorType = string.Empty;
            if (IsType(propertyInfo.PropertyType, typeof(byte).ToString()))
            {
                if (byte.TryParse(value, out byte byteValue))
                {
                    propertyInfo.SetValue(obj, byteValue);
                }
                else
                {
                    errorType = typeof(byte).ToString();
                }
            }
            else if (IsType(propertyInfo.PropertyType, typeof(sbyte).ToString()))
            {
                if (sbyte.TryParse(value, out sbyte sbyteValue))
                {
                    propertyInfo.SetValue(obj, sbyteValue);
                }
                else
                {
                    errorType = typeof(sbyte).ToString();
                }
            }
            else if (IsType(propertyInfo.PropertyType, typeof(short).ToString()))
            {
                if (short.TryParse(value, out short shortValue))
                {
                    propertyInfo.SetValue(obj, shortValue);
                }
                else
                {
                    errorType = typeof(short).ToString();
                }
            }
            else if (IsType(propertyInfo.PropertyType, typeof(ushort).ToString()))
            {
                if (ushort.TryParse(value, out ushort ushortValue))
                {
                    propertyInfo.SetValue(obj, ushortValue);
                }
                else
                {
                    errorType = typeof(ushort).ToString();
                }
            }
            else if (IsType(propertyInfo.PropertyType, typeof(int).ToString()))
            {
                if (int.TryParse(value, out int intValue))
                {
                    propertyInfo.SetValue(obj, intValue);
                }
                else
                {
                    errorType = typeof(int).ToString();
                }
            }
            else if (IsType(propertyInfo.PropertyType, typeof(uint).ToString()))
            {
                if (uint.TryParse(value, out uint uintValue))
                {
                    propertyInfo.SetValue(obj, uintValue);
                }
                else
                {
                    errorType = typeof(uint).ToString();
                }
            }
            else if (IsType(propertyInfo.PropertyType, typeof(long).ToString()))
            {
                if (long.TryParse(value, out long longValue))
                {
                    propertyInfo.SetValue(obj, longValue);
                }
                else
                {
                    errorType = typeof(long).ToString();
                }
            }
            else if (IsType(propertyInfo.PropertyType, typeof(ulong).ToString()))
            {
                if (ulong.TryParse(value, out ulong ulongValue))
                {
                    propertyInfo.SetValue(obj, ulongValue);
                }
                else
                {
                    errorType = typeof(ulong).ToString();
                }
            }
            else if (IsType(propertyInfo.PropertyType, typeof(float).ToString()))
            {
                if (float.TryParse(value, out float floatValue))
                {
                    propertyInfo.SetValue(obj, floatValue);
                }
                else
                {
                    errorType = typeof(float).ToString();
                }
            }
            else if (IsType(propertyInfo.PropertyType, typeof(double).ToString()))
            {
                if (double.TryParse(value, out double doubleValue))
                {
                    propertyInfo.SetValue(obj, doubleValue);
                }
                else
                {
                    errorType = typeof(double).ToString();
                }
            }
            else if (IsType(propertyInfo.PropertyType, typeof(bool).ToString()))
            {
                if (bool.TryParse(value, out bool boolValue))
                {
                    propertyInfo.SetValue(obj, boolValue);
                }
                else
                {
                    errorType = typeof(bool).ToString();
                }
            }
            else if (IsType(propertyInfo.PropertyType, typeof(string).ToString()))
            {
                propertyInfo.SetValue(obj, value);
            }
            else
            {
                throw new Exception("PropertyInfo SetValueByType ,type is not exist:" + propertyInfo.PropertyType.Name);
            }

            if (!string.IsNullOrEmpty(errorType))
            {
                throw new Exception(string.Format("PropertyInfo SetValueByType TryParse type is error,type:{0},value:{1}", propertyInfo.PropertyType.Name, value));
            }
        }

        /// <summary>
        /// 判断类型是否一样
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static bool IsType(Type type, string typeName)
        {
            if (type.ToString() == typeName)
            {
                return true;
            }

            if (type.ToString() == "System.Object")
            {
                return false;
            }

            return IsType(type.BaseType, typeName);
        }

        #region Private Method

        private static void Deserialize(Type type, object obj, XmlNode curRootNode)
        {
            XmlNodeList nodeList = curRootNode.ChildNodes;
            XmlAttributeCollection xmlAttributeCollection = curRootNode.Attributes;

            foreach (PropertyInfo property in type.GetProperties())
            {
                XmlAttributionNameAttribute xmlAttriNameAttri = property.GetCustomAttribute<XmlAttributionNameAttribute>(true);
                if (xmlAttriNameAttri != null)
                {
                    var xmlAttr = xmlAttributeCollection[xmlAttriNameAttri.name];
                    if (xmlAttr != null)
                    {
                        property.SetValueByType(obj, xmlAttr.Value);
                    }
                }

                XmlNodeNameAttribute xmlNodeNameAttr = property.GetCustomAttribute<XmlNodeNameAttribute>(true);
                if (xmlNodeNameAttr != null)
                {
                    Type nodeNameType = xmlNodeNameAttr.type;
                    object nodeObj = Activator.CreateInstance(nodeNameType);
                    foreach (XmlNode xmlNode in nodeList)
                    {
                        if (xmlNode.Name == xmlNodeNameAttr.name)
                        {
                            Deserialize(nodeNameType, nodeObj, xmlNode);
                        }
                    }
                    property.SetValue(obj, nodeObj);
                }

                XmlChildNodeListAttribute xmlChildNodeList = property.GetCustomAttribute<XmlChildNodeListAttribute>(true);
                if (xmlChildNodeList != null && property.PropertyType.IsGenericType == true)
                {
                    IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(xmlChildNodeList.type));

                    foreach (XmlNode xmlNode in nodeList)
                    {
                        if (xmlNode.Name == xmlChildNodeList.name)
                        {
                            object nodeObj = Activator.CreateInstance(xmlChildNodeList.type);
                            Deserialize(xmlChildNodeList.type, nodeObj, xmlNode);
                            list.Add(nodeObj);
                        }
                    }
                    property.SetValue(obj, list);
                }
            }
        }

        private static void Serialize(Type type, object obj, XmlElement rootElement, XmlDocument doc)
        {
            if (type == null)
            {
                throw new Exception("Serialize xml type is null");
            }

            if (obj == null)
            {
                throw new Exception("Serialize xml object is null");
            }

            if (rootElement == null)
            {
                throw new Exception("Serialize xml rootElement is null");
            }

            if (doc == null)
            {
                throw new Exception("Serialize xml XmlDocument is null");
            }

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                XmlAttributionNameAttribute xmlAttriNameAttri = propertyInfo.GetCustomAttribute<XmlAttributionNameAttribute>(true);
                if (xmlAttriNameAttri != null)
                {
                    object childObj = propertyInfo.GetValue(obj, null);
                    rootElement.SetAttribute(xmlAttriNameAttri.name, childObj == null ? "" : childObj.ToString());
                }

                XmlNodeNameAttribute xmlNodeNameAttr = propertyInfo.GetCustomAttribute<XmlNodeNameAttribute>(true);
                if (xmlNodeNameAttr != null)
                {
                    object childObj = propertyInfo.GetValue(obj, null);
                    XmlElement childNode = doc.CreateElement(xmlNodeNameAttr.name);
                    Serialize(propertyInfo.PropertyType, childObj, childNode, doc);
                    rootElement.AppendChild(childNode);
                }

                XmlChildNodeListAttribute xmlChildNodeList = propertyInfo.GetCustomAttribute<XmlChildNodeListAttribute>(true);
                if (xmlChildNodeList != null && propertyInfo.PropertyType.IsGenericType == true)
                {
                    object childObj = propertyInfo.GetValue(obj, null);
                    int count = Convert.ToInt32(childObj.GetType().GetProperty("Count").GetValue(childObj, null));
                    for (int i = 0; i < count; i++)
                    {
                        object item = childObj.GetType().GetProperty("Item").GetValue(childObj, new object[] { i });
                        XmlElement childNode = doc.CreateElement(xmlChildNodeList.name);
                        Serialize(item.GetType(), item, childNode, doc);
                        rootElement.AppendChild(childNode);
                    }
                }
            }
        }

        #endregion

    }
}
