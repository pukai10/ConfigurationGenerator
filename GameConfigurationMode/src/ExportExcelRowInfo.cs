using System;
namespace GameConfigurationMode
{

    public class ExportExcelRowInfo
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
        /// 描述
        /// </summary>
        public string Description { get; set; }

        public override string ToString()
        {
            return $"Index:{Index},Name:{Name},Type:{Type},TitleName:{TitleName},Description:{Description}";
        }
    }
}

