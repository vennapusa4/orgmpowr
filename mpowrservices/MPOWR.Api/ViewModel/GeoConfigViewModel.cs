using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPOWR.Api.ViewModel
{
    public class GeoConfigViewModel
    {
        public int ID { get; set; }
        public int GeoID { get; set; }
        public string GeoName { get; set; }
        public bool IsCountryLevel { get; set; }
        public bool IsOverAllocation { get; set; }
        public bool IsApplicable { get; set; }
    }
}