using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInq_Ex
{
    internal class OfType__
    {
        static void Main(String[] args)
        {
            // Object 타입 List는 모든 타입의 값을 가질 수 있다.
            List<Object> allTypeList = new List<object>()
            {
                "C#", 222, true, "Java", 333, false, "Vue.js"
            };

            List<string> strList = allTypeList.OfType<string>().ToList();

            foreach (var val in strList)
            {
                Console.WriteLine(val);
            }
        }
    }
}
