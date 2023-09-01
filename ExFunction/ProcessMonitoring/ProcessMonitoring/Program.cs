using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using proc_mon;
using proc_mon.Model;

namespace ProcessMonitoring
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DbManager _dbManager = new DbManager();
            List<ProcessInfo> processInfos = _dbManager.GetProcessInfo();

            while (true)
            {
                foreach (ProcessInfo processInfo in processInfos)
                {
                    if (IsProcessRunning(processInfo.Name))
                    {
                        processInfo.Status = 1; // 프로세스가 실행 중인 경우
                    }
                    else
                    {
                        processInfo.Status = 0; // 프로세스가 종료된 경우
                    }
                    Console.WriteLine($"{processInfo.Name} : 상태 {processInfo.Status}");
                }



                Thread.Sleep(5000);
            }
        }

        public static bool IsProcessRunning(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            return processes.Length > 0;
        }
    }
}
