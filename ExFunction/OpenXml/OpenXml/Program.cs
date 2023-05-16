using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MDAS.Model.config;
using OpenXml;
using OpenXml.Model;

public class Program{
	public static void Main(string[] args)
	{
		ReadSheet rs = new ReadSheet();
		string filePath = "C:\\TESTIET\\.xlsx";

		List<IET> gooseDataList = rs.ReadGooseSheet(filePath);
		List<IET_MMS> mmsDataList = rs.ReadMmsSheet(filePath);

		int cnt = 1;
		foreach (var iet in gooseDataList)
		{
			Console.WriteLine($"{iet.description} {iet.DataType} {iet.Pd} {iet.ld} {iet.ln_Prefix} {iet.ln}  {iet.ln_Inst_No} {iet.fc} {iet.data_Object} {iet.data_Attribute}");
		}

		Console.WriteLine("=======Mms=========");
		foreach (var iet in mmsDataList)
		{
			Console.WriteLine($"{iet.Description}  {iet.Address} {iet.DataType}");
        }
	}
}

