namespace NewFeatureCShapr10
{
    internal class EnhancedLamda
    {
        static void Main(string[] args)
        {
            // 기존 C# 9 에서는 Delegate 타입을 명시적으로 지정해줘야 했음.
            Func<string, int> stringConvertIntFunc = (string s) => int.Parse(s);
            int n1 = stringConvertIntFunc("101");
            Console.WriteLine(n1);

            // C# 10 에서는 var를 사용하여 컴파일러가 Delegate 타입을 유추 할 수 있도록 함.
            // 즉 람다식 유추 기능이 향상되었다.
            var stringConvertIntFunc2 = (string s) => int.Parse(s);
            var n2 = stringConvertIntFunc2("101");
            Console.WriteLine(n2);
        }
    }
}