using System;
using System.Collections.Generic;
using System.Management;
using proc_mon;
using proc_mon.Model;
using ProcessMonitoring.Model;

namespace ProcessMonitoring
{
    public class EventHandle
    {
        private List<ProcessInfo> _processInfos = null;
        private DbManager _dbManager = new DbManager();

        public void ProcessStartedHandler(object sender, EventArrivedEventArgs e)
        {
            LoadProcessInfo();

            string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
            int processId = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);
            processName = GetNormalizedProcessName(processName);

            ProcessInfo processInfo = _processInfos.Find(pi => pi.Name == processName);

            if (processInfo != null)
            {
                Console.WriteLine($"프로세스 시작: {processName}, Pid: {processId}");
                processInfo.Status = 1;
                processInfo.Pid = processId;
                _dbManager.InsertLog(new ProcessEvent(processName, 1, 0));
            }

            UpdateProcessInfo();
        }

        public void ProcessStoppedHandler(object sender, EventArrivedEventArgs e)
        {
            try
            {
                LoadProcessInfo();

                string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
                int processId = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);
                uint exitCode = 
                    Convert.ToUInt32(e.NewEvent.Properties["ExitStatus"].Value) == 4294967295
                    ? 0
                    : Convert.ToUInt32(e.NewEvent.Properties["ExitStatus"].Value);

                processName = GetNormalizedProcessName(processName);

                ProcessInfo processInfo = _processInfos.Find(pi => pi.Name == processName);

                if (processInfo != null)
                {
                    Console.WriteLine($"프로세스 종료: {processName}, Pid: {processId}, EXITCODE : {exitCode}");

                    ExitCode exitCodeEnum = exitCode == 0 ? ExitCode.NORMAL : ExitCode.ABNORMAL;
                    _dbManager.InsertLog(new ProcessEvent(processName, 0, exitCodeEnum));

                    processInfo.Status = 0;
                    processInfo.Pid = 0;
                }

                UpdateProcessInfo();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
        }

        private void LoadProcessInfo()
        {
            if (_processInfos == null)
            {
                _processInfos = _dbManager.GetProcessInfo();
            }
        }

        private void UpdateProcessInfo()
        {
            _dbManager.UpdateProcessInfo(_processInfos);
        }

        private string GetNormalizedProcessName(string processName)
        {   // PacketCap_Goos로 가져올 때가 종종있음.  있을때만 Goose로 변경 그외 프로세스는 해당 X
            return processName.Split('.')[0] == "PacketCap_Goos" ? "PacketCap_Goose" : processName.Split('.')[0];
        }

        public enum ExitCode
        {
            NORMAL,
            ABNORMAL
        }
    }

}
