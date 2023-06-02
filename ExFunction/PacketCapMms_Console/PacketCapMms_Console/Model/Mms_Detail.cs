namespace PacketCap_MMS.Model
{
	public class MMS_Detail
	{
		public int seqNum { get; set; }
		public string rptid { get; set; }
		public string src { get; set; }
		public string dst { get; set; }
		public string addr_dataset { get; set; }
		public string addr_data { get; set; }
		public string value { get; set; }
		public string quality { get; set; }
		public string description { get; set; }
		public string timestamp { get; set; }
		public string point_timestamp { get; set; }
	}
}