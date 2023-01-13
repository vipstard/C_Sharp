using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threaddd
{
    internal class AsynchronousDelegate
    {
        static void Main(String[] args)
        {
            // Func의 처음 2개 int => 입력 ,마지막 int  => 출력
            Func<int, int, int> work = GetArea;
             
            IAsyncResult asyncResult = work.BeginInvoke(10, 20, null, null);

            Console.WriteLine("Do Something in Main Thread");

            int result = work.EndInvoke(asyncResult);
            Console.WriteLine(result);
        }


        static int GetArea(int height, int width)
        {
            int area = height * width;
            return area;
        }
    }
}
