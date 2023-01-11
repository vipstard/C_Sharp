using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threaddd
{
    public class ThreadClass2
    {
        static void Main(String[] args)
        {
            // parameter 없는 ThreadStart 사용
            Thread t1 = new Thread(new ThreadStart(Run));
            t1.Start();

            // Start() 파라미터로 rd전달
            Thread t2 = new Thread(new ParameterizedThreadStart(Calc));
            t2.Start(10.00);

            // ThreadStart 에서 파라미터 전달
            Thread t3 = new Thread(() => Sum(10, 20, 30));
            t3.Start();
        }

        static void Run()
        {
            Console.WriteLine("Run");
        }

        static void Calc(object rd)
        {
            double r = (double)rd;
            double area = r * r * 3.14;
            Console.WriteLine($"r={r} , area={area}");
        }

        static void Sum(int n1, int n2, int n3)
        {
            int sum = n1 + n2 + n3;
            Console.WriteLine(sum);
        }
    }
}
