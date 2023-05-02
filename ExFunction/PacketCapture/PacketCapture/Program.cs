using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using PacketCapture;
using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;

public class Program
{
	static readonly object fileLock = new object();
	static ConcurrentQueue<RawCapture> packetQueue = new ConcurrentQueue<RawCapture>();
	static CaptureFileWriterDevice pcapWriter;

	static void Main(string[] args)
	{
		DbManager dbManager = new DbManager();

		long maxFileSize = GetMaxFileSize();

		// Get the device list
		CaptureDeviceList devices = CaptureDeviceList.Instance;

		// Select the device you want to capture packets from
		// NIC Enable 이름 찾아서 그 패킷만 캡쳐.
		// 저장용량 단위(MB) 로 패킷 캡쳐한 후 D드라이브에 파일로 저장하는 예제
		ILiveDevice selectedDevice = devices.FirstOrDefault(d => d.Name == dbManager.GetNIC());

		if (selectedDevice == null)
		{
			Console.WriteLine("Device not found.");
			return;
		}

		// Open the device for capturing
		selectedDevice.Open(DeviceModes.Promiscuous, 1000);

		// Set a filter expression to capture all packets
		selectedDevice.Filter = "";

		// Create a capture thread
		var captureThread = new Thread(() => CapturePackets(selectedDevice, maxFileSize));
		captureThread.Start();

		Console.WriteLine("Press Enter to stop the capture...");
		Console.ReadLine();

		// Stop the capture and close the device when the program exits
		selectedDevice.StopCapture();
		selectedDevice.Close();
	}

	static void CapturePackets(ICaptureDevice device, long maxFileSize)
	{
		long packetLength = 0;
		device.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival);

		device.StartCapture();

		while (packetQueue.Count > 0 || device.Started)
		{
			if (packetQueue.TryDequeue(out RawCapture rawPacket))
			{
				Console.WriteLine("TryDequeue");
				Packet packet = Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
				
				// 패킷 byte 크기 
				packetLength += rawPacket.Data.Length;

				string[] timestamp = ConvertUnixTimestamp(rawPacket);
				string fileName = GetFileName(timestamp);

				lock (fileLock)
				{
					if (pcapWriter == null)
					{
						// Create a new file for writing packets
						Console.WriteLine($"@@@@ {fileName} Save @@@@@@");
						pcapWriter = new CaptureFileWriterDevice(fileName);
						pcapWriter.Open();
					}

					// Write the packet to the pcap file
					pcapWriter.Write(rawPacket);

					if (packetLength >= maxFileSize)
					{
						// Close the current file and create a new one
						pcapWriter.Close();
						packetLength = 0;
						Console.WriteLine($"@@@@ {pcapWriter.Name} closed. @@@@@@");

						// Create a new file for writing packets
						Console.WriteLine($"@@@@ {fileName} Save @@@@@@");
						pcapWriter = new CaptureFileWriterDevice(fileName);
						pcapWriter.Open();
					}
				}
			}
		}
	}

	static string GetFileName(string[] timeStamp)
	{
		var fileName = Path.Combine("D:\\TEST", $"{timeStamp[0]}-{timeStamp[1]}.pcap");
		return fileName;
	}

	static string[] ConvertUnixTimestamp(RawCapture rawPacket)
	{
		// UnixTimestamp 시간 변환
		DateTime packetTime = rawPacket.Timeval.Date;
		long unixTimestampTicks = packetTime.Ticks - DateTimeOffset.UnixEpoch.Ticks;
		double unixTimestampMicroseconds = (double)unixTimestampTicks / TimeSpan.TicksPerMillisecond / 1000;
		var timestamp = unixTimestampMicroseconds.ToString().Split(".");

		// 소수점 뒤 숫자 개수가 6개 이상인 경우 6개까지만 출력 6자리 미만인 경우 그대로 반환
		timestamp[1] = timestamp[1].Length < 6 ? timestamp[1] : timestamp[1].Substring(0, 6); 

		return timestamp;
	}

	static long GetMaxFileSize()
	{
		DbManager dbManager = new DbManager();
		return dbManager.GetStorageCapacity() * 1024 * 1024; 
	}

	static void device_OnPacketArrival(object sender, SharpPcap.PacketCapture e)
	{
		if (((ICaptureDevice)sender).Started)
		{
			Console.WriteLine("Enqueue");
			packetQueue.Enqueue(e.GetPacket());
		}
	}
}
