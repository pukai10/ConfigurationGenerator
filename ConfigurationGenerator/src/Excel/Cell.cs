using System;
using NPOI.SS.UserModel;

namespace ConfigurationGenerator
{
	/// <summary>
	/// 单元格
	/// </summary>
	public class Cell
	{
		private int m_rowIndex;
		private int m_columnIndex;
		private ECellType m_cellType;
		private string m_cellValue;

		/// <summary>
		/// 是否是有效单元格
		/// </summary>
		public bool IsVaild => m_cellType != ECellType.Null;

		public int RowIndex => m_rowIndex;
		public int ColumnIndex => m_columnIndex;

		public string CellValue => m_cellValue;
		public ECellType CelType => m_cellType;

		public Cell(ICell cell,int rowIndex, int columnIndex)
		{
			m_rowIndex = rowIndex;
			m_columnIndex = columnIndex;
			m_cellType = cell.GetCellType(cell != null ? cell.CellType : CellType.Unknown);
			m_cellValue = cell.GetCellValue(cell != null ? cell.CellType : CellType.Unknown);

			//有效的单元格，如果表里配置的是空字符串时，对于Number和Boolean类型需要转成默认值
			if(IsVaild && string.IsNullOrEmpty(m_cellValue)) 
			{
				if(m_cellType == ECellType.Number)
				{
					m_cellValue = "0";
				}
				else if(m_cellType == ECellType.Boolean)
				{
                    m_cellValue = "false";
				}
			}
        }
	}

	/// <summary>
	/// 单元格类型，这里定义简单类型即可
	/// </summary>
	public enum ECellType
	{
		Number,
		String,
		Boolean,
		Null,
    }
}

