using System;
using System.IO;
using PacketCapture;
using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;

public class Program
{
	static readonly object fileLock = new object();
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
	}

	static void device_OnPacketArrival(object sender, SharpPcap.PacketCapture e)
	{
		lock (fileLock)
		{
			if (pcapWriter == null)
			{
				// Create a new file for writing packets
				var fileName = Path.Combine("D:\\", $"captured_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.pcap");
				pcapWriter = new CaptureFileWriterDevice(fileName);
				pcapWriter.Open();
			}

			var rawPacket = e.GetPacket();
			var packet = Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);

			// Write the packet to the pcap file
			pcapWriter.Write(rawPacket);

			if (DateTime.Now >= nextCaptureTime)
			{
				// Close the current file and create a new one
				pcapWriter.Close();
				var fileName = Path.Combine("D:\\", $"captured_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.pcap");
				pcapWriter = new CaptureFileWriterDevice(fileName);
				pcapWriter.Open();

				// Set the next capture time
				nextCaptureTime = DateTime.Now.AddMinutes(1);
			}
		}
	}
}


