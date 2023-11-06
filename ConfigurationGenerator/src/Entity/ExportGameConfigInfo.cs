

using System.Collections.Generic;
using System.Text;

namespace ConfigurationGenerator
{

    public class ExportGameConfigRowInfo
    {
        /// <summary>
        /// 位于表的列索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 字段名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 表中列头的名字
        /// </summary>
        public string TitleName { get; set; }

        /// <summary>
        /// 字符串size
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 数组数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        public override string ToString()
        {
            return $"Index:{Index},Name:{Name},Type:{Type},TitleName:{TitleName},Description:{Description}";
        }
    }

    public class ExportGameConfigInfo
    {
        public string ExcelName { get; set; }
        public string StructName { get; set; }
        public string ExportBytesPath { get; set; }
        public string MetaFilePath { get; set; }
        public string SheetName { get; set; }

        public List<ExportGameConfigRowInfo> exportRowInfoList { get; set; }

        public override string ToString()
        {
            return $"ExcelName:{ExcelName},StructName:{StructName},ExportBytesPath:{ExportBytesPath},MetaFilePath:{MetaFilePath}";
        }
    }

    public class ExportGameConfig
    {
        public string ExcelFile { get; set; }
        public List<ExportGameConfigInfo> ExportList { get; set; }

        public bool AddExportInfo(ExportGameConfigInfo info)
        {
            if(info == null)
            {
                return false;
            }
            bool hasInfo = HasInfo(info.SheetName);

            if (!hasInfo)
            {
                ExportList.Add(info);
                return true;
            }

            return false;
        }


        public bool HasInfo(string sheetName)
        {
            if(ExportList != null)
            {
                for (int i = 0; i < ExportList.Count; i++)
                {
                    var info = ExportList[i];   
                    if(info != null && info.SheetName == sheetName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ExcelFile);
            foreach (var node in ExportList)
            {
                sb.AppendLine(node.ToString());
            }

            return sb.ToString();
        }
    }
}
