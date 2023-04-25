using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketCapture
{
	public class DbManager
	{
		public string GetNIC()
		{
			string configConn = @"Data Source=C:\nms4sa\database\nms_config.db";
			string query = "SELECT name FROM nic WHERE enable = 1";
			string NIC_Name = "";
			using (SQLiteConnection conn = new SQLiteConnection(configConn))
			{
				using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
				{
					conn.Open();
					SQLiteDataReader reader = cmd.ExecuteReader();
					while (reader.Read())
					{
						NIC_Name = reader.GetString(0);
					}

					reader.Close();
					conn.Close();
				}//using cmd
			}//using conn
			return NIC_Name;
		}
	}
}
