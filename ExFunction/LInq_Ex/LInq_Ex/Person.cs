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
        public int age { get; set; }

        public Person()
        {
            
        }
        public Person(string name, int age)
        {
            Name = name;
            this.age = age;
        }

        public override bool Equals(object? obj)
        {
            return this.Name == ((Person)obj).Name && this.age == ((Person)obj).age;
        }
    }
}
