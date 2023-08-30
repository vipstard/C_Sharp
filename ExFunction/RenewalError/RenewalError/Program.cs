using System.Diagnostics;
using MDAS.Model.goose;
using MDAS.Model.mms;

namespace RenewalError
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //string exePath = @"C:\nms4sa\error_mon.exe";

            //Process.Start(exePath);

            //Thread.Sleep(10000);

            foreach (var process in Process.GetProcessesByName("error_mon"))
            {
                process.Kill();
                process.WaitForExit();
            }

            //DbManager dbManager = new DbManager();

            //while (true)
            //{
            //    Console.WriteLine($"현재시간 : {DateTime.Now}");
            //    List<MmsEvent> mmsList = dbManager.GetRenewalMmsList();
            //    List<GooseEvent> gooseList = dbManager.GetRenewalGooseList();



            //    foreach (var mmsEvent in mmsList)
            //    {
            //        Console.WriteLine($"{mmsEvent.timestamp} : {mmsEvent.status}.{mmsEvent.extra_Info}");
            //    }

            //    dbManager.InsertMmsErrorList(mmsList);
            //    dbManager.InsertGooseErrorList(gooseList);

            //    Console.WriteLine($"MMS COUNT : {mmsList.Count} GOOSE COUNT : {gooseList.Count}");
            //    Console.WriteLine($" WAIT.....\n");

            //    Thread.Sleep(20000);
            //}

        }
    }
}