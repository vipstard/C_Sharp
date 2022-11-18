using System.Diagnostics;

namespace PracticeCSharp
{
    public class ProcessClass
    {
        public static void Main(){

            string command = "sc query wnms service";
            Process myProcess = Process.Start("cmd.exe", command);

        Console.WriteLine($"{myProcess}");
        Console.WriteLine($"{myProcess.Responding}");
        }

    }
}
