using System.Globalization;

namespace DateTime
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 2023-03-30 18:52:54 를 2023-03-30 오후 6:52;54로 바꾸고싶다.

            String timeStamp1 = "2023-03-30 18:52:54.745";
            System.DateTime time = System.DateTime.Parse(timeStamp1);
            string outputTime1 = time.ToString("yyyy-MM-dd tt h:mm:ss");
            string outputTime2 = time.ToString("yyyy-MM-dd HH:mm:ss"); // 24시간 형식으로 포맷

            Console.WriteLine(outputTime1);
            Console.WriteLine(outputTime2);



        }
    }
}