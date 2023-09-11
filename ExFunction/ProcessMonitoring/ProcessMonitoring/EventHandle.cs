using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
                    Convert.ToUInt32(e.NewEvent.Properties["ExitStatus"].Value) == 4294967295 // error_mon 정상종료시 이 숫자가 나옴. 나머지는 해당 X
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Create_log_File(ex.Message);
                throw;
            }
        }

        // 프로그램 시작 시 현재 동작중인 프로세스 체크
        public void GetRunningProcesses()
        {
            List<Process> runningProcesses = Process.GetProcesses().ToList();
            LoadProcessInfo();

            _processInfos.ForEach(p => { p.Pid = 0; p.Status = 0; });

            foreach (Process process in runningProcesses)
            {
                // 여기에서 필요한 정보를 추출하고 ProcessInfo 객체를 생성하여 리스트에 추가
                string processName = process.ProcessName;
                int processId = process.Id;

                ProcessInfo existingProcessInfo = _processInfos.Find(pi => pi.Name == processName);

                if (existingProcessInfo != null)
                {
                    // 이미 있는 프로세스 정보가 있으면 업데이트
                    existingProcessInfo.Pid = processId;
                    existingProcessInfo.Status = 1; 

                    Console.WriteLine($"동작중인 프로세스 : {existingProcessInfo.Name}, Pid : {existingProcessInfo.Pid}, 상태 : {existingProcessInfo.Status}");
                }
            }

            UpdateProcessInfo();
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
        {   
            // PacketCap_Goos로 가져올 때가 종종있음.  있을때만 Goose로 변경 그 외 프로세스는 해당 X
            return processName.Split('.')[0] == "PacketCap_Goos" ? "PacketCap_Goose" : processName.Split('.')[0];
        }


        public enum ExitCode
        {
            NORMAL,
            ABNORMAL
        }

        #region System Log
        public void Create_log_File(string msg)
        {
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
        public string GetDateTime()
        {
            DateTime NowDate = DateTime.Now;
            return NowDate.ToString("yyyy-MM-dd HH:mm:ss") + ":" + NowDate.Millisecond.ToString("000");
        }
        #endregion
    }

}
