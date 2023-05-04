using System;
using System.Diagnostics;
using System.ServiceProcess;

namespace GetProcessInfo
{
	public class Program
	{
		static void Main(string[] args)
		{
			string Path = @"C:\nms4sa\PacketCap_DNP.exe";
			RestartService();
			Process.Start(Path);
		}

		/// <summary>
		/// Process 동작 중 or 정지 상태 판단
		/// </summary>
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

		/// <summary>
		/// 서비스 재시작
		/// </summary>
		private static void RestartService()
		{
			string processName = "PacketCap_DNP"; // 재시작할 프로세스 이름

			// 서비스 컨트롤러 생성
			ServiceController controller = new ServiceController(processName);

			// 서비스 중지
			if (controller.Status != ServiceControllerStatus.Stopped)
			{
				controller.Stop();
				controller.WaitForStatus(ServiceControllerStatus.Stopped);
			}

			// 서비스 시작
			controller.Start();
			controller.WaitForStatus(ServiceControllerStatus.Running);

		}
	}
}