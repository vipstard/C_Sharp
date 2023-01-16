using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInq_Ex
{
    public class AllAny
    {
        static void Main(String[] args)
        {
            // Quantifiresrs : 이 연산은 컬렉션과 같은 데이터 집합에서 모든 요소들이 특정
            // 조건을 만족하는지 확인할 수 있는 방법들을 제공한다. All(), Any(), Contains() 가 있다.

            List<int> intList = new List<int>()
            {
                5, 10, 15, 20, 25, 30
            };

            // All() : 모든요소가 조건을 만족하면 true 아니면 false
            // Any() : 하나라도 조건을 만족하면 true 아니면 false
            bool QueryResultAll = (from num in intList select num).All(x => x > 10);
            bool QueryResultAny = (from num in intList select num).Any(x => x > 10);
            bool MethodResultAll= intList.All(x => x > 10);
            bool MethodResultAny = intList.Any(x => x > 10);

            Console.WriteLine(QueryResultAll);
            Console.WriteLine(QueryResultAny);
            Console.WriteLine(MethodResultAll);
            Console.WriteLine(MethodResultAny);

        }
    }
}
