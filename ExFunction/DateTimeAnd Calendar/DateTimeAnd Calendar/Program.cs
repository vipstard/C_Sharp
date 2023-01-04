using System.Globalization;

namespace DateTimeAnd_Calendar
{
    public class Program
    {
        static void Main(string[] args)
        {
            int period = 50;
            DateTime today = DateTime.Now;
            DateTime result = today.AddDays(-30);

            String date = string.Format("{0:yyyy-MM-dd HH:mm:ss}", result);

            Console.WriteLine("today : {0:yyyy-MM-dd HH:mm:ss}", today);
            Console.WriteLine(date);
           
        }
    }
}