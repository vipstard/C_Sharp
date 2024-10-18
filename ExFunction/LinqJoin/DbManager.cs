using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;

namespace JoinFunction
{
    public class DbManager
    {
        private string dbTNS = string.Empty;
        private LinqJoin join = new LinqJoin();

        OracleConnection DbConnection;
        OracleCommand DBcommand;
        OracleDataAdapter DBadapter;

        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();

        public DbManager()
        {
            SetDbTns();
        }

        private void SetDbTns()
        {
            try
            {
                dbTNS = "";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Oracle DB SELECT with a complex SQL query and parameters
        /// </summary>
        public DataTable Oracle_DB_Select()
        {
            using (DbConnection = new OracleConnection(dbTNS))
            {
                try
                {
                    DbConnection.Open();

                    DBadapter = new OracleDataAdapter();

                    DataTable mergeDt = new DataTable();
                    string SQL1 = ReturnSQL1();
                    string SQL2 = ReturnSQL2();

                    DBcommand = new OracleCommand(SQL1, DbConnection);
                    DBcommand = new OracleCommand(SQL2, DbConnection);

                    // Task를 통해 비동기적으로 SQL1과 SQL2를 병렬로 실행
                    Task<DataTable> task1 = Task.Run(() => ExecuteQuery(SQL1, DbConnection));
                    Task<DataTable> task2 = Task.Run(() => ExecuteQuery(SQL2, DbConnection));
                    Task.WaitAll(task1, task2);


                    mergeDt = join.MergeDataTable(task1.Result, task2.Result);

                    return mergeDt;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
                finally
                {
                    DbConnection.Close();
                }
            }
        }

        // SQL을 실행하고 결과를 DataTable로 반환하는 메서드
        private DataTable ExecuteQuery(string sql, OracleConnection connection)
        {
            Stopwatch st = new Stopwatch();

            st.Start();

            OracleCommand command = new OracleCommand(sql, connection);
            OracleDataAdapter adapter = new OracleDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            st.Stop();
            Console.WriteLine("SQL실행시간 :" + st.Elapsed);
            return dt;
        }


        private string ReturnSQL1()
        {
            string SQL = @"";

            return SQL;
        }

        private string ReturnSQL2()
        {
            String SQL = @"";
            return SQL;
        }
    }
}
