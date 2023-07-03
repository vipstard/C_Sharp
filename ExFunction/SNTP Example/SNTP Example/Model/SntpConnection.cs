using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNTP_Example.Model
{

	public class SntpConnection
	{
		public int Id { get; set; }
		public string Ip { get; set; }
		public int Status { get; set; }
		public String Name { get; set; }
		public String Text { get; set; }
		public int SntpFailFactor { get; set; }
		public int SntpPeriod { get; set; }
		public int type { get; set; }
		public int DeviceId { get; set; }
	}
}
