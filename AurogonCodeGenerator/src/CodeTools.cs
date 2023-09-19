using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AurogonCodeGenerator
{
    public static class CodeTools
    {
        private static string m_types = @"UInt32:uint32,uint
Int32:int32,int
Boolean:boolean,bool
UInt16:uint16,ushort
Int16:int16,short
UInt64:uint64,ulong
Int64:int64,long
Single:single,float
Double:double
Decimal:decimal
String:string
Byte:uint8,byte
SByte:int8,sbyte";


        private static Dictionary<string, string> m_typeDict = new Dictionary<string, string>();

        private static bool m_isInitTypeDict = false;
        private static void InitTypeDict()
        {
            if(m_isInitTypeDict)
            {
                return;
            }

            m_typeDict = new Dictionary<string, string>();
            string[] lines = m_types.Split('\n');
            foreach (var line in lines)
            {
                string content = line.Trim();
                string[] keyValue = content.Split(':');
                if(keyValue.Length != 2)
                {
                    continue;
                }
                string key = keyValue[0].Trim();
                string[] types = keyValue[1].Split(',');

                for (int i = 0; i < types.Length; i++)
                {
                    string type = types[i].Trim();
                    if(!string.IsNullOrEmpty(type) && !m_typeDict.ContainsKey(type))
                    {
                        m_typeDict.Add(type, key);
                    }
                }
            }

            m_isInitTypeDict = true;
        }

        public static CodeFieldType ConvertCodeFieldType(string type)
        {
            InitTypeDict();

            string lower = type.ToLower();
            if(!m_typeDict.ContainsKey(lower))
            {
                return CodeFieldType.Object;
            }

            if(Enum.TryParse(m_typeDict[lower],out CodeFieldType result) == false)
            {
                throw new Exception("type convert to failed,type:" + type);
            }

            return result;
        }

        public static void ListAdd<T>(this List<T> list,T t)
        {
            if(list == null)
            {
                list = new List<T>();
            }

            if(list.Contains(t) == false)
            {
                list.Add(t);
            }
        }
    }
}
