using System;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ConfigurationGenerator
{
	public class ExcelReader
	{
		private string m_excelPath;
		private IWorkbook m_workbook;
		public ExcelReader(string excelPath)
		{
			m_excelPath = excelPath;
			InitExcel();
        }



		private void InitExcel()
		{
			if(string.IsNullOrEmpty(m_excelPath))
			{
				throw new Exception("excel file path is empty");
			}

			if(!File.Exists(m_excelPath))
			{
				throw new Exception($"{m_excelPath} file is not exist");
			}

			string extension = Path.GetExtension(m_excelPath);

			FileStream fs = new FileStream(m_excelPath, FileMode.Open, FileAccess.Read);
			Console.WriteLine(extension);
			m_workbook = null;
			switch(extension)
			{
				case ".xls":
                    m_workbook = new HSSFWorkbook(fs);
                    break;
				case ".xlsx":
                    m_workbook = new XSSFWorkbook(fs);
					break;
			}

			fs.Close();
			fs.Dispose();

			if(m_workbook == null)
			{
				throw new Exception($"open excel failed,file extension must 'xls' or 'xlsx',please check your file:{m_excelPath}");
			}

			//int sheetNum = workbook.NumberOfSheets;
			//for(int i = 0; i < sheetNum; i++)
			//{
			//	ISheet sheet = workbook.GetSheetAt(i);
			//	Console.WriteLine(sheet.SheetName);

			//	Console.WriteLine($"{sheet.FirstRowNum} {sheet.LastRowNum}");
			//	for (int j = sheet.FirstRowNum; j <= sheet.LastRowNum; j++)
			//	{
			//		IRow row = sheet.GetRow(j);

   //                 Console.WriteLine($"row:{row.FirstCellNum} {row.LastCellNum}");
   //                 for (int cellIndex = row.FirstCellNum; cellIndex < row.LastCellNum; cellIndex++)
			//		{
			//			ICell cell = row.GetCell(cellIndex);
			//			if(cell == null)
			//			{
			//				//Console.Write("cell is null,index is" + cellIndex);
			//				continue;
			//			}


			//			if(cell.CellType == CellType.Formula)
			//			{
			//				Console.Write($"公式：{cell.CachedFormulaResultType} {cell.NumericCellValue},");
			//			}

			//			Console.Write($"{cell} {cell.CellType} ");
			//		}
			//		Console.Write("\n");
			//	}
			//}
		}


        #region 属性

		public int SheetNum
		{
			get
			{
				if(m_workbook == null)
				{
					throw new WorkbookNullException();
				}

				return m_workbook.NumberOfSheets;
			}
		}



        #endregion

    }

	public class WorkbookNullException : Exception
	{
		public WorkbookNullException():base("excel workBook is null")
		{

		}
	}
}

