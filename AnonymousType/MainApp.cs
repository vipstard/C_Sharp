using System;

namespace AnonymousType
{ /* 무명형식은 선언과 동시에 인스턴스 할당을 한다. 
   * 인스턴스를 만들고 다시는 사용하지
    않을때 무명형식이 요긴하다. 
    (프로퍼티에 할당된 값 변경 불가능, 읽기만 가능하다는 이야기이다. ) 
    참고로 LINQ와 함께 사용하면 아주 요긴하다. */
    class MainApp
    {
        static void Main(string[] args)
        {
            var a = new { Name = "박상현", Age = 123 };
            Console.WriteLine($"Name:{a.Name}, Age:{a.Age}");

            var b = new { Subject = "수학", Scores = new int[] { 90, 80, 70, 60 } };

            Console.Write($"Subject :{b.Subject}, Score: ");
            foreach (var score in b.Scores)
                Console.Write($"{score} ");

            Console.WriteLine();
        }
    }
}
