using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPOWR.Model
{
   public class MDFImport
    {
        public string Country { get; set; }
        public string Budget { get; set; }
        public string BusinessUnit { get; set; }
        public string PartnerID { get; set; }
        public decimal? Allocated_MDF { get; set; }
        public decimal? MSA { get; set; }
        public decimal? Aruba_MSA { get; set; }
        public string  ProcessResult { get; set; }

    }
}
