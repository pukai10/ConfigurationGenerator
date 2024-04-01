using System;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.Text;

namespace AurogonExcel
{
	public class ExcelReader
	{
		private string m_excelPath;
		public string ExcelPath => m_excelPath;
		private string m_excelName;
		public string ExcelName => m_excelName;

		private List<Sheet> m_sheets;
		public ExcelReader(string excelPath)
		{
			m_excelPath = excelPath;
			m_excelName = Path.GetFileName(m_excelPath);
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
            IWorkbook workbook = null;
			switch(extension)
			{
				case ".xls":
                    workbook = new HSSFWorkbook(fs);
                    break;
				case ".xlsx":
                    workbook = new XSSFWorkbook(fs);
					break;
				default:
					return;
			}

			fs.Close();
			fs.Dispose();

			if(workbook == null)
			{
				throw new Exception($"open excel failed,file extension must 'xls' or 'xlsx',please check your file:{m_excelPath}");
			}
			m_sheets = new List<Sheet>();
            m_sheetNum = workbook.NumberOfSheets;
			for (int i = 0; i < m_sheetNum; i++)
			{
				Sheet sheet = new Sheet(workbook.GetSheetAt(i), m_excelName);

				if(sheet.IsVaild)
				{
					m_sheets.Add(sheet);
                }
			}
		}


		#region 属性

		private int m_sheetNum;
		public int SheetNum => m_sheetNum;

		private int m_rowNum;
		public int RowNum => m_rowNum;

		private int m_columnNum;
		public int ColumnNum => m_columnNum;

        #endregion

		public Sheet this[int index]
		{
			get
			{
				if(m_sheets == null || m_sheets.Count <= index || index < 0)
				{
					throw new Exception("sheets is null or index out of range,or index less 0");
				}

				return m_sheets[index];
			}
		}

		public Sheet this[string sheetName]
		{
			get
			{
				if(m_sheets == null)
				{
					throw new Exception("excel no sheet");
				}

				for (int i = 0; i < m_sheets.Count; i++)
				{
					Sheet sheet = m_sheets[i];
					if(sheet != null && sheet.IsVaild && sheet.SheetName == sheetName)
					{
						return sheet;
					}
				}

				return null;
			}
		}

        public override string ToString()
        {
			StringBuilder sb = new StringBuilder();
			sb.AppendLine($"ExcelName:{m_excelName},Path:{m_excelPath}");

			for(int i = 0; i < SheetNum; i++)
			{
				var sheet = this[i];
				sb.AppendLine(sheet.ToString());
				sb.AppendLine("");
			}

			return sb.ToString();
        }
    }

	public class WorkbookNullException : Exception
	{
		public WorkbookNullException():base("excel workBook is null")
		{

		}
	}

	public class SheetNullException : Exception
	{
		public SheetNullException(string sheetName):base($"sheet:{sheetName} is null") { }
	}
}

