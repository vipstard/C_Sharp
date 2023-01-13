using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threaddd
{
    public class ParallelProgramming
    {
        static void Main(String[] args)
        {
            // 1. 순차적 실행
            // 한 쓰레드가 0~999 출력 처리
            //for (int i = 0; i < 1000; i++)
            //{
            //    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} : {i}");
            //}

            // 2. 병렬 처리
            // 다중 쓰레드가 병렬로 출력
            Parallel.For(0, 1000, (i) =>
            {
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} : {i}");
            });
        }
    }
}
