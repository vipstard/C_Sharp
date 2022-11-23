
namespace Process
{
    using System.Diagnostics;

    internal class Program
    {
        static void Main(string[] args)
        {
            string command = "sc query wnms service";

            //CMD 접근
            Process myProcess = Process.Start("cmd.exe", command); 
            
            Console.WriteLine(myProcess.Responding);

            if (myProcess.Responding) //Running True 상태
            {
                // 프로세스에 명령어 날리기
                // Ex) Process.Start("Path + FileName Or FileName", "Command");


            }

        }
    }
}