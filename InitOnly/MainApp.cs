using System;

namespace InitOnly
{
    /* init 접근자는 set 접근자처럼 외부에서 프로퍼티를 변경할 수 있지만, 객체 초기화를 할 때만 프로퍼티 변경이 가능하다.
        초기화가 한차례 이루어진 후 변경되면안되는 데이터들에 사용한다. (ex 성적표, 범죄기록, 금융거래 기록) */

    class Transaction
    {
        public string From { get; init; }
        public string To { get; init; }
        public int Amount { get; init; }

        public override string ToString()
        {
            return $"{From,-10} -> {To,-10} : ${Amount}";
        }
        class MainApp
        {
            static void Main(string[] args)
            {
                Transaction tr1 = new Transaction { From = "Alice", To = "Bob", Amount = 100 };
                Transaction tr2 = new Transaction { From = "Bob", To = "Charlie", Amount = 50 };
                Transaction tr3 = new Transaction { From = "Charlie", To = "Alice", Amount = 50 };

                Console.WriteLine(tr1);
                Console.WriteLine(tr2);
                Console.WriteLine(tr3);
            }
        }
    }
}
