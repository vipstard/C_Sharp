using MDAS.Model.mms;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace RenewalError
{
    public class DbManager
    {
        private string _mrConn = @"Data Source = C:\nms4sa\database\mr.db";
        private string _lastTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        public List<MmsEvent> GetRenewalList()
        {
            List<MmsEvent> eventList = new List<MmsEvent>();
            using (var conn = new SQLiteConnection(_mrConn))
            {
                conn.Open();

                // 마지막으로 검색한 데이터의 타임스탬프 이후로 추가된 데이터만 선택
                string query = "SELECT * FROM MMS_EVENT WHERE STATUS = 7 AND EXTRA_INFO = 2";
                if (!string.IsNullOrEmpty(_lastTimestamp))
                {
                    query += $" AND timestamp > '{_lastTimestamp}'";
                }

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MmsEvent mmsEvent = new MmsEvent();
                            mmsEvent.timestamp = reader["timestamp"].ToString();
                            mmsEvent.src_name = reader["timestamp"].ToString();
                            mmsEvent.src_ip = reader["src_ip"].ToString();
                            mmsEvent.dst_name = reader["src_ip"].ToString();
                            mmsEvent.dst_ip = reader["src_ip"].ToString();
                            mmsEvent.status = Convert.ToInt32(reader["status"].ToString());
                            mmsEvent.extra_Info = Convert.ToInt32(reader["extra_Info"].ToString());
                            mmsEvent.address = reader["address"].ToString();
                            mmsEvent.value = reader["value"].ToString();
                            mmsEvent.packet = reader["packet"] != DBNull.Value ? (byte[])reader["packet"] : null;
                            mmsEvent.packet_len = Convert.ToInt32(reader["packet_len"].ToString());

                            eventList.Add(mmsEvent);

                            // 검색한 데이터 중 가장 최근의 타임스탬프 기억
                            _lastTimestamp = mmsEvent.timestamp;

                        } //while
                    } //using reader
                } //using cmd
            } //using conn

            return eventList;
        }

        public void InsertErrorList(List<MmsEvent> list)
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
    }
}
