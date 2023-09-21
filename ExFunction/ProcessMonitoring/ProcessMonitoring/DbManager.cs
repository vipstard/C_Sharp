using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using proc_mon.Model;
using ProcessMonitoring.Model;

namespace proc_mon
{
    public class DbManager
    {
        private string _procConn = @"Data Source = C:\nms4sa\database\proc.db";

        public List<ProcessInfo> GetProcessInfo()
        {
            List<ProcessInfo> processList = new List<ProcessInfo>();
            using (var conn = new SQLiteConnection(_procConn))
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
            using (var conn = new SQLiteConnection(_procConn))
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

        public void UpdateExitProcessEvent(string timeStamp, int extraInfo)
        {
	        using (var conn = new SQLiteConnection(_procConn))
	        {
		        conn.Open();

			    string updateQuery = "UPDATE proc_event SET extra_Info = @extraInfo WHERE timestamp = @timestamp AND status = @status";

				using (var cmd = new SQLiteCommand(updateQuery, conn))
			    {
				    cmd.Parameters.AddWithValue("@timestamp", timeStamp);
				    cmd.Parameters.AddWithValue("@status", 0);
				    cmd.Parameters.AddWithValue("@extraInfo", extraInfo);
				    cmd.ExecuteNonQuery();
			    }

				Console.WriteLine("Update Log ");
	        } //using conn
        } //using cmd

		public void InsertLog(ProcessEvent processEvent)
        {
            using (var conn = new SQLiteConnection(_procConn))
            {
                conn.Open();

                string insertQuery = "INSERT INTO proc_event( timestamp, name, status, extra_info) VALUES(@TimeStamp, @Name, @Status, @ExtraInfo)";

                using (var cmd = new SQLiteCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@TimeStamp", processEvent.TimeStamp);
                    cmd.Parameters.AddWithValue("@Name", processEvent.Name);
                    cmd.Parameters.AddWithValue("@Status", processEvent.Status);
                    cmd.Parameters.AddWithValue("@ExtraInfo", processEvent.ExtraInfo);

                    cmd.ExecuteNonQuery();
                }
                Console.WriteLine("Insert Log");
			} //using conn
        } //using cmd

    }
}
