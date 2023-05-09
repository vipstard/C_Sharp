using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OpenXml;


public class Program{
	public static void Main(string[] args)
	{
		string filePath = "C:\\IET_D000_C437.xlsx";
		List<IET> dataList = ReadExcelData(filePath);

		foreach (var iet in dataList)
		{
			Console.WriteLine(iet.DataType + " " + iet.Pd);
		}

	}


	public static List<IET> ReadExcelData(string filePath)
	{
		List<IET> dataList = new List<IET>();

		using (SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, false))
		{
			WorkbookPart workbookPart = document.WorkbookPart;
			Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == "DS_Goose");
			if (sheet == null) return dataList;

			WorksheetPart worksheetPart = workbookPart.GetPartById(sheet.Id) as WorksheetPart;
			if (worksheetPart == null) return dataList;

			// 공유 문자열 테이블 참조 가져오기
			SharedStringTablePart sharedStringsPart = workbookPart.SharedStringTablePart;
			SharedStringTable sharedStrings = sharedStringsPart.SharedStringTable;

			// 열 인덱스 찾기
			int dataTypeIndex = -1;
			int pdIndex = -1;

			// 헤더 행 및 데이터 행 찾기
			Row headRow = worksheetPart.Worksheet.Descendants<Row>().Skip(4).FirstOrDefault();
			Row SubRow = worksheetPart.Worksheet.Descendants<Row>().Skip(5).FirstOrDefault();

			
			if (headRow != null && SubRow != null)
			{
				// 5행에 있는 HeadData 명칭,DataType 등
				foreach (Cell cell in headRow.Descendants<Cell>())
				{
					string cellValue = GetCellValue(cell, sharedStrings);
					if (cellValue.Contains("명칭"))
					{

					}
					else if (cellValue == "Data Type")
					{
						dataTypeIndex = GetColumnIndex(cell);
					}
				}

				// 6행에 있는 HeadData PD ~ Data Attribute
				foreach (Cell cell in SubRow.Descendants<Cell>())
				{
					string cellValue = GetCellValue(cell, sharedStrings);
					if (cellValue == "PD")
					{
						pdIndex = GetColumnIndex(cell);
					}
				}
			}

			if (dataTypeIndex == -1 || pdIndex == -1) return dataList;

			// 데이터가 시작되는 행 찾기
			Row dataStartRow = worksheetPart.Worksheet.Descendants<Row>()
				.Where(r => r.Descendants<Cell>().Any(c => GetColumnIndex(c) == dataTypeIndex))
				.FirstOrDefault();

			if (dataStartRow == null) return dataList;

			// 행 반복
			foreach (Row row in worksheetPart.Worksheet.Descendants<Row>().SkipWhile(r => r.RowIndex < dataStartRow.RowIndex).Skip(6))
			{
				Cell dataTypeCell = row.Descendants<Cell>().FirstOrDefault(c => GetColumnIndex(c) == dataTypeIndex);
				Cell pdCell = row.Descendants<Cell>().FirstOrDefault(c => GetColumnIndex(c) ==pdIndex);

				string dataTypeValue = GetCellValue(dataTypeCell, sharedStrings);
				string PdTypeValue = GetCellValue(pdCell, sharedStrings);

				if (dataTypeValue != null && PdTypeValue != null)
				{
					dataList.Add(new IET { DataType = dataTypeValue, Pd = PdTypeValue });
				}
				
			}
		}

		return dataList;
	}

	private static int GetColumnIndex(Cell cell)
	{
		string cellReference = cell.CellReference.Value;
		string columnReference = new string(cellReference
			.TakeWhile(char.IsLetter)
			.ToArray());

		int columnIndex = 0;
		int factor = 1;

		for (int i = columnReference.Length - 1; i >= 0; i--)
		{
			columnIndex += factor * (columnReference[i] - 'A' + 1);
			factor *= 26;
		}

		return columnIndex;
	}

	private static string GetCellValue(Cell cell, SharedStringTable sharedStrings)
	{
		if (cell == null)
		{
			return null;
		}

		if (cell.DataType == null)
		{
			return cell.InnerText.Trim();
		}

		switch (cell.DataType.Value)
		{
			case CellValues.SharedString:
				return sharedStrings.ElementAt(int.Parse(cell.InnerText)).InnerText.Trim();

			case CellValues.Boolean:
				return cell.InnerText.Trim() == "1" || cell.InnerText.ToLower() == "true" ? "TRUE" : "FALSE";

			case CellValues.Number:
				return cell.InnerText.Trim();

			case CellValues.String:
				return cell.InnerText.Trim();

			case CellValues.Date:
				return DateTime.FromOADate(double.Parse(cell.InnerText)).ToString();

			default:
				return cell.InnerText.Trim();
		}
	}
}

