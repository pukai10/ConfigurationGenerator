using System;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using NPOI.SS.Formula.Functions;

namespace ConfigurationGenerator
{
	public class Sheet
	{

		private bool m_isVaild = false;
		public bool IsVaild => m_isVaild;

		private string m_excelName;
		private List<Cell[]> m_cells = new List<Cell[]>();

		public Sheet(ISheet sheet, string excelName)
		{
			m_excelName = excelName;

			if (sheet == null)
			{
				m_isVaild = false;
				return;
			}

			m_sheetName = sheet.SheetName;

			int firstRowIndex = sheet.FirstRowNum;
			int lastRowIndex = sheet.LastRowNum;

			//如果不是从0行开始或者结束行是0，那认为是无效的Sheet
			if (firstRowIndex != 0 || lastRowIndex == 0)
			{
				m_isVaild = false;
				return;
			}

			m_cells = new List<Cell[]>();
			for (int rowIndex = firstRowIndex; rowIndex < lastRowIndex; rowIndex++)
			{
				IRow row = sheet.GetRow(rowIndex);
				//第一行都是空的那认为是无效的Sheet
				if (row == null && rowIndex == 0)
				{
					m_isVaild = false;
					return;
				}

				if (row != null)
				{
					int firstIndex = row.FirstCellNum;
					int lastIndex = row.LastCellNum;
					if (rowIndex == 0)
					{
						//如果第一行的首单元格不是0索引开始或者最后一个单元格索引是0，那认为是无效Sheet
						if (firstIndex != 0 || lastIndex == 0)
						{
							m_isVaild = false;
							return;
						}

						ICell cell = row.GetCell(0);

						string content = cell.GetCellValue(cell == null ? CellType.Unknown : cell.CellType);

						//如果第一行第一个单元格都是空字符串，那认为是无效Sheet
						if (string.IsNullOrEmpty(content))
						{
							m_isVaild = false;
							return;
						}

						m_columnNum = lastIndex - firstIndex;

					}

					Cell[] cells = new Cell[m_columnNum];

					for (int cellIndex = firstIndex; cellIndex < lastIndex; cellIndex++)
					{
						Cell cell = new Cell(row.GetCell(cellIndex), rowIndex, cellIndex);
						if (cell.IsVaild == false)
						{
							throw new Exception($"表{m_excelName}的Sheet:{m_sheetName}中，出现无效单元格，位置：({rowIndex},{cellIndex})");
						}

						//如果该行的第一个单元格都是空字符串，那读表结束，第一行前面校验过了
						if (cellIndex == 0 && string.IsNullOrEmpty(cell.CellValue))
						{
							return;
						}

						//数组最长是第一行计算出的，后面出现超过这个长度的全部忽略
						if (cellIndex < m_columnNum)
						{
							cells[cellIndex] = cell;
						}
					}

					m_cells.Add(cells);

				}
			}

			m_rowNum = m_cells.Count;

			if (m_rowNum <= 0)
			{
				m_isVaild = false;
				return;
			}

			m_isVaild = true;
		}

		#region Property

		private string m_sheetName;
		public string SheetName => m_sheetName;

		private int m_rowNum;
		public int RowNum => m_rowNum;

		private int m_columnNum;
		public int ColumnNum => m_columnNum;

		public Cell[] this[int index]
		{
			get
			{
				if(m_cells == null)
				{
					throw new Exception("没有单元格数据");
				}

				if(index < 0 || index >= m_cells.Count)
				{
					throw new Exception($"index less 0 or index out of range,index:{index},max:{m_cells.Count}");
				}

				return m_cells[index];
			}
		}

		public Cell this[int rowIndex,int colunmIndex]
		{
			get
			{
				Cell[] cells = this[rowIndex];

				if(cells == null)
				{
					return null;
				}

				if(cells.Length <= colunmIndex || colunmIndex < 0)
                {
                    throw new Exception($"colunmIndex less 0 or colunmIndex out of range,colunmIndex:{colunmIndex},maxColunmNum:{cells.Length}");
                }

				return cells[colunmIndex];
			}
		}

		#endregion

    }
}

