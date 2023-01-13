using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threaddd
{
    //Task 클래스와 이의 Generic형태인 Task<T>클래스는 .NET 4.0에 도입된 새로운 클래스들로서
    //쓰레드풀로부터 쓰레드를 가져와 비동기 작업을 실행한다. Task 관련 클래스들과 Parallel 클래스들을
    //합쳐 Task Parallel Library(TPL)이라 부르는데, 이들은 기본적으로 다중 CPU 병렬 처리를 염두에 두고 만들었다.
    //Task 클래스는 .NET 4.0 이전 버전의 ThreadPool.QueueUserWorkItem()와 같은 기능을 제공하지만,
    //보다 빠르고 유연한 기능을 갖추고 있다. Task클래스 사용을 위해 흔히 사용되는 방법은 Task.Factory.StartNew()를
    //사용하여 실행하고자 하는 메서드에 대한 델리케이트를 지정하는 것이다. 이 StartNew()는 쓰레드를 생성과
    //동시에 실행하는 방식이고, 만약 시작을 하지 않고 Task 객체를 만들기 위해서는 Task() 생성자를 사용하여
    //메서드 델리게이트를 지정한다.

    public class Task_
    {
        // Task 실행방법 3가지
        // Start : 이미 생성된 Task를 실행하는 메서드
        // Factory.StartNew : Task를 생성하면서 동시에 시작하는 메서드
        // Run : Task에 있는 여러 옵션들을 디폴트로 하고 간단하게 실행하도록 하는 Wrapper 메서드

        static void Main(String[] args)
        {
            // Task 생성자에 Run을 지정 Task 객체 생성
            Task t1 = new Task(new Action(Run));

            // 람다식 사용해서 Task 객체 생성
            Task t2 = new Task(() =>
            {
                Console.WriteLine("Long query");
            });

            // Task 쓰레드 시작
            t1.Start();
            t2.Start();

            // Task가 끝날 때 까지 대기
            Task.WaitAll(t1, t2);

        }

        static void Run()
        {
            Console.WriteLine("Long Running Method");
        }
    }
}
