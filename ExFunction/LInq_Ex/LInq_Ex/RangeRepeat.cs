using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInq_Ex
{
    public class RangeRepeat
    {
        static void Main(String[] args)
        {
            List<int> intList = Enumerable.Range(1, 5).ToList();
            List<int> intList2 = Enumerable.Range(1, 50).Where(n=>n%10==0) .ToList(); // 10의 배수 생성

            foreach (var i in intList2)
            {
                Console.WriteLine(i);
            }

            // Repeat : 특정데이터 특정횟수만큼 생성
            List<String> strList = Enumerable.Repeat("Hungry", 10).ToList();

            foreach (var VARIABLE in strList)
            {
                Console.WriteLine(VARIABLE);
            }
        }
    }
}
