using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPOWR.Api.ViewModel
{
    public class PBRoundtwoViewModel
    {
        
    }
    public class Partners
    {
        public string PartnerID { get; set; }
        public string ParnterName { get; set; }

        public Nullable<int> ModelParameterID { get; set; }

        public IEnumerable<BusinessUnits> BusinessUnits { get; set; }


    }

    public class BusinessUnits
    {
       

        public string Name { get; set; }
        //MDFBySellout
        public Nullable<decimal> MDF { get; set; }
        //AdditionalCalculatedMDF
        public Nullable<decimal> AdditionalMDF { get; set; }
        //Reason
        //Sellout

    }

    public class UpdateRound2
    {

        public long PartnerBUBudgetID { get; set; }
        public long PartnerBudgetID { get; set; }
        public short BusinessUnitID { get; set; }
        public int MDFVarianceReasonID { get; set; }
        public int FocusedAreaID { get; set; }
        public Nullable<decimal> Baseline_MDF { get; set; }
        public string Comments { get; set; }
        public Nullable<decimal> Additional_RecommendedMDF { get; set; }
        public Nullable<decimal> Additional_MDF { get; set; }
        public string Additional_MDF_Reason { get; set; }
        public string UserID { get; set; }
    }
        public partial class PartnerBUBudgetVM
    {
        public long PartnerBUBudgetID { get; set; }
        public long PartnerBudgetID { get; set; }
        public short BusinessUnitID { get; set; }
        public int MDFVarianceReasonID { get; set; }
        public int FocusedAreaID { get; set; }
        public Nullable<decimal> Baseline_MDF { get; set; }
        public string Comments { get; set; }
        public Nullable<decimal> Additional_RecommendedMDF { get; set; }
        public Nullable<decimal> Additional_MDF { get; set; }
        public string Additional_MDF_Reason { get; set; }

        //public virtual BusinessUnit BusinessUnit { get; set; }
        //public virtual MDFVarianceReason MDFVarianceReason { get; set; }
        //public virtual PartnerBudget PartnerBudget { get; set; }
    }

}