using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SNTP_Example.Model;

namespace PacketCap_MMS
{
    public class DbManager
    {
        private string _sntpConn = @"Data Source=C:\nms4sa\database\sr.db";
        private string _connConn = @"Data Source=C:\nms4sa\database\nms_config.db";


		public void SetSntpDevice()
		{
			// nms_config.db에 접속하여 device 테이블 및 setting 테이블 조회
			using (var nmsConfigConnection = new SQLiteConnection(_connConn))
			{
				nmsConfigConnection.Open();

				using (var deviceCommand = new SQLiteCommand("SELECT * FROM device WHERE type IN (0, 7)", nmsConfigConnection))
				{
					using (var deviceReader = deviceCommand.ExecuteReader())
					{
						while (deviceReader.Read())
						{
							// 필요한 데이터 가져오기
							string deviceName = deviceReader["name"].ToString();
							string deviceIp = deviceReader["ip"].ToString();
							int type = Convert.ToInt32(deviceReader["type"]);
							int deviceId = Convert.ToInt32(deviceReader["id"]);

							// sr.db에 중복 확인하여 데이터 삽입
							using (var srConnection = new SQLiteConnection(_sntpConn))
							{
								srConnection.Open();

								// 중복 여부 확인
								using (var duplicateCheckCommand = new SQLiteCommand(srConnection))
								{
									duplicateCheckCommand.CommandText = "SELECT COUNT(*) FROM sntp_connection WHERE deviceId = @deviceId";
									duplicateCheckCommand.Parameters.AddWithValue("@deviceId", deviceId);

									int count = Convert.ToInt32(duplicateCheckCommand.ExecuteScalar());
									if (count > 0)
									{
										// 중복된 값이 이미 존재하므로 건너뜀
										continue;
									}
								}

								// 중복이 없으므로 데이터 삽입
								using (var insertCommand = new SQLiteCommand(srConnection))
								{
									insertCommand.CommandText = "INSERT INTO sntp_connection (name, ip, type, deviceId, sntp_period) VALUES (@deviceName, @deviceIp, @type, @deviceId, @sntpPeriod)";
									insertCommand.Parameters.AddWithValue("@deviceName", deviceName);
									insertCommand.Parameters.AddWithValue("@deviceIp", deviceIp);
									insertCommand.Parameters.AddWithValue("@type", type);
									insertCommand.Parameters.AddWithValue("@deviceId", deviceId);

									// nms_config.db의 setting 테이블에서 ntp_period 값을 가져와서 sntp_period에 삽입
									using (var settingCommand = new SQLiteCommand("SELECT ntp_period FROM setting", nmsConfigConnection))
									{
										string ntpPeriod = settingCommand.ExecuteScalar()?.ToString();
										insertCommand.Parameters.AddWithValue("@sntpPeriod", ntpPeriod);

										insertCommand.ExecuteNonQuery();
									}
								}
							}
						}
					}
				}
			}
		}


		public void DeleteSntpDevice()
		{
			// nms_config.db에 접속하여 device 테이블에서 id 조회
			using (var nmsConfigConnection = new SQLiteConnection(_connConn))
			{
				nmsConfigConnection.Open();

				List<int> deviceIds = new List<int>();

				using (var command = new SQLiteCommand("SELECT id FROM device WHERE type IN (0, 7)", nmsConfigConnection))
				{
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							int deviceId = Convert.ToInt32(reader["id"]);
							deviceIds.Add(deviceId);
						}
					}
				}

				// sr.db에 접속하여 데이터 삭제
				using (var srConnection = new SQLiteConnection(_sntpConn))
				{
					srConnection.Open();

					// sntp_connection 테이블에서 deviceId가 포함된 데이터 삭제
					using (var deleteCommand = new SQLiteCommand(srConnection))
					{
						StringBuilder commandText = new StringBuilder("DELETE FROM sntp_connection WHERE deviceId NOT IN (");
						for (int i = 0; i < deviceIds.Count; i++)
						{
							commandText.Append("@deviceIds" + i);
							deleteCommand.Parameters.AddWithValue("@deviceIds" + i, deviceIds[i]);
							if (i < deviceIds.Count - 1)
							{
								commandText.Append(", ");
							}
						}
						commandText.Append(")");
						deleteCommand.CommandText = commandText.ToString();
						deleteCommand.ExecuteNonQuery();
					}
				}
			}
		}

		public string GetSntpIp()
		{
			string ip = "";

			using (var conn = new SQLiteConnection(_connConn))
			{
				conn.Open();

				using (var command = new SQLiteCommand("SELECT ntp_server FROM setting ", conn))
				{
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							 ip = reader["ntp_server"].ToString();
						}
					}
				}
			}
			return ip;
		}

		public List<SntpConnection> GetDevicesList()
		{
			List<SntpConnection> devices = new List<SntpConnection>();

			using (var conn = new SQLiteConnection(_sntpConn))
			{
				conn.Open();

				using (var command = new SQLiteCommand("SELECT * FROM sntp_connection", conn))
				{
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							int id = Convert.ToInt32(reader["id"]);
							int status = Convert.ToInt32(reader["status"]);
							string name = reader["name"].ToString();
							string ip = reader["ip"].ToString();
							int sntpFailFactor = Convert.ToInt32(reader["sntp_fail_factor"]);
							int sntpPeriod = Convert.ToInt32(reader["sntp_Period"]);
							int type = Convert.ToInt32(reader["type"]);
							int deviceId = Convert.ToInt32(reader["deviceId"]);

							SntpConnection device = new SntpConnection
							{
								Id = id,
								Status = status,
								Name = name,
								Ip = ip,
								SntpFailFactor = sntpFailFactor,
								SntpPeriod = sntpPeriod,	
								type = type,
								DeviceId = deviceId
							};

							devices.Add(device);
						}
					}
				}
			}

			return devices;
		}


		public void UpdateStatus(List<SntpConnection> list)
		{
			using (var conn = new SQLiteConnection(_sntpConn))
			{
				conn.Open();

				using (var command = new SQLiteCommand(conn))
				{
					foreach (var sntpConnection in list)
					{
						command.CommandText = $"UPDATE sntp_connection SET Status = {sntpConnection.Status} WHERE deviceId = '{sntpConnection.DeviceId}'";
						command.ExecuteNonQuery();
					}
				}
			}
		}

	}
}
