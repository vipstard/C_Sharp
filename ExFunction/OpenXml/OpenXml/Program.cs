using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OpenXml;


public class Program{
	public static void Main(string[] args)
	{
		string filePath = "C:\\TESTIET\\";
		List<IET> dataList = ReadExcelData(filePath);

		foreach (var iet in dataList)
		{
			Console.WriteLine($"{iet.description} {iet.DataType} {iet.Pd} {iet.ld} {iet.ln_Prefix} {iet.ln}  {iet.ln_Inst_No} {iet.fc} {iet.data_Object} {iet.data_Attribute}");
		}

	}


	public static List<IET> ReadExcelData(string filePath)
	{
		List<IET> dataList = new List<IET>();

		using (SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, false))
		{
			WorkbookPart workbookPart = document.WorkbookPart;
			Sheet sheet = workbookPart.Workbook.Descendants<Sheet>()
				.Where(s => s.Name.Value.Contains("DS_Goose") || s.Name.Value.Contains("GOOSEPUB"))
				.FirstOrDefault();

			if (sheet == null) return dataList;

			WorksheetPart worksheetPart = workbookPart.GetPartById(sheet.Id) as WorksheetPart;
			if (worksheetPart == null) return dataList;

			// 공유 문자열 테이블 참조 가져오기
			SharedStringTablePart sharedStringsPart = workbookPart.SharedStringTablePart;
			SharedStringTable sharedStrings = sharedStringsPart.SharedStringTable;

			// 열 인덱스 찾기
			int dataTypeIndex = -1;
			int pdIndex = -1;
			// colIndex[0] : 명칭 , [1] : Data Type, [2] : PD, [3] : LD, [4] : LN-Prefix, [5] : LN, [6] : LN-Inst No, [7] : FC, [8] : Data Object, [9] : Data Attribute
			int[] colIndex = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };

			// 열 병합 되있는 부분 DataType 같은 부분은 5행부터 시작 PD 부터 는 6행부터 시작해서 분리
			Row headRow = worksheetPart.Worksheet.Descendants<Row>().Skip(4).FirstOrDefault();
			Row SubRow = worksheetPart.Worksheet.Descendants<Row>().Skip(5).FirstOrDefault();

			
			if (headRow != null && SubRow != null)
			{
				#region 5행에 있는 HeadData 명칭,DataType 등
				foreach (Cell cell in headRow.Descendants<Cell>())
				{
					string cellValue = GetCellValue(cell, sharedStrings);

					if (cellValue.Contains("명칭"))
					{
						colIndex[0] = GetColumnIndex(cell);
					}
					else if (cellValue == "Data Type")
					{
						colIndex[1] = GetColumnIndex(cell);
					}
				}
				#endregion

				#region 6행에 있는 HeadData PD 부터 Data Attribute
				foreach (Cell cell in SubRow.Descendants<Cell>())
				{
					string cellValue = GetCellValue(cell, sharedStrings);
					if (cellValue == "PD")
					{
						colIndex[2] = GetColumnIndex(cell);
					}
					else if (cellValue == "LD")
					{
						colIndex[3] = GetColumnIndex(cell);
					}
					else if (cellValue == "LN-Prefix")
					{
						colIndex[4] = GetColumnIndex(cell);
					}
					else if (cellValue == "LN")
					{
						colIndex[5] = GetColumnIndex(cell);
					}
					else if (cellValue == "LN-Inst No.")
					{
						colIndex[6] = GetColumnIndex(cell);
					}
					else if (cellValue == "FC")
					{
						colIndex[7] = GetColumnIndex(cell);
					}
					else if (cellValue == "Data Object")
					{
						colIndex[8] = GetColumnIndex(cell);
					}
					else if (cellValue == "Data Attribute")
					{
						colIndex[9] = GetColumnIndex(cell);
					}
				}
				#endregion
			}

			if (colIndex[0] == -1 || colIndex[1] == -1 || colIndex[2] == -1) return dataList;

			// 데이터가 시작되는 행 찾기
			Row dataStartRow = worksheetPart.Worksheet.Descendants<Row>()
				.Where(r => r.Descendants<Cell>().Any(c => GetColumnIndex(c) == colIndex[1]))
				.FirstOrDefault();

			if (dataStartRow == null) return dataList;

			// 행 반복
			foreach (Row row in worksheetPart.Worksheet.Descendants<Row>().SkipWhile(r => r.RowIndex < dataStartRow.RowIndex).Skip(6))
			{
				#region Cell 값 받아오기

				string tempDescriptionValue1 = GetCellValue(ReturnCell(row, colIndex[0]), sharedStrings);
				string tempDescriptionValue2 = GetCellValue(ReturnCell(row, colIndex[0] + 1), sharedStrings);
				string descriptionValue =  tempDescriptionValue1 + tempDescriptionValue2;
				string dataTypeValue = GetCellValue(ReturnCell(row, colIndex[1]), sharedStrings);
				string pdTypeValue = GetCellValue(ReturnCell(row, colIndex[2]), sharedStrings);
				string ldTypeValue = GetCellValue(ReturnCell(row, colIndex[3]), sharedStrings);
				string lnPreFixTypeValue = GetCellValue(ReturnCell(row, colIndex[4]), sharedStrings);
				string lnTypeValue = GetCellValue(ReturnCell(row, colIndex[5]), sharedStrings);
				string lnInstNoTypeValue = GetCellValue(ReturnCell(row, colIndex[6]), sharedStrings);
				string fcTypeValue = GetCellValue(ReturnCell(row, colIndex[7]), sharedStrings);
				string dataObjectTypeValue = GetCellValue(ReturnCell(row, colIndex[8]), sharedStrings);
				string dataAttributeTypeValue = GetCellValue(ReturnCell(row, colIndex[9]), sharedStrings);
				#endregion

				if (descriptionValue != null && dataTypeValue != null && pdTypeValue != null)
				{
					dataList.Add(new IET
					{
						description = descriptionValue, DataType = dataTypeValue, Pd = pdTypeValue,
						ld = ldTypeValue, ln_Prefix = lnPreFixTypeValue, ln = lnTypeValue, ln_Inst_No = lnInstNoTypeValue,
						fc=fcTypeValue, data_Object = dataObjectTypeValue, data_Attribute = dataAttributeTypeValue
					});
				}
				
			}
		}

		return dataList;
	}

	private static Cell ReturnCell(Row row, int colIndex)
	{
		return row.Descendants<Cell>().FirstOrDefault(c => GetColumnIndex(c) == colIndex);
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

