using System.Data;
using System.Diagnostics;

namespace JoinFunction
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch st = new Stopwatch();
            LinqJoin join = new LinqJoin();
            DbManager dbManager = new DbManager();
            DataTable dt = new DataTable();

            st.Start();
            dt = dbManager.Oracle_DB_Select();

            foreach (DataRow r in dt.Rows)
            {
                System.Console.WriteLine(r["LOT_ID"] + "----" + r["PROD_TYPE"]);
            }
            st.Stop();
            System.Console.WriteLine(st.Elapsed);
        }
    }
}
