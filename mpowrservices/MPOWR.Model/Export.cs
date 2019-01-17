using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPOWR.Model
{
    public class Export
    {
        public class Export_SDFC
        {
            public string FundID { get; set; }
            public string BudgetID { get; set; }
            public string BudgetName { get; set; }
            public string Description { get; set; }
            public string IsPanHPEFund { get; set; }
            public string partner_name { get; set; }
            public string LocatorID { get; set; }
            public string BusinessRelationshiptype { get; set; }
            public string AllocationApprover { get; set; }
            public string UploadCurrency { get; set; }
            public decimal AllocationAmount { get; set; }
            public string IsMSA { get; set; }
            public string Internalcomments { get; set; }
            public string BusinessRelationship { get; set; }
            public string IsARUBAMSA { get; set; }
            public decimal? WMGORatio { get; set; }
            public string HQID { get; set; }
          
        }
        public class Export_PartnerBudget
        {
            public string Region { get; set; }
            public string Country { get; set; }
            public string Budget { get; set; }
            public string Partner_name { get; set; }
            public string Membership_Type { get; set; }
            public string Business_Unit { get; set; }
            public decimal? projected_sellout { get; set; }
            public decimal? Last_Period_Sellout { get; set; }
            public decimal? YoY_change_sellout { get; set; }
            public string Prediction_Accuracy { get; set; }
            public decimal? RecommendedMDF { get; set; }
            public string PREV_MDF_Assessment { get; set; }
            public string MDF_Alignment { get; set; }
            public decimal? Last_Period_MDF { get; set; }
            public decimal? YoY_change_MDF { get; set; }
            public decimal? AllocatedMDF { get; set; }
            public string Reason { get; set; }
            public decimal? Last_Period_Productivity { get; set; }
            public decimal? Projected_Productivity { get; set; }
            public decimal? Median_Avg_MDF_Sellout { get; set; }
            public decimal? ProductivityImprovement { get; set; }
            public string MSA { get; set; }
            public string PartnerID { get; set; }
            public decimal? SOW { get; set; }
            public decimal MGO { get; set; }
            public decimal W_MGO { get; set; }
            public decimal MGO_ROI { get; set; }

        }

        //HPEM-458 - Import from excel changes start
        public class BudgetAllocationSummary
        {
            public string BusinessUnit { get; set; }
            public decimal? Allocated { get; set; }
            public decimal? Remaining { get; set; }
            public decimal? Total { get; set; }
           
        }
        public class BudgetsummaryWithR1R2: BudgetAllocationSummary
        {
           
            public decimal? Round1MDF { get; set; }
            public decimal? Round2MDF { get; set; }
        }
        //HPEM-458- Import from excel changes end
    }
}
