using System.Diagnostics;

namespace SoftwareInfo
{
    internal class Program
    {
        // Process 이름, 동작상태 , CPU 점유율(%), 메모리 사용(K) , 버전 얻기
        static void Main(string[] args)
        {
            String[] strArr = 
            {"nms4sa_service", "EliveCap", "EMMS4SARunTime", "EmmsParse", 
                "goose_mon", "mms_mon", "SNMP_MON"};
            List<double> cpuLIst = new List<double>();
            
            

            while(true){
                Process[] processs = Process.GetProcesses();


                //// 프로세스 동작여부
                //Console.WriteLine("PID\t프로세스이름\t동작상태(K)");
                //Console.WriteLine("=========================================================");
                //foreach (var p in processs)
                //{
                //    foreach (var processName in strArr)
                //    {
                //        if (processName == p.ProcessName)
                //        {
                //            Console.WriteLine(p.Id + "\t" + p.ProcessName +(p.ProcessName=="mms_mon"?"\t":"")+ "\t" + (p.Responding?"정상":"이상"));


                //        }

                //    }
                //}
                //Thread.Sleep(3000);
                //Console.WriteLine();


                //프로세스 CPU 점유율 구하기
                Console.WriteLine("프로세스이름\t CPU사용량(%)");
                Console.WriteLine("=========================================================");
                Parallel.For(0, 7, (i) =>
                {
                    var cpuCounter = new PerformanceCounter("Process", "% Processor Time", strArr[i]);
                    if (cpuCounter != null)
                    {
                        cpuCounter.NextValue();
                        //Thread.Sleep(1500);
                        double value = Math.Round(cpuCounter.NextValue() / 10, 1);
                        Console.WriteLine(strArr[i] + "\t" + value);
                    }
                });
                Console.WriteLine();


                //// 프로세스 메모리 사용량 구하기
                //Console.WriteLine();
                //Console.WriteLine("PID\t프로세스이름\t 메모리사용량(K)");
                //Console.WriteLine("=========================================================");
                //foreach (Process p in processs)
                //{
                //    foreach (var processName in strArr)
                //    {
                //        if (processName == p.ProcessName)
                //        {
                //            PerformanceCounter ram = new PerformanceCounter("Process", "Working Set - Private", processName);

                //            Debug.WriteLine(p.Id + " " + p.ProcessName + " " + p.MainWindowTitle);
                //            Console.WriteLine(p.Id + "\t" + p.ProcessName  + "\t" +
                //                              string.Format("{0:##,##}", ram.RawValue / 1024) + "K");
                //        }
                //    }

                //}
                //Thread.Sleep(3000);
                //Console.WriteLine("\n");

            }

            // 전체 메모리 사용량 구할때 사용
        //    static void GetTotalUsedMemory(/*double memsize_MB*/)
        //{
        //    ManagementClass cls = new ManagementClass("Win32_OperatingSystem");
        //    ManagementObjectCollection instances = cls.GetInstances();

        //    foreach (ManagementObject info in instances)
        //    {
        //        double total_physical_memeory = double.Parse(info["TotalVisibleMemorySize"].ToString());
        //        double free_physical_memeory = double.Parse(info["FreePhysicalMemory"].ToString());
        //        double remain_physical_memory = total_physical_memeory - free_physical_memeory;

        //        Console.WriteLine("Memory Information ================================");
        //        Console.WriteLine("Total Physical Memory :{0:#.###} GB", total_physical_memeory/ (1024 * 1024));
        //        Console.WriteLine("Total Physical Memory :{0:#,###} MB", total_physical_memeory/1024);
        //        Console.WriteLine("Total Physical Memory :{0:#,###} KB", total_physical_memeory);

        //        Console.WriteLine("Free Physical Memory :{0:#,###} GB", free_physical_memeory/(1024 * 1024));
        //        Console.WriteLine("Free Physical Memory :{0:#,###} MB", free_physical_memeory/1024);
        //        Console.WriteLine("Free Physical Memory :{0:#,###} KB", free_physical_memeory);

        //        Console.WriteLine("Remain Physical Memory : {0:0.00} GB", remain_physical_memory / (1024 * 1024));
        //        Console.WriteLine("Remain Physical Memory : {0:#,###} MB", remain_physical_memory / 1024);
        //        Console.WriteLine("Remain Physical Memory : {0:#,###} KB", remain_physical_memory);

        //        Console.WriteLine("Memory Usage Percent = {0} %", 100 *(int) remain_physical_memory / (int)total_physical_memeory);

        //        //Console.WriteLine("DWConfig Usage Percent = {0:0.00} %", 100 * (memsize_MB / remain_physical_memory_MB) );
                
               
        //        Console.WriteLine();
        //    }

        }
        }
    }
