using System;
using NPOI.SS.UserModel;

namespace ConfigurationGenerator
{
	public class CellValue
	{
		private CellValueType m_cellValueType;

        public CellValue(ICell cell)
		{
			InitCell(cell);
		}


		private void InitCell(ICell cell)
		{
			if(cell == null)
			{
				throw new ArgumentNullException("cell is null");
			}


			m_cellValueType = GetCellValueType(cell.CellType, cell);

        }


		private CellValueType GetCellValueType(CellType cellType,ICell cell)
		{
			switch(cellType)
			{
				case CellType.Numeric:
					return CellValueType.Int;
				case CellType.Boolean:
					return CellValueType.Boolean;
				case CellType.String:
					return CellValueType.String;
				case CellType.Formula:
					return GetCellValueType(cell.CachedFormulaResultType, cell);
				default:
					return CellValueType.Null;
			}
		}
	}

	public enum CellValueType
	{
		Int,
		Float,
		Boolean,
		String,
		Null
	}

}

