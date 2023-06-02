using PacketDotNet;

namespace PacketCap_MMS.Model
{
	public class MMS_Packet
	{
		public string timestamp { get; set; }
		public EthernetPacket ePacket { get; set; }
	}
}
