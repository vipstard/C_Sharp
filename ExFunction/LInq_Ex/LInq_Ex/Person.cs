﻿using System;
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

        public override string ToString()
        {
            return "Name : " +   Name +  ",  Age : " +  age;
        }

        public Person()
        {
            
        }
        public Person(string name, int age)
        {
            Name = name;
            this.age = age;
        }

    }
}