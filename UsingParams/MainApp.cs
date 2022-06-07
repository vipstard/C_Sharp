using System;

namespace UsingParams
{
    class MainApp
    {
       /* 가변 개수의 인수 (params)
        매개변수 개수가 유연하게 달라질 수 있는 경우에 적합하다.*/
        static int Sum(params int[] args)
        {
            Console.Write("Summing...");

            int sum = 0;
            
            for(int i =0; i<args.Length; i++)
            {
                if (i > 0)
                    Console.Write(", ");

                Console.Write(args[i]);

                sum += args[i];
            }

            Console.WriteLine();

            return sum;
        }
        static void Main(string[] args)
        {
            int sum = Sum(3, 4, 5, 6, 7, 8, 9, 10);

            Console.WriteLine($"Sum : {sum}");
        }
    }
}
