using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proc_mon.Model
{
    public class ProcessInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Folder { get; set; }
        public string Desc { get; set; }
        public string Param { get; set; }
        public int Pid { get; set; }
        public int Count { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
    }
}
