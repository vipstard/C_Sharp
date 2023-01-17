using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInq_Ex
{
    public class GroupBy
    {
        static void Main(String[] args)
        {
            var GroupByMethodResult = Person.GetPersons()
                .GroupBy(p => p.Grade)
                .Select(p => new
                {
                    Grade = p.Key,
                    Count = p.Count()
                });

            Console.WriteLine("메서드 구문");

            foreach (var group in GroupByMethodResult)
            {
                Console.WriteLine(group.Grade + " 학년의 인원 수 : " + group.Count);
            }
        }
    }
}
