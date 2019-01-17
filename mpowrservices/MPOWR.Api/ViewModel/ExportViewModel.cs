using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPOWR.Api.ViewModel
{
    public class ExportViewModel
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
            public string HQID { get; set; }
        }

        public class Export_PartnerBudget
        {

            public string Partner_name { get; set; }
            public decimal? MGO_ROI { get; set; }
            public string Membership_Type { get; set; }
            public string Business_Unit { get; set; }
            public decimal? Last_Period_Sellout { get; set; }
            public decimal? Sellout_Silver_Below { get; set; }
            public decimal? Sellout_Gold_Platinum { get; set; }
            public decimal? Unweighted_Sellout { get; set; }
            public decimal? YoY_Sellout { get; set; } //YoY(1H17 vs 1H16)  // previous year vs perios to previous year 
            public decimal? Scaled_Sellout { get; set; }
            public decimal? HOH_SellOut { get; set; } //HoH(2H17 vs 1H17) // previous period  vs previous year
            public decimal? projected_sellout { get; set; }
            public string Source_of_sellout { get; set; }//Source of 1H18 Projected Sellout
            public decimal? YOY_Currenty_PreviousY { get; set; } //YoY(1H18 Projected vs 1H17)
            public decimal? RecommendedMDF { get; set; }
            public decimal? Last_Year_MDF { get; set; }
            public decimal? MSA { get; set; }
            public decimal? ARUBAMSA { get; set; }
            public decimal? Last_Period_MDF { get; set; }
            public decimal? AllocatedMDFR1 { get; set; }
            public decimal? AllocatedMDFR2 { get; set; }
            public decimal? MDF_CurrentY_PreviousY_Delta { get; set; }  //1H18 vs 1H17 MDF Delta
            public decimal? YoY_change_MDF { get; set; }
            public decimal? HOH_CurrentP_previousP { get; set; } //HoH(1H18 vs 2H17)
            public decimal? Last_Period_Productivity { get; set; }
            public decimal? previosp_MDF_vs_Sellpout { get; set; } //2H17 MDF/2H17 Sellout
            public decimal? Projected_Productivity { get; set; }
            public decimal? Median_Avg_MDF_Sellout { get; set; }
            public decimal? ProductivityImprovement { get; set; }
            public decimal? SOW { get; set; }
            public decimal? MGO { get; set; }
            public decimal? W_MGO { get; set; }
            public decimal? W_MGO_ROI { get; set; }
            public decimal? WMGORatio { get; set; }
            public string PartnerID { get; set; }
            public string Region { get; set; }
            public string Country { get; set; }
            public string Budget { get; set; }
            public string Prediction_Accuracy { get; set; }
            public string ProjectionMethod { get; set; }
            public string PREV_MDF_Assessment { get; set; }
            public string MDF_Alignment { get; set; }
            public string Reason { get; set; }
        }
        public class Export_PartnerBudget_USA
        {

            public string Partner_name { get; set; }
            public decimal? MGO_ROI { get; set; }
            public string Membership_Type { get; set; }
            public string Business_Unit { get; set; }
            public decimal? Last_Period_Sellout { get; set; }
            public decimal? YoY_Sellout { get; set; } //YoY(1H17 vs 1H16)  // previous year vs perios to previous year 
            public decimal? Scaled_Sellout { get; set; }
            public decimal? HOH_SellOut { get; set; } //HoH(2H17 vs 1H17) // previous period  vs previous year
            public decimal? projected_sellout { get; set; }
            public string Source_of_sellout { get; set; }//Source of 1H18 Projected Sellout
            public decimal? YOY_Currenty_PreviousY { get; set; } //YoY(1H18 Projected vs 1H17)
            public decimal? RecommendedMDF { get; set; }
            public decimal? Last_Year_MDF { get; set; }
            public decimal? MSA { get; set; }
            public decimal? ARUBAMSA { get; set; }
            public decimal? Last_Period_MDF { get; set; }
            public decimal? AllocatedMDFR1 { get; set; }
            public decimal? AllocatedMDFR2 { get; set; }
            public decimal? MDF_CurrentY_PreviousY_Delta { get; set; }  //1H18 vs 1H17 MDF Delta
            public decimal? YoY_change_MDF { get; set; }
            public decimal? HOH_CurrentP_previousP { get; set; } //HoH(1H18 vs 2H17)
            public decimal? Last_Period_Productivity { get; set; }
            public decimal? previosp_MDF_vs_Sellpout { get; set; } //2H17 MDF/2H17 Sellout
            public decimal? Projected_Productivity { get; set; }
            public decimal? Median_Avg_MDF_Sellout { get; set; }
            public decimal? ProductivityImprovement { get; set; }
            public decimal? SOW { get; set; }
            public decimal? MGO { get; set; }
            public decimal? W_MGO { get; set; }
            public decimal? W_MGO_ROI { get; set; }
            public decimal? WMGORatio { get; set; }
            public string PartnerID { get; set; }
            public string Region { get; set; }
            public string Country { get; set; }
            public string District { get; set; }
            public string Budget { get; set; }
            public string Prediction_Accuracy { get; set; }
            public string ProjectionMethod { get; set; }
            public string PREV_MDF_Assessment { get; set; }
            public string MDF_Alignment { get; set; }
            public string Reason { get; set; }
          //  public decimal? YoY_change_sellout { get; set; }//11 currently not there in excel
           
        }

        //HPEM-457 - Export to excel changes start
        public class BudgetAllocationSummary 
        {
            public string BusinessUnit { get; set; }
            public decimal Allocated { get; set; }
            public decimal Remaining { get; set; }
            public decimal Total { get; set; }


        }
        //HPEM-457 - Export to excel changes end

    }
}