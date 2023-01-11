using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threaddd
{
    public class ThreadPoolTest
    {
        //  쓰레드풀에서 사용가능한 작업 쓰레드를 할당 받아 사용하는 방식이 있는데,
        //  이는 다수의 쓰레드를 계속 만들어 사용하는 것보다 효율적이다.
        //  이 방식은 실행되는 메서드로부터 리턴 값을 돌려받을 필요가 없는 곳에 주로 사용된다.
        //  리턴값이 필요한 경우는 비동기 델리게이트(Asynchronous delegate)를 사용한다.
        static void Main(String[] args)
        {
            ThreadPool.QueueUserWorkItem(Calc);
            ThreadPool.QueueUserWorkItem(Calc, 10.0);
            ThreadPool.QueueUserWorkItem(Calc, 20.0);

            Console.ReadLine();
        }

        static void Calc(object radius)
        {
            if (radius == null) return;

            double r = (double)radius;
            double area = r * r * 3.14;
            Console.WriteLine("r={0}, area={1}", r, area);
        }
    }
}
