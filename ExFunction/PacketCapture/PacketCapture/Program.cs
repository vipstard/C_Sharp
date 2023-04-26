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
	static DateTime nextCaptureTime = DateTime.MinValue;
	static CaptureFileWriterDevice pcapWriter;

	static void Main(string[] args)
	{
		DbManager dbManager = new DbManager();
		// Get the device list
		var devices = CaptureDeviceList.Instance;

		// Select the device you want to capture packets from
		// NIC Enable 이름 찾아서 그 패킷만 가져온다.
		// 1분 단위로 패킷 캡쳐한 후 D드라이브에 파일로 저장하는 예제
		var selectedDevice = devices.FirstOrDefault(d => d.Name == dbManager.GetNIC());

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
		var captureThread = new Thread(() => CapturePackets(selectedDevice));
		captureThread.Start();

		Console.WriteLine("Press Enter to stop the capture...");
		Console.ReadLine();

		// Stop the capture and close the device when the program exits
		selectedDevice.StopCapture();
		selectedDevice.Close();
	}

	static void CapturePackets(ICaptureDevice device)
	{
		device.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival);

		device.StartCapture();

		while (packetQueue.Count > 0 || device.Started)
		{
			if (packetQueue.TryDequeue(out RawCapture rawPacket))
			{
				Console.WriteLine("TryDequeue");
				var packet = Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
				//
				DateTime now = DateTime.Now;
				lock (fileLock)
				{
					if (pcapWriter == null)
					{
						// Create a new file for writing packets
						var fileName = Path.Combine("D:\\TEST", $"captured_{TimeZoneInfo.ConvertTimeToUtc(now)}.pcap");
						pcapWriter = new CaptureFileWriterDevice(fileName);
						pcapWriter.Open();
					}

					// Write the packet to the pcap file
					pcapWriter.Write(rawPacket);

					if (DateTime.Now >= nextCaptureTime)
					{
						// Close the current file and create a new one
						pcapWriter.Close();
						var fileName = Path.Combine("D:\\TEST", $"captured_{TimeZoneInfo.ConvertTimeToUtc(now)}.pcap");
						pcapWriter = new CaptureFileWriterDevice(fileName);
						pcapWriter.Open();

						// Set the next capture time
						nextCaptureTime = DateTime.Now.AddMinutes(1);
					}
				}
			}
		}
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
