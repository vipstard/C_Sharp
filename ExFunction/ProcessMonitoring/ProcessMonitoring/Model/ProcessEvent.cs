using System;

namespace ProcessMonitoring.Model
{
    public class ProcessEvent
    {
        public string TimeStamp { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public int  ExtraInfo { get; set; }

        public ProcessEvent(string name, int status,  EventHandle.ExitCode exitCode)
        {
            Name = name;
            Status = status;
            ExtraInfo = Convert.ToInt32(exitCode);
        }
    }

}
