using System;
using System.Diagnostics;

namespace ProcessInfoCmd
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int setting = 50;
            string command = "C://nms4sa// " + setting.ToString();
            MmsScan(command);
        }

        static private void MmsScan(string command)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.RedirectStandardOutput = true;
            processInfo.UseShellExecute = false;
            processInfo.CreateNoWindow = true;

            Process process = new Process();
            process.StartInfo = processInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            Debug.WriteLine(output);

        }
    }
}