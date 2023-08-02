namespace AnyAllExist
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AnyRun();
            //AllRun();
            //ExistRun();
        }

        /// <summary>
        /// 조건에 해당하는 값이 1개라도 존재한다면 true
        /// </summary>
        public static void AnyRun()
        {
            var arr1 = new int[] { 1, 2, 3, 4, 5 };
            var arr2 = new int[] { 4, 5, 6, 7, 8};

            bool result1 = arr1.Any(l => l == 3);
            Console.WriteLine($"단일 값 비교 : {result1}" );

            bool result2 = arr1.Any(l => arr2.Contains(l));
            Console.WriteLine($"다중 값 비교 : {result1}" );

            // 중복된 값을 찾는방법
            var list = arr2.Where(m => arr1.Any(a1 => a1 == m)).ToList();
            Console.WriteLine($"중복 갯수 : { list.Count }");

            foreach (var i in list)
            {
                Console.Write(i + " ");
            }

            Console.WriteLine("\n");
        }

        /// <summary>
        /// 전체 값이 조건에  전부 해당한다면 true
        /// </summary>
        public static void AllRun()
        {
            var arr1 = new List<int> {1, 2, 3, 4, 5};
            var arr2 = new List<int> {4, 5};
            
            bool result1 = arr1.All(l => l > 3);
            Console.WriteLine($"모두 3 이상의 숫자인가? : {result1}");

            bool result2 = arr2.All(l => arr1.Contains(l));
            Console.WriteLine($"arr2가 arr1의 부분집합인가? : {result2}");

            Console.WriteLine("\n");
        }

        /// <summary>
        ///  Exists() 값이 존재한다면 true
        /// </summary>
        public static void ExistRun()
        {
            var arr1 = new List<string> { "A", "B", ""};

            bool result1 = arr1.Exists(a => string.IsNullOrWhiteSpace(a));
            Console.WriteLine($"공백이나 null 이 포함되어있나? :  {result1}");

            bool result2 = arr1.Exists(a => a.Length > 4);
            Console.WriteLine($"arr1의 크기는 4보다 큰가? : {result2}");
        }
    }
}