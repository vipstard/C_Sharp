namespace ActionTest
{
    class MainApp
    {
        static void Main(string[] args)
        {
            // 매개변수가 없는 경우
            Action act1 = () => Console.WriteLine("Action()");
            act1();

            // 매개변수가 1개 있는 경우
            int result = 0;
            Action<int> act2 = (x) => result = x * x;

            act2(3);
            Console.WriteLine($"result : {result}");

            // 매개변수가 2개 있는 경우 안에서 출력문을 작성하여 함수 호출하면 
            // 출력문까지 출력된다.
            Action<double, double> act3 = (x, y) =>
            {
                double pi = x / y;
                Console.WriteLine($"Action<T1, T2>({x}, {y}) : {pi}");
            };
            act3(22.0, 7.0);
        }
    }
}