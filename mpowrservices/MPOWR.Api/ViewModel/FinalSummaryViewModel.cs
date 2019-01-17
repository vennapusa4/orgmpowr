using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPOWR.Api.ViewModel
{
    public class FinalSummaryViewModelRequest
    {
        public int Previous_Period_Year { get; set; }  // -- 0 Previous period    1 Previous Year
        public int Current_Financial_Period_Year_Id { get; set; }
        public int CountryID { get; set; }
        public int PartnerTypeID { get; set; }
        public int? DistrictID { get; set; }
        public int VersionID { get; set; }
    }

    public class FinalSummaryViewModelResponse
    {
        public string Description { get; set; }
        public string Grew_Previous_Period { get; set; }  // -- 0 Previous period    1 Previous Year
        public string Grew_Current_Period { get; set; }
        public string Grew_PoP { get; set; }
        public string Decl_Previous_Period { get; set; }
        public string Decl_Current_Period { get; set; }
        public string Decl_PoP { get; set; }
    }
}