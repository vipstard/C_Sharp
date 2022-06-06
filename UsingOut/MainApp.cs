using System;

namespace UsingOut
{
    /* out 출력전용 매개변수 사용해보기  */
    /* 메소드 호출전 미리 선언할 필요 X, 호출 할 때 즉석으로 선언해도 됌 */
    class MainApp
    {
        static void Divide(int a, int b, out int quotient, out int remainder)
        {
            quotient = a / b;
            remainder = a% b;
        }

        static void Main(string[] args)
        {
            int a = 20;
            int b = 3;
            // int c ;
            // int d ; (미리 선언 X)

            Divide(a, b, out int c, out int d);

            Console.WriteLine($"a:{a}, b:{b}:, a/b:{c}, a%b:{d}");
        }
    }
}
