using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPOWR.Model
{
    public class CountryViewModel 
    {
        public dynamic Countries { get; set; }
        public dynamic Partners { get; set; }
        public dynamic Districts { get; set; }
        public dynamic Memberships { get; set; }
        public readonly string NoMap = "NM";
    }


    public class buunit
    {
        public short BusinessUnitID { get; set; }
        public string DisplayName { get; set; }
        public short ID { get; set; }
        public string Name { get; set; }

    }
}
