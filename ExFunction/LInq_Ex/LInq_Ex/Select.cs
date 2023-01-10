using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInq_Ex
{
    public class Select
    {

        static void Main(String[] args)
        {
            List<Person> list = new List<Person>()
            {
                new Person("John", 24),
                new Person("John", 24),
                new Person("Park", 56),
                new Person("Park", 56),
                new Person("Lily", 23),
                new Person("Lily", 23),
                new Person("Roanldo", 30),
                new Person("Roanldo", 30)
            };

            // Person -> PersonCopy 타입 변경
            List<PersonCopy> methodResult = list.Select(p=> new PersonCopy()
            {
                Name = p.Name.ToLower(),
                age = p.age * 10 // 프로퍼티 값 변경
            }).ToList();

            foreach (var result in methodResult)
            {
                Console.WriteLine(result.ToString());

            }
         
        }
        


    }
}
