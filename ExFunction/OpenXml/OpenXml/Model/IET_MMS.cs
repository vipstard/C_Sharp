using System.ComponentModel.DataAnnotations.Schema;

namespace MDAS.Model.config
{
    [Table("iet_mms")]
    public class IET_MMS
    {
        public int Id { get; set; }

        [Column("device_id")]
        public int? DeviceId { get; set; }

        public string? FileName { get; set; }

        public string? Description { get; set; }

        [NotMapped] 
        public string? GooseSend { get; set; }

        [Column("data_type")]
        public string? DataType { get; set; }

        public string? Address { get; set; }

        public IET_MMS()
        {
        }
    }
}
