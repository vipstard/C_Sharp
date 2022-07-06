using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModuleA;
using ModuleB;

namespace ConsoleApp1
{
    class Program
    {
        private TestModuleB _module;

        public Program()
        {
            _module = new TestModuleB();
        }

        static void Main(string[] args)
        {

        }
            public void Test1()
            {
                _module.Test2();
            }
        }
    }
