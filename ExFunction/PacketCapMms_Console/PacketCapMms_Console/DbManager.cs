using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacketCap_MMS.Model;

namespace PacketCap_MMS
{
	public class DbManager
	{
		private string _mmsConn = @"Data Source=C:\nms4sa\database\mms_packet.db";
		public void SetDatabase_Mms_Detail_Data(MMS_Detail mms_detail)
		{
			using (SQLiteConnection conn = new SQLiteConnection(_mmsConn))
			{
				conn.Open();
				string query = "insert into mms_detail " +
							   "(sqNum,src,dst,rptid,addr_dataset,description,addr_data,value,quality,timestamp,point_timestamp)" +
							   " values(" +
							   "@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9,@param10,@param11)";

				using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
				{

					cmd.Parameters.Add(new SQLiteParameter("@param1", mms_detail.seqNum));
					cmd.Parameters.Add(new SQLiteParameter("@param2", mms_detail.src));
					cmd.Parameters.Add(new SQLiteParameter("@param3", mms_detail.dst));
					cmd.Parameters.Add(new SQLiteParameter("@param4", mms_detail.rptid));
					cmd.Parameters.Add(new SQLiteParameter("@param5", mms_detail.addr_dataset));
					cmd.Parameters.Add(new SQLiteParameter("@param6", mms_detail.description));
					cmd.Parameters.Add(new SQLiteParameter("@param7", mms_detail.addr_data));
					cmd.Parameters.Add(new SQLiteParameter("@param8", mms_detail.value));
					cmd.Parameters.Add(new SQLiteParameter("@param9", mms_detail.quality));
					cmd.Parameters.Add(new SQLiteParameter("@param10", mms_detail.timestamp));
					cmd.Parameters.Add(new SQLiteParameter("@param11", mms_detail.point_timestamp));
					cmd.ExecuteNonQuery();
					conn.Close();
				}//using cmd
			}//using conn
		}
	}
}
