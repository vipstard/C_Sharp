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
		private readonly string configConn = @"Data Source=C:\nms4sa\database\nms_config.db";

		public string GetNIC()
		{
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

		/// <summary>
		/// Setting 테이블의 파일 저장용량(MB) 단위 값을 읽어온다.
		/// </summary>
		/// <returns></returns>
		public int GetStorageCapacity()
		{
			string query = "SELECT  storage_dump_limit FROM setting";
			int StorageCapacity = 0;

			using (SQLiteConnection conn = new SQLiteConnection(configConn))
			{
				using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
				{
					conn.Open();
					SQLiteDataReader reader = cmd.ExecuteReader();
					while (reader.Read())
					{
						StorageCapacity = reader.GetInt32(0);
					}

					reader.Close();
					conn.Close();
				}//using cmd
			}//using conn

			return StorageCapacity;
		}

		//public void AddMetaDb()
		//{
			
		//}
	}
}
