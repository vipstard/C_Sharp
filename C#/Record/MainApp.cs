using System;

namespace Record
{

/*    상태를 변경할 수 없다는 특성 때문에 불변 객체에서는 데이터 복사와 비교가 많이 이뤄진다.
        레코드는 불변객체에서 이뤄지는 이 두 가지 연산을 편리하게 수행할 수 있도록 C#9.0에서 도입된 형식이다. */
record RTransaction
    {
        public string From { get; init; }
        public string To { get; init; }
        public int Amount { get; init; }

        public override string ToString()
        {
            return $"{From,-10} -> {To,-10}  : ${Amount}";
        }
    }
    class MainApp
    {
        static void Main(string[] args)
        {
            RTransaction tr1 = new RTransaction
            {
                From = "Alice",
                To = "Bob",
                Amount = 100
            };

            RTransaction tr2 = new RTransaction
            {
                From = "Alice",
                To = "Charlie",
                Amount = 100
            };

            Console.WriteLine(tr1);
            Console.WriteLine(tr2);

        }
    }
}
