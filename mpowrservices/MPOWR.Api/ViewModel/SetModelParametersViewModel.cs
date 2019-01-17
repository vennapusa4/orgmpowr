using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPOWR.Api.ViewModel
{
   
        public class SetModelParametersViewModel
        {
            //public RegionDetails RegionDetails { get; set; }
            // public FinancialYearDetails FinancialYearDetails { get; set; }
            public string RegionDetails { get; set; }
            public int RegionId { get; set; }
            public string FinancialYearDetails { get; set; }
            public int FinancialYearId { get; set; }
            public int ModelParameterFYConfigID { get; set; }
            public int ModelParameterConfigID { get; set; }
            public decimal HighPerformerProductivity { get; set; }
            public decimal MediumPerformerProductivityRatio { get; set; }
            public decimal LowPerformerProductivityRatio { get; set; }
            public string UserId { get; set; }
        }
       
    }
