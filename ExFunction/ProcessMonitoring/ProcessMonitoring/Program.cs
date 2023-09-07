using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using proc_mon.Model;
using System.Threading;

namespace proc_mon
{
    public class Program
    {
        private static List<ProcessInfo> _processInfos = null;
        private static DbManager _dbManager = new DbManager();
        private static AutoResetEvent _autoResetEvent = new AutoResetEvent(false); // 초기 상태는 non-signaled

        private static void Main(string[] args)
        {
            _processInfos = _dbManager.GetProcessInfo();

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

            // 프로그램 종료 방지를 위해 대기
            Console.WriteLine("프로세스 감시를 시작했습니다. 프로그램을 종료하려면 Ctrl+C를 누르세요.");
            _autoResetEvent.WaitOne();
        }

        private static void ProcessStartedHandler(object sender, EventArrivedEventArgs e)
        {
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
                    break;
                }
            }

            // 프로세스 시작 이벤트가 발생할 때 DB 업데이트
            _dbManager.UpdateProcessInfo(_processInfos);
        }

        private static void ProcessStoppedHandler(object sender, EventArrivedEventArgs e)
        {
            string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
            int processId = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);

            foreach (ProcessInfo processInfo in _processInfos)
            {
                processName = processName.Split('.')[0] == "PacketCap_Goos" ? "PacketCap_Goose" : processName.Split('.')[0];

                if (processInfo.Name == processName)
                {
                    Console.WriteLine($"프로세스 종료: {processName}, Pid: {processId}");
                    processInfo.Status = 0;
                    processInfo.Pid = 0;
                    break;
                }
            }

            // 프로세스 종료 이벤트가 발생할 때 DB 업데이트
            _dbManager.UpdateProcessInfo(_processInfos);
        }
    }
}
