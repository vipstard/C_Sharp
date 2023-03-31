using System.Text.RegularExpressions;

namespace ValueOfSplit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //String value = "SelectWithValue: request: 333_C333CTRL/qwe1234$aa$ddos$vsvs: zf_val=[1]"; 
            //List<string> list = new List<string>();
            //list = value.Split(":").ToList();
            //Console.WriteLine();

            //foreach (var s in list)
            //{
            //    Console.WriteLine(s);
            //}



            // 정규식을 사용해서 숫자 만 분리하는방법
            string regexValue = "Object_value_invalid";

            string result = Regex.Match(regexValue, @"\d+").Value;

            Console.WriteLine(result);

        }
    }
}