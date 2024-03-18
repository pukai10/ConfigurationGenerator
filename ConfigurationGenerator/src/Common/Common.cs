using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationGenerator
{
    public class Common
    {
        public static string Version = "1.0.0.1";


        /// <summary>
        /// 是否是基本数据类型
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static bool IsBaseType(string typeName)
        {
            switch(typeName)
            {
                case "uint8":
                case "int8":
                case "uint16":
                case "int16":
                case "uint32":
                case "int32":
                case "uint64":
                case "int64":
                case "float":
                case "double":
                case "boolean":
                case "string":
                    return true;
                default:
                    return false;
            }
        }


    }
}
