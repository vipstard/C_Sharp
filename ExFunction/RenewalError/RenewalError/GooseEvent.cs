using System.ComponentModel.DataAnnotations.Schema;


namespace MDAS.Model.goose
{
    public class GooseEvent
    {
        public int recv_sec { get; set; }
        public int recv_msec { get; set; }
        public string ied_name { get; set; }
        public int status { get; set; }
        public string mac { get; set; }
        public string address { get; set; }
        public string timestamp { get; set; }
        public int process_time { get; set; }
        public int st_num { get; set; }
        public int sq_num { get; set; }
        public byte[] packet { get; set; }
        public int packet_len { get; set; }
        [NotMapped]
        public string dateTime { get; set; }
    }
}
