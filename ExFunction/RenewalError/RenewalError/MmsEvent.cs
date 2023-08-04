using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace MDAS.Model.mms
{
    //MMS_EVENT와 똑같음
    public class MmsEvent   
    {
        public string timestamp { get; set; }
        public string src_name { get; set; }
        public string src_ip { get; set; }
        public string dst_name { get; set; }
        public string dst_ip { get; set; }
        public int status { get; set; }
        public int extra_Info { get; set; }
        public string address { get; set; }
        public string value { get; set; }
        public byte[] packet { get; set; }
        public int packet_len { get; set; }
    }
}
