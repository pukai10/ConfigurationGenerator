using NPOI.SS.UserModel;

namespace AurogonExcel
{
	public static class ExcelExtension
	{

        public static string GetCellValue(this ICell cell, CellType cellType)
        {
            if (cell == null)
            {
                return string.Empty;
            }

            switch (cellType)
            {
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Numeric:
                    return cell.NumericCellValue.ToString();
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Formula:
                    return GetCellValue(cell, cell.CachedFormulaResultType);
                default:
                    return string.Empty;
            }
        }

        public static ECellType GetCellType(this ICell cell,CellType cellType)
        {
            if(cell == null)
            {
                return ECellType.Null;
            }

            switch(cellType)
            {
                case CellType.Numeric:
                    return ECellType.Number;
                case CellType.Boolean:
                    return ECellType.Boolean;
                case CellType.String:
                    return ECellType.String;
                case CellType.Formula:
                    return cell.GetCellType(cell.CachedFormulaResultType);
                default:
                    return ECellType.Null;
            }
        }
    }
}

