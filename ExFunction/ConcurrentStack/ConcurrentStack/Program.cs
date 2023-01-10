using System.Collections.Concurrent;

namespace ConcurrentStack
{
    public class Program
    {
        static void Main(string[] args)
        {
            var s = new ConcurrentStack<int>();

            Task tPush = Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    s.Push(i);
                   Thread.Sleep(100);

                }
            });

            Task tPop = Task.Run(() =>
            {
                int n = 0;
                int result;

                while (n<100)
                {
                    if (s.TryPop(out result))
                    {
                        Console.WriteLine(result);
                        n++;
                    }

                    Thread.Sleep(150);

                }
            });
            Task.WaitAll(tPush, tPop);
        }
    }
}