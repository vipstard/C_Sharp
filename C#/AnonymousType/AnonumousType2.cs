using System;

namespace AnonymousType
{
     class AnonumousType2
    {
        static void Main(string[] args)
        {
            var a = new { Name = "박상현", Age = 123 };
            Console.WriteLine($"Name:{a.Name}, Age: {a.Age}");

            var b = new { Subject = "수학", Scores = new int[] { 90, 80, 70, 60 } };

            Console.Write($"Subject:{b.Subject}, Scores: ");
            foreach (var score in b.Scores)
                Console.WriteLine($"{score} ");

            Console.WriteLine();

        }
    }
}
