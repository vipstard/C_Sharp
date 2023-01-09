namespace LInq_Ex
{
    public class DisTinct
    {
        // Linq 중복제거 
        static void Main(string[] args)
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

            // 익명타입 사용
          var queryResult = 
              (from A in list select A)
              .Select(A=>new {A.Name, A.age})
              .Distinct();

          foreach (var result in queryResult)
          {
              Console.WriteLine($"{result.Name} / {result.age}");
            }

          Console.WriteLine();

          var methodResult = list.Select(A=>new {A.Name, A.age}).Distinct();

          foreach (var result in methodResult)
          {
              Console.WriteLine($"{result.Name}  / {result.age}");
            }
          

        }
    }
}