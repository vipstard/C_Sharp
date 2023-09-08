using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using proc_mon;
using proc_mon.Model;
using ProcessMonitoring.Model;

namespace ProcessMonitoring
{
    public class EventHandle
    {
        private  List<ProcessInfo> _processInfos = null;
        private  DbManager _dbManager = new DbManager();

         public void ProcessStartedHandler(object sender, EventArrivedEventArgs e)
        {
            _processInfos = _dbManager.GetProcessInfo();
            string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
            int processId = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);


            foreach (ProcessInfo processInfo in _processInfos)
            {
                processName = processName.Split('.')[0];
                if (processInfo.Name == processName)
                {
                    Console.WriteLine($"프로세스 시작: {processName}, Pid: {processId}");
                    processInfo.Status = 1;
                    processInfo.Pid = processId;

                    _dbManager.InsertLog(new ProcessEvent(processName, 1, 0));
                    break;
                }
            }

            // 프로세스 시작 이벤트가 발생할 때 DB 업데이트
            _dbManager.UpdateProcessInfo(_processInfos);
        }

        public void ProcessStoppedHandler(object sender, EventArrivedEventArgs e)
        {
            try
            {
                _processInfos = _dbManager.GetProcessInfo();
                string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
                int processId = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);
                uint exitCode = Convert.ToUInt32(e.NewEvent.Properties["ExitStatus"].Value) == 4294967295 ? 0 : Convert.ToUInt32(e.NewEvent.Properties["ExitStatus"].Value);


                foreach (ProcessInfo processInfo in _processInfos)
                {
                    // 시스템 자체에서 PacketCap_Goos로  들어올때가 있어서 추가 
                    processName = processName.Split('.')[0] == "PacketCap_Goos" ? "PacketCap_Goose" : processName.Split('.')[0];

                    if (processInfo.Name == processName)
                    {
                        Console.WriteLine($"프로세스 종료: {processName}, Pid: {processId}, EXITCODE : {exitCode}");

                        // 종료된 프로세스의 ExitCode를 확인
                        switch (exitCode)
                        {
                            case 0:  // 종료 코드가 0인 경우, 정상 종료
                                _dbManager.InsertLog(new ProcessEvent(processName, 0, ExitCode.NORMAL));
                                break;
                            case 1:   // 종료 코드가 1인 경우, 비정상 종료
                                _dbManager.InsertLog(new ProcessEvent(processName, 0, ExitCode.ABNORMAL));
                                break;
                        }

                        processInfo.Status = 0;
                        processInfo.Pid = 0;
                        break;
                    }
                }

                // 프로세스 종료 이벤트가 발생할 때 DB 업데이트
                _dbManager.UpdateProcessInfo(_processInfos);

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
           
        }

        public enum ExitCode
        {
            NORMAL,
            ABNORMAL

        }


    }
}
