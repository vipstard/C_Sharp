using MDAS.Model.goose;
using MDAS.Model.mms;

namespace RenewalError
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DbManager dbManager = new DbManager();

            while (true)
            {
                List<MmsEvent> mmsList = dbManager.GetRenewalMmsList();
                List<GooseEvent> gooseList = dbManager.GetRenewalGooseList();

                foreach (var mmsEvent in mmsList)
                {
                    Console.WriteLine($"{mmsEvent.timestamp} : {mmsEvent.status}.{mmsEvent.extra_Info}");
                }

                dbManager.InsertMmsErrorList(mmsList);
                dbManager.InsertGooseErrorList(gooseList);

                Console.WriteLine($"MMS COUNT : {mmsList.Count} GOOSE COUNT : {gooseList.Count}");
                Console.WriteLine($" WAIT.....\n");

                Thread.Sleep(30000);
            }
            
        }
    }
}