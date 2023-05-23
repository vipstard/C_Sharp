using System;
using System.Diagnostics;

namespace ProcessInfoCmd
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //int setting = 50;
            //string command = "C://nms4sa// " + setting.ToString();
            //MmsScan(command);
            int[] list = DefaultEthernetSwitchPortOrder();

            Console.WriteLine("\n\n===========Print==========");
            foreach (var i in list)
            {
	            Console.WriteLine(i);
            }


        }

        static private int[] DefaultEthernetSwitchPortOrder()
        {
	        int reverseStartNum = 0;
	        int[] list = new int[24];
	        int idx = 0;

			for (int i = 1; i <= 9; i += 4)
	        {
		        Console.WriteLine(i + "===>");
                list[idx++] = i;

		        for (int j = i + 1; j < i + 4; j++)
		        {
			        Console.WriteLine(j);
			        list[idx++] = j;
			        if (j == i + 3)
			        {
				        reverseStartNum = j + 12;
			        }
		        }

		        for (int k = reverseStartNum; k > reverseStartNum - 4; k--)
		        {
			        Console.WriteLine(k);
			        list[idx++] = k;
		        }
	        }
            return list;
		}
        //static private void MmsScan(string command)
        //{
        //    ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
        //    processInfo.RedirectStandardOutput = true;
        //    processInfo.UseShellExecute = false;
        //    processInfo.CreateNoWindow = true;

        //    Process process = new Process();
        //    process.StartInfo = processInfo;
        //    process.Start();

        //    string output = process.StandardOutput.ReadToEnd();

        //    process.WaitForExit();

        //    Debug.WriteLine(output);

        //}
    }
}