using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenXml.Model
{
    public class IET
    {
        public int id { get; set; }
        public int device_id { get; set; }
        public string fileName { get; set; }
        public string description { get; set; }
        public string DataType { get; set; }
        public string? Pd { get; set; }
        public string ld { get; set; }
        public string ln_Prefix { get; set; }
        public string ln { get; set; }
        public string ln_Inst_No { get; set; }
        public string fc { get; set; }
        public string data_Object { get; set; }
        public string data_Attribute { get; set; }
        [NotMapped]
        public string value { get; set; }
    }
}
