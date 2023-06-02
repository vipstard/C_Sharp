using PacketCap_MMS;

namespace PacketCapMms_Console
{
	internal class Program
	{
		static void Main(string[] args)
		{
			PacketProcess proc = new PacketProcess();
			proc.ProcessStart();
		}
	}
}