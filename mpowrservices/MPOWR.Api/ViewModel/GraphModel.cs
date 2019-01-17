using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPOWR.Api.ViewModel
{
    public class GraphModel
    {

        public decimal Default_Sustain_MDF_Percentage { get; set; }

        public decimal Default_Growth_MDF_Percentage { get; set; }


        public decimal Final_Sustain_MDF_Percentage { get; set; }

        public decimal Final_Growth_MDF_Percentage { get; set; }

        public decimal Sustain_MDF_Percentage { get; set; }
        public decimal Growth_MDF_Percentage { get; set; }
        public decimal SalesGrowth_with_mdf { get; set; }
        public decimal SalesGrowth_without_mdf { get; set; }
        public decimal MDFBYSellout_Percentage { get; set; }
        public decimal Aligned_Percentage { get; set; }
        public decimal MisAligned_Percentage { get; set; }


        public decimal Default_SalesGrowth_with_mdf { get; set; }
        public decimal Default_SalesGrowth_without_mdf { get; set; }
        public decimal Default_MDFBYSellout_Percentage { get; set; }

        public decimal Default_Aligned_Percentage  { get; set; }
        public decimal Default_Misaligned_Percentage { get; set; }



        public decimal Default_MDF_Platinum_Percentage { get; set; }
        public decimal Default_MDF_Gold_Percentage { get; set; }
        public decimal Default_MDF_SB_Percentage { get; set; }

        public decimal MDF_Platinum_Percentage { get; set; }
        public decimal MDF_Gold_Percentage { get; set; }
        public decimal MDF_SB_Percentage { get; set; }

    }

}