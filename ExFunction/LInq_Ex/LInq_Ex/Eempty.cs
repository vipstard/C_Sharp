using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInq_Ex
{
    public class Eempty
    {
        private static List<string> getInit()
        {
            return null;
        }
        static void Main(String[] args)
        {
            List<String> strList = getInit() ?? Enumerable.Empty<string>().ToList();
            Console.WriteLine(strList.Count);
        }
    }
}