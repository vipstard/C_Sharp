using System.Collections.Concurrent;

namespace ConcurrentQueue
{
    public class Program
    {
        // ConcurrentQueue 에는 Dequeue() 메서드가 없고 대신 TryDequeue() 메서드를 사용한다
        //또한 마찬가지로 ConcurrentQueue에서는 Peek() 메서드 대신 TryPeek() 메서드를 사용한다.

        static void Main(string[] args)
        {
            var queue = new ConcurrentQueue<int>();

            // Queue 에 데이터를 넣는 쓰레드
            Task tEnq = Task.Run(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    queue.Enqueue(i);
                     Task.Delay(100);
                }
            });

            // Queue에서 데이터를 읽는 쓰레드
            Task tDeq = Task.Run(() =>
            {
                int n = 0;
                int result;
                while (n<1000)
                {
                    if (queue.TryDequeue(out result))
                    {
                        Console.WriteLine(result);
                        n++;
                    }
                    Task.Delay(100);
                }
            });

            Task.WaitAll(tEnq, tDeq);
        }
    }
}