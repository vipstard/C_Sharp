using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Messaging;
using System.Threading;
using System.Threading.Tasks;
using proc_mon;
using proc_mon.Model;
using ProcessMonitoring.Model;

namespace ProcessMonitoring
{
    public class EventHandle
    {
        private List<ProcessInfo> _processInfos = null;
        private DbManager _dbManager = new DbManager();
        private List<ProcessInfo> _processArr = null;
        private bool isAnyProcessDown = false;
        private string _timeStamp = null;
        private string _updateTimeStamp = null;
        private readonly string _queuePath = ".\\private$\\MonQueue";


		public void ProcessStartedHandler(object sender, EventArrivedEventArgs e)
        {
            LoadProcessInfo();

            string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
            int processId = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);
            processName = GetNormalizedProcessName(processName);

            ProcessInfo processInfo = _processInfos.Find(pi => pi.Name == processName);

            if (processInfo != null)
            {
	            string timeStamp = DateTime.Now.ToString();
	            processInfo.Status = 1;
                processInfo.Pid = processId;

                Console.WriteLine($"프로세스 시작: {processName}, Pid: {processId}, timeStamp : {timeStamp}");
				_dbManager.InsertLog(new ProcessEvent(timeStamp, processName, 1, 0));
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
                uint exitCode = Convert.ToUInt32(e.NewEvent.Properties["ExitStatus"].Value);

                // wnms service 정상 종료 시켜도 exitCode가 튀는 경우가 있음. 0으로 변경작업
                switch (exitCode)
                {
                    case 4294967295:
                        exitCode = 0; 
                        break;

                    case 3221225786: //SNMP_MON, mms_mon
                        exitCode = 0;
                        break;

                    case 3221225477:
                        exitCode = 0;
                        break;

                    case 62097:
                        exitCode = 0;
                        break;
                }

                
                processName = GetNormalizedProcessName(processName);

                ProcessInfo processInfo = _processInfos.Find(pi => pi.Name == processName);

                if (processInfo != null)
                {
	                _timeStamp = _updateTimeStamp != null ? _updateTimeStamp : DateTime.Now.ToString();
	                Console.WriteLine($"프로세스 종료: {processName}, Pid: {processId}, EXITCODE : {exitCode}, TimeStamp : {_timeStamp}");

                    ExitCode exitCodeEnum = exitCode == 0 ? ExitCode.NORMAL : ExitCode.ABNORMAL;
                    _dbManager.InsertLog(new ProcessEvent(_timeStamp, processName, 0, exitCodeEnum));

                    processInfo.Status = 0;
                    processInfo.Pid = 0;
                    _timeStamp = null;
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
            Console.WriteLine();
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
        private void UpdateExitProcessEvent(string timeStamp, int extraInfo)
        {
	        _dbManager.UpdateExitProcessEvent(timeStamp, extraInfo);
        }

		private string GetNormalizedProcessName(string processName)
        {   
            // PacketCap_Goos로 가져올 때가 종종있음.  있을때만 Goose로 변경 그 외 프로세스는 해당 X
            return processName.Split('.')[0] == "PacketCap_Goos" ? "PacketCap_Goose" : processName.Split('.')[0];
        }


  

        /// <summary>
        /// IED 추가시 프로세스 종료시키고 별도로 ProcessStartedHandler가 이벤트감지하고 DB에 Insert 후
        /// 여기서 timestamp 일치하는 데이터 extraInfo를 3 (IED추가) 으로 업데이트 
        /// </summary>
        /// <param name="RestartProcessFlag"></param>
		public void IedAddRestartProcess(bool RestartProcessFlag)
		{
			while (RestartProcessFlag)
			{
				try
				{
					string[] processes = { "goose_mon", "mms_mon" };

					// 데이터베이스 폴링을 통해 변경 사항 확인
					if (_dbManager.DatabaseHasNewData())
					{
						foreach (var process in processes)
						{
							var existingProcess = Process.GetProcessesByName(process).FirstOrDefault();
							if (existingProcess != null)
							{
								existingProcess.Kill();
								existingProcess.WaitForExit();

								_updateTimeStamp = DateTime.Now.ToString();
								Console.WriteLine($"TimeStamp: {_updateTimeStamp}");

								Thread.Sleep(3000);

								// Update는 제일 마지막에 실행
								UpdateExitProcessEvent(_updateTimeStamp, 3);// IED 추가시 extraInfo=3
								_updateTimeStamp = null;
							}
						}
					}

					// 일정 간격으로 폴링을 하기 위해 기다림
					Thread.Sleep(5000); // 5초마다 폴링
				}
				catch (Exception e)
				{
					Console.WriteLine($"Error: {e.Message}");
					// 예외 처리
				}
			}
		}

        /// <summary>
        /// 위의 함수와 역할 같음 MSMQ사용. 현재 사용 X
        /// </summary>
        /// <param name="RestartProcessFlag"></param>
        public void IedAddRestartProcessMSMQ(bool RestartProcessFlag)
        {
            while (RestartProcessFlag)
            {

                try
                {
                    string[] processes = { "goose_mon", "mms_mon" };

                    // 큐가 없으면 생성
                    if (!MessageQueue.Exists(_queuePath))
                    {
                        MessageQueue.Create(_queuePath);
                    }

                    // 메세지 받기, 메세지 있으면 제일 먼저 실행
                    using (MessageQueue queue = new MessageQueue(_queuePath))
                    {
                        queue.Formatter = new XmlMessageFormatter(new string[] { "System.String,mscorlib" });

                        Message message = queue.Receive();
                        string messageText = message.Body.ToString();

                        Console.WriteLine("Message received: " + messageText);

                        List<Process> runningProcesses = Process.GetProcesses().ToList();

                        foreach (var process in processes)
                        {
                            var existingProcess = runningProcesses.Find(p => p.ProcessName == process);
                            if (existingProcess != null)
                            {
                                int extraInfo = Convert.ToInt32(messageText);
                                _updateTimeStamp = DateTime.Now.ToString();
                                Console.WriteLine($"TimeStamp : {_updateTimeStamp}");

                                existingProcess.Kill();
                                existingProcess.WaitForExit();

                                Thread.Sleep(3000);

                                // Update는 제일 마지막에 실행
                                UpdateExitProcessEvent(_updateTimeStamp, extraInfo);
                                _updateTimeStamp = null;
                            }
                        }
                    }



                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                    // 예외 처리
                }
            }
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

#if false
        public void OverallProcessStatus()
        {
            try
            {
                _processArr = _dbManager.GetProcessInfo();
                Process[] processes = Process.GetProcesses();

                foreach (var proc in _processArr)
                {
                    var process = processes.FirstOrDefault(p => p.ProcessName.Contains(proc.Name));

                    if (process == null)
                    {
                        isAnyProcessDown = true;
                        break;
                    }
                    else isAnyProcessDown = false;

                    Console.WriteLine($"{proc.Name}, {isAnyProcessDown}");
                }

                //_alarmDal.OverallProcessStatus(isAnyProcessDown);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

#endif
    }

}
