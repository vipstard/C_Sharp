using System;
using System.Linq.Expressions;

namespace ExpressionTreeViaLambda
{
    class MainApp
    {
        static void Main(string[] args)
        {
            Expression<Func<int, int, int>> expression = (a, b) => 1 * 2 + (a - b); // 식트리 만들기
            Func<int, int, int> func = expression.Compile();

            //x = 7, y = 8
            Console.WriteLine($"1*2+({7}-{8}) = {func(7, 8)}");

        }
    }
}
