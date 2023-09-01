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
        private string _alarmConn = @"Data Source = C:\nms4sa\database\nms_alarm.db";

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

      
        //public void InsertMmsErrorList(List<MmsEvent> list)
        //{
        //    using (var conn = new SQLiteConnection(_mrConn))
        //    {
        //        conn.Open();

        //        // mmsError 테이블에 데이터 삽입
        //        string insertQuery =
        //            "INSERT INTO mms_Error (timestamp, src_name, src_ip, dst_name, dst_ip, status, extra_Info, address, value, packet, packet_len)" +
        //            "VALUES (@timestamp, @src_name, @src_ip, @dst_name, @dst_ip, @status, @extra_Info, @address, @value, @packet, @packet_len)";

        //        using (var cmd = new SQLiteCommand(insertQuery, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@timestamp", "");
        //            cmd.Parameters.AddWithValue("@src_name", "");
        //            cmd.Parameters.AddWithValue("@src_ip", "");
        //            cmd.Parameters.AddWithValue("@dst_name", "");
        //            cmd.Parameters.AddWithValue("@dst_ip", "");
        //            cmd.Parameters.AddWithValue("@status", 0);
        //            cmd.Parameters.AddWithValue("@extra_Info", 0);
        //            cmd.Parameters.AddWithValue("@address", "");
        //            cmd.Parameters.AddWithValue("@value", "");
        //            cmd.Parameters.AddWithValue("@packet", "");
        //            cmd.Parameters.AddWithValue("@packet_len", 0);

        //            foreach (MmsEvent mmsEvent in list)
        //            {
        //                // mmsEvent에서 값을 가져와서 파라미터 설정
        //                cmd.Parameters["@timestamp"].Value = mmsEvent.timestamp;
        //                cmd.Parameters["@src_name"].Value = mmsEvent.src_name;
        //                cmd.Parameters["@src_ip"].Value = mmsEvent.src_ip;
        //                cmd.Parameters["@dst_name"].Value = mmsEvent.dst_name;
        //                cmd.Parameters["@dst_ip"].Value = mmsEvent.dst_ip;
        //                cmd.Parameters["@status"].Value = mmsEvent.status;
        //                cmd.Parameters["@extra_Info"].Value = mmsEvent.extra_Info;
        //                cmd.Parameters["@address"].Value = mmsEvent.address;
        //                cmd.Parameters["@value"].Value = mmsEvent.value;
        //                cmd.Parameters["@packet"].Value = mmsEvent.packet == null ? DBNull.Value : (object)mmsEvent.packet;
        //                cmd.Parameters["@packet_len"].Value = mmsEvent.packet_len;

        //                // 삽입 쿼리 실행
        //                try
        //                {
        //                    cmd.ExecuteNonQuery();
        //                }
        //                catch (SQLiteException ex)
        //                {
        //                    Console.WriteLine("SQL Error: " + ex.Message);
        //                    // 에러를 적절히 처리하거나 로그에 기록하십시오.
        //                }
        //            }
        //        } //using cmd
        //    } //using conn
        //}

    }
}
