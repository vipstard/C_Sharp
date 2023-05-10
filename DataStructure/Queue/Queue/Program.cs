using System.Collections.Concurrent;

namespace Queue
{
	internal class Program
	{
		// 멀티  쓰레딩에서 Queue를 사용하기 위한 방법 1. lock 2. Queue.Synchronized
		// .net 4.0 부터는 멀티쓰레딩 환경에서 큐를 간편하게 쓸 수 있는 ConcurrnetQueue 가 있다.
		static void Main(string[] args)
		{
			var q = new ConcurrentQueue<int>();

			// 큐에 넣는 쓰레드
			Task tEnq = Task.Factory.StartNew(() =>
			{
				for (int i = 0; i < 100; i++)
				{
					q.Enqueue(i);
					Console.WriteLine("EnQueue");
					Thread.Sleep(100);
				}
			});

			// 큐에서 읽는 쓰레드
			Task tDeq = Task.Factory.StartNew(() =>
			{
				int n = 0;
				int result;
				while (n < 100)
				{
					if (q.TryDequeue(out result))
					{
						Console.WriteLine(result);
						n++;
					}

					Thread.Sleep(100);
				}
			});

			// 두 쓰레드가 끝날 때 까지 대기
			Task.WaitAll(tEnq, tDeq);
		}
	}
}