using System;

namespace TypeCasting
{
    /* is , as 실습 */
    class Mammal
    {
        public void Nurse()
        {
            Console.WriteLine("Nurse()");
        }
    }

    class Dog : Mammal
    {
        public void Bark()
        {
            Console.WriteLine("Bark()");
        }
    }

    class Cat : Mammal
    {
        public void Meow()
        {
            Console.WriteLine("Meow()");
        }
    }
    class MainApp
    {
        static void Main(string[] args)
        {
            Mammal mammal = new Dog();
            Dog dog;

            if(mammal is Dog) // is : 객체가 해당 형식에 해당하는지 검사하여 그 결과를 bool 값으로 반환한다.
            {
                dog = (Dog)mammal;
                dog.Bark();
            }

            Mammal mammal2 = new Cat();

            Cat cat = mammal2 as Cat;
            if (cat != null)
                cat.Meow();

                Cat cat2 = mammal as Cat; 
                if (cat2 != null)
                    cat2.Meow();
                else
                    Console.WriteLine("cat2 is not a Cat");
            
        }
    }
}
