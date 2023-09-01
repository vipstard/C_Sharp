using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using proc_mon.Model;

namespace proc_mon
{
    public class DbManager
    {
        private string _alarmConn = @"Data Source = C:\nms4sa\database\proc.db";

        public List<ProcessInfo> GetProcessInfo()
        {
            List<ProcessInfo> processList = new List<ProcessInfo>();
            using (var conn = new SQLiteConnection(_alarmConn))
            {
                conn.Open();

                string query = "SELECT * FROM proc_info";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProcessInfo processInfo = new ProcessInfo();
                            processInfo.Id = Convert.ToInt32(reader["id"].ToString());
                            processInfo.Name = reader["Name"].ToString();
                            processInfo.Folder = reader["Folder"].ToString();
                            processInfo.Desc = reader["Desc"].ToString();
                            processInfo.Param = reader["Param"].ToString();
                            processInfo.Pid = Convert.ToInt32(reader["pid"].ToString());
                            processInfo.Count = Convert.ToInt32(reader["count"].ToString());
                            processInfo.Status = Convert.ToInt32(reader["status"].ToString());
                            processInfo.Type = Convert.ToInt32(reader["type"].ToString());

                            processList.Add(processInfo);

                        } //while
                    } //using reader
                } //using cmd
            } //using conn

            return processList;
        }

        public void UpdateProcessInfo(List<ProcessInfo> processInfos)
        {
            using (var conn = new SQLiteConnection(_alarmConn))
            {
                conn.Open();

                foreach (ProcessInfo processInfo in processInfos)
                {
                    string updateQuery = "UPDATE proc_info SET Pid = @Pid, Status = @Status WHERE Id = @Id";

                    using (var cmd = new SQLiteCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Pid", processInfo.Pid);
                        cmd.Parameters.AddWithValue("@Status", processInfo.Status);
                        cmd.Parameters.AddWithValue("@Id", processInfo.Id);

                        cmd.ExecuteNonQuery();
                    }
                }
            } //using conn
        } //using cmd

    }
}
