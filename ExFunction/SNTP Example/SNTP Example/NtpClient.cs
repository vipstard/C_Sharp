using GuerrillaNtp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using SNTP_Example.Model;

public class NtpClient
{

	public List<SntpConnection> CheckSntp(string sntpIp, List<SntpConnection> list)
	{
		try
		{
			DateTime? deviceLocal = null;
			GuerrillaNtp.NtpClient client = new GuerrillaNtp.NtpClient(IPAddress.Parse(sntpIp));
			NtpClock clock = null;

			try
			{
				clock = client.Query();
			}
			catch (SocketException ex)
			{
				Console.WriteLine("NTP 서버에 연결할 수 없습니다: " + ex.Message);
				Debug.WriteLine("NTP 서버에 연결할 수 없습니다: " + ex.Message);
			}

			if (clock != null)
			{
				DateTime sntpLocal = clock.Now.LocalDateTime;

				foreach (var sntpConnection in list)
				{
					string deviceIp = sntpConnection.Ip;
					GuerrillaNtp.NtpClient deviceClient = new GuerrillaNtp.NtpClient(IPAddress.Parse(deviceIp));
					NtpClock deviceClock = null;

					try
					{
						deviceClock = deviceClient.Query();
					}
					catch (Exception e)
					{
						Console.WriteLine($"Device 시각동기 연결 실패");
						Debug.WriteLine("Device 시각동기 연결 실패");
						sntpConnection.Status = 0;
					}

					if (deviceClock != null)
					{
						deviceLocal = deviceClock.Now.LocalDateTime;
						Console.Write("연결성공 ");

						TimeSpan timeDifference = sntpLocal - deviceLocal.Value;
						if (timeDifference.Duration() <= TimeSpan.FromSeconds(5))
						{
							sntpConnection.Status = 1;
							Console.WriteLine();
						}
						else
						{
							Console.WriteLine($"시간 불일치 {sntpLocal} :: {deviceLocal}");
							sntpConnection.Status = 0;
						}
					}
					else
					{
						
						sntpConnection.Status = 0;
					}
				}
			}

			return list;
		}
		catch (Exception ex)
		{
			Console.WriteLine("오류: " + ex.Message);
			return null;
		}
	}


	public void GetSntp(string paramIp)
	{
		try
		{
			IPAddress ip = IPAddress.Parse(paramIp);
			GuerrillaNtp.NtpClient client = new GuerrillaNtp.NtpClient(ip);
			NtpClock clock = null;

			try
			{
				clock = client.Query();
			}
			catch (SocketException ex)
			{
				Console.WriteLine("NTP 서버에 연결할 수 없습니다: " + ex.Message);
			}

			if (clock != null)
			{
				DateTime local = clock.Now.LocalDateTime;
				//DateTime utc = clock.UtcNow.UtcDateTime;
				Console.WriteLine($"{local}" );
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("오류: " + ex.Message);
		}

	}
}