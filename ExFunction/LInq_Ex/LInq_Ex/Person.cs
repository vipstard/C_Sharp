using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInq_Ex
{
    public class Person
    {
        public String Name { get; set; }
        public int Age { get; set; }
        public int Grade { get; set; }

        public override string ToString()
        {
            return "Name : " + Name + ",  Age : " + Age + ", Grade : " + Grade;
        }

        public Person()
        {
        }

        public Person(string name, int Age)
        {
            Name = name;
            this.Age = Age;
        }

        public static List<Person> GetPersons()
        {
            return new List<Person>()
            {
                new Person { Name = "1", Age = 20, Grade = 1 },
                new Person { Name = "1", Age = 21, Grade = 1 },
                new Person { Name = "1", Age = 22, Grade = 1 },
                new Person { Name = "1", Age = 23, Grade = 2 },
                new Person { Name = "1", Age = 24, Grade = 2 },
                new Person { Name = "1", Age = 25, Grade = 1 },
                new Person { Name = "1", Age = 26, Grade = 3 },
                new Person { Name = "1", Age = 27, Grade = 3 },
                new Person { Name = "1", Age = 28, Grade = 3 },
                new Person { Name = "1", Age = 29, Grade = 3 },
                new Person { Name = "1", Age = 30, Grade = 4 }
            };
        }
    }
}
