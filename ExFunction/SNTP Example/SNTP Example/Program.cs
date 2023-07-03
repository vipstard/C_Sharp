using System;
using System.Collections.Generic;
using System.Threading;
using PacketCap_MMS;
using SNTP_Example.Model;

class Program
{
	static void Main()
	{
		Thread t = new Thread(Main);
		DbManager dbManager = new DbManager();
		NtpClient ntpClient = new NtpClient();
		string sntpIp = dbManager.GetSntpIp();
		List<SntpConnection> DeviceList = null;


		while (true)
		{
			dbManager.SetSntpDevice();
			dbManager.DeleteSntpDevice();

			DeviceList = dbManager.GetDevicesList();

			List<SntpConnection> checkSntpList = ntpClient.CheckSntp(sntpIp, DeviceList);
			dbManager.UpdateStatus(checkSntpList);

			Thread.Sleep(5000);
			Console.WriteLine();
		}
		
			

	}

	
}