using PacketCap_MMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketCapMms_Console
{
	internal class Program
	{
		static void Main(string[] args)
		{
			while (true)
			{
				PacketProcess proc = new PacketProcess();
				proc.ProcessStart();
			}
			
		}
	}
}
