using System.Collections.Concurrent;

namespace ConcurrentDictionary
{
    public class Program
    {
        //  ConcurrentDictionary 에 Key를 1부터 100까지 계속 집어 넣을 때,
        // 동시에 다른 쓰레드에서는 계속 그 해시테이블에서 Key가 1부터 100까지인
        // 데이타를 빼내 (순차적으로) 읽어 오는 작업을 하는 샘플 코드이다.
        static void Main(string[] args)
        {
            var dic = new ConcurrentDictionary<int, String>();

            Task t1 = Task.Run(() =>
            {
                int key = 1;
                while (key <= 100)
                {
                    if (dic.TryAdd(key, "D" + key))
                    {
                        key++;
                    }

                    Task.Delay(100);
                }
            });

            Task t2 = Task.Run(() =>
            {
                int key = 1;
                string val;

                while (key <= 100)
                {
                    if (dic.TryGetValue(key, out val))
                    {
                        Console.WriteLine($"{key} , {val}");
                        key++;
                    }

                    Task.Delay(100);
                }
            });

            Task.WaitAll(t1, t2);
        }
    }
}

