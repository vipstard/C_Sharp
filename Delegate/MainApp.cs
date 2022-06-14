using System;

namespace Delegate
{

    /* 대리자 선언. 대리자는 인스턴스 메소드도 참조할 수 있고, 정적 메소드도 참조할 수 있다. */
        delegate int MyDelegate(int a, int b); 

        class Calculator
        {
            public int Plus(int a, int b)
            {
                return a + b;
            }

            public static int Minus(int a, int b)
            {
                return a - b;
            }
        }

    class MainApp
    {
        static void Main(string[] args)
        {
            Calculator Calc = new Calculator();
            MyDelegate Callback;

            /* 메소드를 호출하듯 대리자를 사용하면, 참조하고있는 메소드가 실행된다. */
            Callback = new MyDelegate(Calc.Plus);
            Console.WriteLine(Callback(3, 4));

            Callback = new MyDelegate(Calculator.Minus);
            Console.WriteLine(Callback(7, 5));
        }
    }
}
