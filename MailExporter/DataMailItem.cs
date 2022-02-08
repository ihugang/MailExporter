using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailExporter
{
    public class DataMailItem
    {
        public bool IsSelected { get; set; }
        public uint No { get; set; }

        public string Subject { get; set; }

        public DateTime? SendTime { get; set; }
    }
    
}
