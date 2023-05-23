namespace pcapFile_ReadToWriteExport
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello, World!");
		}
		static public string Export(/*[FromBody] SingleFileExportParam param*/)
		{
			//Save at Windows Temporary Folder
			string Root = Path.GetTempPath() + "/";
			string fileName = Root;
			return fileName;
		}

	}
}