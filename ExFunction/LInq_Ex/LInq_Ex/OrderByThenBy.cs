using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInq_Ex
{
    public class OrderByThenBy
    {
        static void Main(String[] args)
        {
            List<Person> listPerson = new List<Person>() {
                new Person{ Name = "Tom",  Age=30},
                new Person{ Name = "Tom",  Age=33},
                new Person{ Name = "Nick", Age=23},
                new Person{ Name = "Nick", Age=20},
                new Person{ Name = "Elsa", Age=15},
                new Person{ Name = "Elsa", Age=40},
                new Person{ Name = "Elsa", Age=28},
            };

            List<Person> MethodResult = listPerson
                .OrderBy(x => x.Name)
                .ThenByDescending(x => x.Age)
                .ToList();

            foreach (var v in MethodResult)
            {
                Console.WriteLine(v.ToString());
            }
        }
    }
}
