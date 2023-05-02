namespace GetProcessInfo
{
	internal class Program
	{
		static void Main(string[] args)
		{
			GetPidAndStatus();
		}

		private static void GetPidAndStatus()
		{
			System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName("PacketCap_DNP");
			int pid = 0;
			bool isRunning = false;

			foreach (var process in processes)
			{
				pid	= process.Id;
				isRunning	 = !process.HasExited;
			}

			Console.WriteLine($"PID: {pid}, 동작 상태: {(isRunning ? "동작 중" : "종료됨")}");

		}
	}
}