using System;
using System.Management;
using System.Threading;
using System.Threading.Tasks;
using ProcessMonitoring;

namespace proc_mon
{
    public class Program
    {
        private static AutoResetEvent _autoResetEvent = new AutoResetEvent(false); // 초기 상태는 non-signaled
        private static EventHandle _eventHandle = new EventHandle();

        public static async Task Main(string[] args)
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

	        // MSMQ 비동기 메서드를 주기적으로 호출
	        while (true)
	        {
		        await _eventHandle.IedAddRestartProcessAsync();
		        await Task.Delay(5000); // 5초마다 메세지 큐 확인
	        }

	        // 아래의 코드는 실행되지 않을 것입니다.
	        Console.WriteLine("프로세스 감시를 시작했습니다. 프로그램을 종료하려면 Ctrl+C를 누르세요.");
        }


	}
}
