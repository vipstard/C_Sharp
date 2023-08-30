using MDAS.Model.mms;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using MDAS.Model.goose;

namespace RenewalError
{
    public class DbManager
    {
        private string _mrConn = @"Data Source = C:\nms4sa\database\mr.db";
        private string _grConn = @"Data Source = C:\nms4sa\database\gr.db";
        private string _mmsLastTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        private string _gooseLastTimestamp = (DateTime.Now - TimeSpan.FromHours(9)).ToString("yyyy-MM-dd HH:mm:ss");

        public List<MmsEvent> GetRenewalMmsList()
        {
            List<MmsEvent> eventList = new List<MmsEvent>();
            using (var conn = new SQLiteConnection(_mrConn))
            {
                conn.Open();
                Console.WriteLine($"mtime : {_mmsLastTimestamp}");

                // 마지막으로 검색한 데이터의 타임스탬프 이후로 추가된 데이터만 선택
                string query = "SELECT * FROM MMS_EVENT WHERE ((STATUS = 7 AND EXTRA_INFO = 2) OR STATUS = 5)";
                if (!string.IsNullOrEmpty(_mmsLastTimestamp))
                {
                    query += $" AND timestamp > '{_mmsLastTimestamp}'";
                }

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MmsEvent mmsEvent = new MmsEvent();
                            mmsEvent.timestamp = reader["timestamp"].ToString();
                            mmsEvent.src_name = reader["src_name"].ToString();
                            mmsEvent.src_ip = reader["src_ip"].ToString();
                            mmsEvent.dst_name = reader["dst_name"].ToString();
                            mmsEvent.dst_ip = reader["dst_ip"].ToString();
                            mmsEvent.status = Convert.ToInt32(reader["status"].ToString());
                            mmsEvent.extra_Info = Convert.ToInt32(reader["extra_Info"].ToString());
                            mmsEvent.address = reader["address"].ToString();
                            mmsEvent.value = reader["value"].ToString();
                            mmsEvent.packet = reader["packet"] != DBNull.Value ? (byte[])reader["packet"] : null;
                            mmsEvent.packet_len = Convert.ToInt32(reader["packet_len"].ToString());

                            eventList.Add(mmsEvent);

                            // 검색한 데이터 중 가장 최근의 타임스탬프 기억
                            _mmsLastTimestamp = mmsEvent.timestamp;

                        } //while
                    } //using reader
                } //using cmd
            } //using conn

          
            return eventList;
        }

        public List<GooseEvent> GetRenewalGooseList()
        {
            List<GooseEvent> gooseEventList = new List<GooseEvent>();
            using (var conn = new SQLiteConnection(_grConn))
            {
                conn.Open();
                Console.WriteLine($"gtime now : {_gooseLastTimestamp}");

                // 마지막으로 검색한 데이터의 타임스탬프 이후로 추가된 데이터만 선택
                string query = "SELECT * FROM GOOSE_EVENT WHERE STATUS = 2";

                if (!string.IsNullOrEmpty(_gooseLastTimestamp))
                {
                    query += $" AND timestamp > '{_gooseLastTimestamp}'";
                }

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GooseEvent gooseEvent = new GooseEvent();
                           gooseEvent.recv_sec = Convert.ToInt32(reader["recv_sec"].ToString());
                           gooseEvent.recv_msec = Convert.ToInt32(reader["recv_msec"].ToString()); 
                           gooseEvent.ied_name = reader["ied_name"].ToString();
                           gooseEvent.status = Convert.ToInt32(reader["status"].ToString());
                           gooseEvent.mac = reader["mac"].ToString();
                           gooseEvent.address = reader["address"].ToString();
                           gooseEvent.timestamp = reader["timestamp"].ToString();
                           gooseEvent.process_time =Convert.ToInt32(reader["process_time"].ToString());
                           gooseEvent.st_num =  Convert.ToInt32(reader["st_num"].ToString());
                           gooseEvent.sq_num = Convert.ToInt32(reader["sq_num"].ToString());
                           gooseEvent.packet = reader["packet"] != DBNull.Value ? (byte[])reader["packet"] : null;
                           gooseEvent.packet_len = Convert.ToInt32(reader["packet_len"].ToString());

                           gooseEventList.Add(gooseEvent);

                            // 검색한 데이터 중 가장 최근의 타임스탬프 기억
                            _gooseLastTimestamp = gooseEvent.timestamp;

                        } //while
                    } //using reader
                } //using cmd
            } //using conn
            Console.WriteLine($"gtime Last : {_gooseLastTimestamp}");
            return gooseEventList;
        }

        public void InsertMmsErrorList(List<MmsEvent> list)
        {
            using (var conn = new SQLiteConnection(_mrConn))
            {
                conn.Open();

                // mmsError 테이블에 데이터 삽입
                string insertQuery =
                    "INSERT INTO mms_Error (timestamp, src_name, src_ip, dst_name, dst_ip, status, extra_Info, address, value, packet, packet_len) " +
                    "VALUES (@timestamp, @src_name, @src_ip, @dst_name, @dst_ip, @status, @extra_Info, @address, @value, @packet, @packet_len)";

                using (var cmd = new SQLiteCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@timestamp", "");
                    cmd.Parameters.AddWithValue("@src_name", "");
                    cmd.Parameters.AddWithValue("@src_ip", "");
                    cmd.Parameters.AddWithValue("@dst_name", "");
                    cmd.Parameters.AddWithValue("@dst_ip", "");
                    cmd.Parameters.AddWithValue("@status", 0);
                    cmd.Parameters.AddWithValue("@extra_Info", 0);
                    cmd.Parameters.AddWithValue("@address", "");
                    cmd.Parameters.AddWithValue("@value", "");
                    cmd.Parameters.AddWithValue("@packet", "");
                    cmd.Parameters.AddWithValue("@packet_len", 0);

                    foreach (MmsEvent mmsEvent in list)
                    {
                        // mmsEvent에서 값을 가져와서 파라미터 설정
                        cmd.Parameters["@timestamp"].Value = mmsEvent.timestamp;
                        cmd.Parameters["@src_name"].Value = mmsEvent.src_name;
                        cmd.Parameters["@src_ip"].Value = mmsEvent.src_ip;
                        cmd.Parameters["@dst_name"].Value = mmsEvent.dst_name;
                        cmd.Parameters["@dst_ip"].Value = mmsEvent.dst_ip;
                        cmd.Parameters["@status"].Value = mmsEvent.status;
                        cmd.Parameters["@extra_Info"].Value = mmsEvent.extra_Info;
                        cmd.Parameters["@address"].Value = mmsEvent.address;
                        cmd.Parameters["@value"].Value = mmsEvent.value;
                        cmd.Parameters["@packet"].Value = mmsEvent.packet == null ? DBNull.Value : (object)mmsEvent.packet;
                        cmd.Parameters["@packet_len"].Value = mmsEvent.packet_len;

                        // 삽입 쿼리 실행
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (SQLiteException ex)
                        {
                            Console.WriteLine("SQL Error: " + ex.Message);
                            // 에러를 적절히 처리하거나 로그에 기록하십시오.
                        }
                    }
                } //using cmd
            } //using conn
        }

        public void InsertGooseErrorList(List<GooseEvent> list)
        {
            using (var conn = new SQLiteConnection(_grConn))
            {
                conn.Open();

                // goose_error 테이블에 데이터 삽입
                string insertQuery =
                    "INSERT INTO GOOSE_ERROR (timestamp, recv_sec, recv_msec, ied_name, status, mac, address, process_time, st_num, sq_num, packet, packet_len) " +
                    "VALUES (@timestamp, @recv_sec, @recv_msec, @ied_name, @status, @mac, @address, @process_time, @st_num, @sq_num, @packet, @packet_len)";

                using (var cmd = new SQLiteCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@timestamp", "");
                    cmd.Parameters.AddWithValue("@recv_sec", 0);
                    cmd.Parameters.AddWithValue("@recv_msec", 0);
                    cmd.Parameters.AddWithValue("@ied_name", "");
                    cmd.Parameters.AddWithValue("@status", 0);
                    cmd.Parameters.AddWithValue("@mac", "");
                    cmd.Parameters.AddWithValue("@address", "");
                    cmd.Parameters.AddWithValue("@process_time", 0);
                    cmd.Parameters.AddWithValue("@st_num", 0);
                    cmd.Parameters.AddWithValue("@sq_num", 0);
                    cmd.Parameters.AddWithValue("@packet", "");
                    cmd.Parameters.AddWithValue("@packet_len", 0);

                    foreach (GooseEvent gooseEvent in list)
                    {
                        // gooseEvent에서 값을 가져와서 파라미터 설정
                        cmd.Parameters["@timestamp"].Value = gooseEvent.timestamp; // 이 부분은 어떻게 처리할지 고려 필요
                        cmd.Parameters["@recv_sec"].Value = gooseEvent.recv_sec;
                        cmd.Parameters["@recv_msec"].Value = gooseEvent.recv_msec;
                        cmd.Parameters["@ied_name"].Value = gooseEvent.ied_name;
                        cmd.Parameters["@status"].Value = gooseEvent.status;
                        cmd.Parameters["@mac"].Value = gooseEvent.mac;
                        cmd.Parameters["@address"].Value = gooseEvent.address;
                        cmd.Parameters["@process_time"].Value = gooseEvent.process_time;
                        cmd.Parameters["@st_num"].Value = gooseEvent.st_num;
                        cmd.Parameters["@sq_num"].Value = gooseEvent.sq_num;
                        cmd.Parameters["@packet"].Value = gooseEvent.packet == null ? DBNull.Value : (object)gooseEvent.packet;
                        cmd.Parameters["@packet_len"].Value = gooseEvent.packet_len;

                        // 삽입 쿼리 실행
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (SQLiteException ex)
                        {
                            Console.WriteLine("SQL Error: " + ex.Message);
                            // 에러를 적절히 처리하거나 로그에 기록하십시오.
                        }
                    }
                } //using cmd
            } //using conn
        }
    }
}
