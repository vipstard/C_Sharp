using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threaddd{

    //Parallel.Invoke() 메서드는 여러 작업들을 병렬로 처리하는 기능을 제공한다.
    //즉, 다수의 작업 내용을 Action delegate로 받아 들여 다중 쓰레드들로 동시에
    //병렬로 Task를 나눠서 실행하게 된다. Task 클래스와 다른 점은, 만약 1000개의 Action
    //델리게이트가 있을 때, Task 클래스는 보통 1000개의 쓰레드를 생성하여 실행하지만
    //(물론 사용자 다르게 지정할 수 있지만), Parallel.Invoke는 1000개를 일정한 집합으로 나눠 내부적으로
    //상대적으로 적은(적절한) 수의 쓰레드들에게 자동으로 할당해서 처리한다.

    public class ParallelProgramming2
    {
        static void Main(String[] args)
        {


        }
    }
}