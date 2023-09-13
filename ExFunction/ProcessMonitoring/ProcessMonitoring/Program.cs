using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using proc_mon.Model;
using System.Threading;
using ProcessMonitoring;
using ProcessMonitoring.Model;

namespace proc_mon
{
    public class Program
    {
        private static AutoResetEvent _autoResetEvent = new AutoResetEvent(false); // 초기 상태는 non-signaled
        private static EventHandle _eventHandle = new EventHandle();

        private static void Main(string[] args)
        {
            DbManager dbManager = new DbManager();
            
            // 프로그램 실행전에 이미 동작중인 프로세스 상태 업데이트
            _eventHandle.GetRunningProcesses();

            // WMI 이벤트 감시기 생성
            var processStartWatcher = new ManagementEventWatcher(
                new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));

            var processStopWatcher = new ManagementEventWatcher(
                new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));

            // 프로세스 시작 및 종료 이벤트 핸들러 등록
            processStartWatcher.EventArrived += _eventHandle.ProcessStartedHandler;
            processStopWatcher.EventArrived += _eventHandle.ProcessStoppedHandler;
            

            // 이벤트 감시 시작
            processStartWatcher.Start();
            processStopWatcher.Start();

            Console.WriteLine("프로세스 감시를 시작했습니다. 프로그램을 종료하려면 Ctrl+C를 누르세요.");
            _autoResetEvent.WaitOne();
        }
    }
}
