using PacketCap_MMS.Model;
using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PacketCap_MMS
{
	public class PacketProcess
	{
		//private static List<string> dataList = new List<string>();
		private string NIC_Name = "";
		private DbManager dbManager = new DbManager();
		private List<string> rptidList = new List<string>();
		private List<string> dataList = new List<string>();
		private List<string> dataSetList = new List<string>();
		private Task mmsTask = null;
		public ILiveDevice device = null;
		private bool QueueTaskFlag = false;
		public ConcurrentQueue<MMS_Packet> _mmsDetailDataQueue = new ConcurrentQueue<MMS_Packet>();
		private string connStr = @"Data Source=C:\nms4sa\database\nms_config.db";

		public void ProcessStart()
		{
			StartMmsTask();
			CapturePackets();
		}

		public void StartMmsTask()
		{
			if (mmsTask == null)
			{
				QueueTaskFlag = true;
				mmsTask = Task.Factory.StartNew(MMS_Dequeue);
			}
		}

		public void StopProcess()
		{
			device.StopCapture();
			//while (mmsTask != null)
			//{
			//    if (_mmsDetailDataQueue.Count == 0)
			//    {
			//        mmsTask.Dispose();
			//    }
			//}
		}

		public void MMS_Dequeue()
		{
			//queue analyze
			while (QueueTaskFlag)
			{
				while (_mmsDetailDataQueue.Count > 0)
				{
					EthernetPacket ePacket = null;
					MMS_Packet mPacket = null;
					_mmsDetailDataQueue.TryDequeue(out mPacket);
					ePacket = mPacket.ePacket;

					byte[] data = ePacket.Bytes;
					Console.WriteLine("data size: " + data.Length);

					if (ePacket.Type == EthernetType.IPv4)
					{
						int seqNum = 0;
						string str = "";
						string rptid = "";
						string addr_DataSet = "";
						string addr_Data = "";
						int listIdx = 0;
						int efCnt = 0;
						bool afterData = false;//패킷 data 위치(rptid,dataset,data) 지났는지
						string point_timestamp = "";
						byte value = 0;
						string quality = "";

						StringBuilder sb = new StringBuilder();
						if (data.Length < 1000 && data.Length > 90)
						{
							MMS_Detail mms_detail = new MMS_Detail();
							if (data[87] == 82 && data[88] == 80 && data[89] == 84)
							{
								for (int i = 77; i < data.Length; i++)
								{
									//Packet byte data들 char로 치환->rptid,dataset,data찾기 *밑에서 계속 초기화
									str += Convert.ToChar(data[i]);
									sb.Append(Convert.ToChar(data[i]));

									if (listIdx == 0 || listIdx == 1)
									{
										for (int j = 0; j < rptidList.Count; j++)
										{

											if (str.Contains(rptidList[j]))
											{
												listIdx++;
												rptid = rptidList[j];
												afterData = true;
												str = "";
											}//if
										}//for rptidList

										if (data[i] == 140 && afterData)//mms 시간 index
										{
											if (data[i + 1] == 6)
											{
												seqNum = data[i - 1];
												afterData = false;
											}
										}

										for (int j = 0; j < dataSetList.Count; j++)
										{
											if (str.Contains(dataSetList[j]))
											{
												listIdx++;
												addr_DataSet = dataSetList[j];
												str = "";
											}//if
										}//for dataSetList

									}//if list_idx
									else if (listIdx == 2)
									{
										for (int j = 0; j < dataList.Count; j++)
										{
											if (str.Contains(dataList[j]))
											{
												afterData = true;
												addr_Data = dataList[j];

												break;
											}//if
										}//for dataList
										#region find data value 131(83):bool 132(84):bit-string 134(86):unsigned

										if (data[i] == 131 && afterData) //131(83):bool
										{
											value = data[i + 2];
										}
										else if (data[i] == 132 && afterData)//84 bitstring
										{
											if (efCnt == 0)
											{
												efCnt++;
												quality = "";
												for (int j = i + 3; j <= (i + data[i + 1] + 1); j++)
												{
													quality += ToHex(data[j]);
												}

											}
										}

										else if (data[i] == 145 && afterData)//91 time
										{
											if (data[i + 1] == 8)//08 time
											{
												string timestamp = "";
												for (int j = i + 2; j < i + 6; j++)
												{
													timestamp += ToHex(data[j]);
												}//for
												long unixTimestamp = Convert.ToInt64(timestamp, 16); // convert hex to decimal
												DateTimeOffset kstDateTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp); // convert Unix timestamp to DateTimeOffset in KST
												point_timestamp = kstDateTime.ToString("yyyy-MM-dd HH:mm:ss");
											}//if
										}//else if 145

										#endregion
									}//else if lidstIdx ==2

								}//for datalength

								#region save into mms_packet                                

								mms_detail.seqNum = seqNum;
								mms_detail.rptid = rptid;
								mms_detail.addr_dataset = addr_DataSet;
								mms_detail.addr_data = addr_Data;
								mms_detail.value = (value == 1 ? true : false).ToString();//True false 인지 1,0인지
								mms_detail.timestamp = mPacket.timestamp;
								mms_detail.src = ePacket.SourceHardwareAddress.ToString();
								mms_detail.dst = ePacket.DestinationHardwareAddress.ToString();
								mms_detail.description = " ";//추후 IET에서 가져와야 함
								mms_detail.quality = quality.Equals("0000") ? "Good" : "abnormal";//0000이 Good??
								mms_detail.point_timestamp = point_timestamp;
								dbManager.SetDatabase_Mms_Detail_Data(mms_detail);
								#endregion
							}//if data.Length
						}//if 87 88 89
					}//if IPv4
				}
				Thread.Sleep(1000);
			}
		}


		/// <summary>
		/// Prints the time and length of each received packet
		/// </summary>
		private void device_OnPacketArrival(object sender, PacketCapture e)
		{
			var rawPacket = e.GetPacket();
			MMS_Packet mmsPacket = new MMS_Packet();
			var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
			EthernetPacket ePacket = packet.Extract<EthernetPacket>();
			mmsPacket.ePacket = ePacket;
			mmsPacket.timestamp = e.Header.Timeval.Date.AddHours(9).ToString();


			if (ePacket.Type == EthernetType.IPv4)
			{
				_mmsDetailDataQueue.Enqueue(mmsPacket);
			}//if IPv4

		}



		public void CapturePackets()
		{
			// Retrieve the device list
			var devices = CaptureDeviceList.Instance;
			int i = 0;
			int idx = 0;
			string query = "SELECT name FROM nic WHERE enable = 1";
			GetData(connStr, query, ref NIC_Name, idx);

			foreach (var dev in devices)
			{
				if (dev.Name.Equals(NIC_Name))
				{
					device = devices[i];
					break;
				}
				i++;
			}


			query = "select distinct rptid from rcb";
			GetData(connStr, query, ref rptidList, idx);
			idx = 1;
			query = "select distinct address from dataset";
			GetData(connStr, query, ref dataSetList, idx);
			idx = 2;
			query = "select distinct address from data";
			GetData(connStr, query, ref dataList, idx);//나중에 수정(갯수가 많을경우 고려)
													   // Open the device for capturing
			int readTimeoutMilliseconds = 1000;
			device.Open(DeviceModes.Promiscuous, readTimeoutMilliseconds);

			// Register our handler function to the 'packet arrival' event
			device.OnPacketArrival +=
				new PacketArrivalEventHandler(device_OnPacketArrival);

			// Start the capturing process
			device.StartCapture();

		}

		public void GetData<T>(string connStr, string query, ref T input2, int idx)
		{
			using (SQLiteConnection conn = new SQLiteConnection(connStr))
			{
				using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
				{
					//Debugger.Launch();
					conn.Open();
					SQLiteDataReader reader = cmd.ExecuteReader();
					while (reader.Read())
					{
						if (input2 is string)
						{
							NIC_Name = reader.GetString(0);
						}
						else if (input2 is List<string>)
						{
							switch (idx)
							{
								case 0:
									rptidList.Add(reader.GetString(0));
									break;
								case 1:
									dataSetList.Add(reader.GetString(0));
									break;
								case 2:
									dataList.Add(reader.GetString(0));
									break;
							}
						}
					}

					reader.Close();
					conn.Close();
				}//using cmd
			}//using conn
		}



		public static string ToHex(int i)
		{
			// 대문자 X일 경우 결과 hex값이 대문자로 나온다.
			string hex = i.ToString("X");
			if (hex.Length % 2 != 0)
			{
				hex = "0" + hex;
			}
			return hex;
		}


	}
}


