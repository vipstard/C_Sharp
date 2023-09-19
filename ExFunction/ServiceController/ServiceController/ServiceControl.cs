using System.Diagnostics;

namespace ServiceController
{
    using System.ServiceProcess;

    public class ServiceControl
    {
        public void RestartService()
        {
            string serviceName = "wnms service";

            try
            {
                ServiceController serviceController = new ServiceController(serviceName);

                if (serviceController.Status == ServiceControllerStatus.Running)
                {
                    serviceController.Stop();
                    serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                }

                serviceController.Start();
                serviceController.WaitForStatus(ServiceControllerStatus.Running);
            }
            catch (Exception e)
            {
                
            }
        }

        public void StropProcess()
        {
	        // 종료할 프로세스 이름 또는 경로
	        string[] processName = { "goose_mon, mms_mon" }; // 예: "notepad" 또는 "C:\\Path\\To\\YourApp.exe"

	        // 프로세스 이름 또는 경로를 기반으로 프로세스 찾기
	        foreach (var p in processName)
	        {
				Process[] processes = Process.GetProcessesByName(p);

				if (processes.Length > 0)
				{
					// 프로세스가 여러 개일 경우, 모든 프로세스를 종료합니다.
					foreach (Process process in processes)
					{
						try
						{
							process.Kill(); // 프로세스 종료
							process.WaitForExit(); // 종료될 때까지 대기
							Console.WriteLine($"프로세스 {process.ProcessName} (PID: {process.Id}) 종료됨");
						}
						catch (Exception ex)
						{
							Console.WriteLine($"프로세스 종료 중 오류 발생: {ex.Message}");
						}
					}
				}
				else
				{
					Console.WriteLine("해당 프로세스가 실행 중이지 않습니다.");
				}

			}

		}


    }
}
