global using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;


namespace Excel_Get
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Excel.Application excel = new Excel.Application();
            Excel.Workbook workbook = excel.Workbooks.Open(@"C:\nms4sa\database\iet\");
        }
    }
}

