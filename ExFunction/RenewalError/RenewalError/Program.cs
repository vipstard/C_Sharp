namespace RenewalError
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DbManager dbManager = new DbManager();

            while (true)
            {
                var list = dbManager.GetRenewalList();

                foreach (var mmsEvent in list)
                {
                    Console.WriteLine($"{mmsEvent.timestamp} : {mmsEvent.status}.{mmsEvent.extra_Info}");
                }

                dbManager.InsertErrorList(list);
                Console.WriteLine($"COUNT : {list.Count} \nWAIT.....\n");
                Thread.Sleep(30000);
            }
            
        }
    }
}