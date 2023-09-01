using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using proc_mon.Model;

namespace proc_mon
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            DbManager _dbManager = new DbManager();
            List<ProcessInfo> processInfos = _dbManager.GetProcessInfo();

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
                    Console.WriteLine("\n");
                }

                _dbManager.UpdateProcessInfo(processInfos);

                Thread.Sleep(5000);
            }
        }

        public static bool IsProcessRunning(string processName, out int pid)
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
    }
}