using System;

namespace PropertiesInInterface
{ /* 프로퍼티나 인덱서를 가진 인터페이스를 상속하는 클래스가 "반드시" 해당 프로퍼티와 인덱서를 구현해야 한다. */

    interface INameValue
    {
        string Name
        {
            get;
            set;
        }

        string Value
        {
            get;
            set;
        }
    }

    class NamedValue : INameValue
    {
        public string Name
        {
            get;
            set;
        }
        public string Value
        {
            get;
            set;
        }
    }
    class MainApp
    {
        static void Main(string[] args)
        {
            NamedValue name = new NamedValue()
            { Name = "이름", Value = "박상현" };

            NamedValue height = new NamedValue()
            { Name = "키", Value = "177cm" };

            NamedValue weight = new NamedValue()
            {
                Name = "몸무게",
                Value = "90Kg"
            };

            Console.WriteLine($"{name.Name} : {name.Value}");
            Console.WriteLine($"{height.Name} : {height.Value}");
            Console.WriteLine($"{weight.Name}: {weight.Value}");


        }
    }
}
