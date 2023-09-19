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
                // 예외 처리: 서비스를 다룰 때 오류가 발생할 수 있으므로 필요한 예외 처리를 여기에 추가하세요.
            }
        }


    }
}
