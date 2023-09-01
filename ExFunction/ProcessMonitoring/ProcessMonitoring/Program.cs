using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using proc_mon.Model;

namespace proc_mon
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DbManager _dbManager = new DbManager();
            List<ProcessInfo> processInfos = _dbManager.GetProcessInfo();

            // WMI 이벤트 감시기 생성
            var processStartWatcher = new ManagementEventWatcher(
                new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));

            var processStopWatcher = new ManagementEventWatcher(
                new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));

            // 프로세스 시작 및 종료 이벤트 핸들러 등록
            processStartWatcher.EventArrived += ProcessStartedHandler;
            processStopWatcher.EventArrived += ProcessStoppedHandler;

            // 이벤트 감시 시작
            processStartWatcher.Start();
            processStopWatcher.Start();

            while (true)
            {
                foreach (ProcessInfo processInfo in processInfos)
                {
                    if (IsProcessRunning(processInfo.Name, out int pid))
                    {
                        processInfo.Status = 1; // 프로세스가 실행 중인 경우
                        processInfo.Pid = pid; // 프로세스의 Pid 저장
                    }
                    else
                    {
                        processInfo.Status = 0; // 프로세스가 종료된 경우
                        processInfo.Pid = 0; // Pid를 0으로 초기화
                    }

                    Console.WriteLine($"{processInfo.Name} : 상태 {processInfo.Status}, Pid {processInfo.Pid}");
                }

                _dbManager.UpdateProcessInfo(processInfos);

                Console.WriteLine("\n");
                System.Threading.Thread.Sleep(5000);
            }
        }

        private static bool IsProcessRunning(string processName, out int pid)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length > 0)
            {
                pid = processes[0].Id;
                return true;
            }
            pid = 0;
            return false;
        }

        private static void ProcessStartedHandler(object sender, EventArrivedEventArgs e)
        {
            string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
            int processId = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);

            Console.WriteLine($"프로세스 시작: {processName}, Pid: {processId}");
            // 여기서 로그를 저장하거나 원하는 작업을 수행할 수 있습니다.
        }

        private static void ProcessStoppedHandler(object sender, EventArrivedEventArgs e)
        {
            string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
            int processId = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);

            Console.WriteLine($"프로세스 종료: {processName}, Pid: {processId}");
            // 여기서 로그를 저장하거나 원하는 작업을 수행할 수 있습니다.
        }
    }
}
