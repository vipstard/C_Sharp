using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threaddd
{
    public class Semaphoree
    {
        

        // 10개 쓰레드를 실행 처음에 5개만 실행되고 하나씩 해제되면서 나머지가 실행된다.
        static void Main(String[] args)
        {
         MyClass c = new MyClass();

            for (int i = 1; i <= 100; i++)
            {
                new Thread(c.Run).Start(i);
            }
        }
    }

    class MyClass
    {
        private Semaphore sem;

        public MyClass()
        {
            // 5개의 쓰레드만 허용
            sem = new Semaphore(5, 5);
        }

        public void Run(object seq)
        {
            // 쓰레드가 가진 데이타 (일련번호)
            Console.WriteLine(seq);

            // 최대 5개 쓰레드만 아래 문장 실행
            sem.WaitOne();

            Console.WriteLine("Running#" + seq);
            Thread.Sleep(500);

            // Semaphore 1개 해제
            // 이후 다음 쓰레드 WaitOne()에서 진입 가능
            sem.Release();
        }
    }
}
