namespace TextLog
{
	internal class Program
	{
		static void Main(string[] args)
		{
		
		}

		public static void create_log_file(string msg)
		{
			string runtimePath = AppDomain.CurrentDomain.BaseDirectory + "..\\Log\\log.txt";
			string FilePath = AppDomain.CurrentDomain.BaseDirectory + "..\\Log\\" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
			string DirPath = AppDomain.CurrentDomain.BaseDirectory + "..\\Log";
			string temp;

			DirectoryInfo di = new DirectoryInfo(DirPath);
			FileInfo fi = new FileInfo(FilePath);
			try
			{
				if (di.Exists != true) Directory.CreateDirectory(DirPath);

				if (fi.Exists != true)
				{
					using (StreamWriter sw = new StreamWriter(FilePath))
					{
						temp = string.Format("[{0}] {1}", GetDateTime(), msg);
						sw.WriteLine(temp);
						sw.Close();
					}
				}
				else
				{
					using (StreamWriter sw = File.AppendText(FilePath))
					{
						temp = string.Format("[{0}] - {1}", GetDateTime(), msg);
						sw.WriteLine(temp);
						sw.Close();
					}
				}
			}
			catch (Exception)
			{
			}
		}

		public static string GetDateTime()
		{
			DateTime NowDate = DateTime.Now;
			return NowDate.ToString("yyyy-MM-dd HH:mm:ss") + ":" + NowDate.Millisecond.ToString("000");
		}
	}
}