//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MPOWR.Dal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ModelOutputTable
    {
        public long ModelParamROutputID { get; set; }
        public Nullable<int> ModelParameterID { get; set; }
        public string PartnerID { get; set; }
        public string Partner_Name { get; set; }
        public string Membership_Type { get; set; }
        public string Business_Unit { get; set; }
        public Nullable<decimal> Last_Period_Sellout { get; set; }
        public Nullable<decimal> Projected_Sellout { get; set; }
        public Nullable<decimal> YoY_change_sellout { get; set; }
        public Nullable<decimal> Last_Period_MDF { get; set; }
        public Nullable<decimal> Recommended_MDF { get; set; }
        public Nullable<decimal> YoY_change_MDF { get; set; }
        public Nullable<decimal> Last_Period_Productivity { get; set; }
        public Nullable<decimal> Projected_Productivity { get; set; }
        public Nullable<short> BusinessUnitID { get; set; }
        public Nullable<decimal> Median_Avg_MDF_Sellout { get; set; }
        public string MDF_Alignment { get; set; }
        public string PREV_MDF_Assessment { get; set; }
        public Nullable<decimal> Planned_Sales { get; set; }
        public Nullable<decimal> Target_Achievement { get; set; }
        public Nullable<decimal> SOW { get; set; }
        public Nullable<decimal> SOW_Growth { get; set; }
        public string PBM { get; set; }
        public string PMM { get; set; }
        public Nullable<decimal> Footprint_Growth { get; set; }
        public Nullable<decimal> No_of_end_customers { get; set; }
        public Nullable<decimal> Total_MDF { get; set; }
        public Nullable<decimal> Incremental_MDF { get; set; }
        public Nullable<decimal> Late_MDF { get; set; }
        public Nullable<decimal> W_MGO_Marketing_MDF { get; set; }
        public Nullable<decimal> New_Logos_MGO { get; set; }
        public string Prediction_Accuracy { get; set; }
        public string Partner_Size { get; set; }
        public Nullable<decimal> High_Productivity_Value { get; set; }
        public Nullable<decimal> Additional_Recommended_MDF { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> DistrictID { get; set; }
        public Nullable<decimal> Growth_MDF { get; set; }
        public Nullable<decimal> Sustain_MDF { get; set; }
        public Nullable<decimal> CURR_HALF_Sellout { get; set; }
        public Nullable<short> CountryID { get; set; }
        public Nullable<short> FinancialYearID { get; set; }
        public Nullable<int> PartnerTypeID { get; set; }
        public Nullable<decimal> Avg_Productivity { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<decimal> Current_Period_MGO { get; set; }
        public Nullable<decimal> Current_Period_Won_MGO { get; set; }
        public Nullable<decimal> Current_Period_MGO_ROI { get; set; }
        public Nullable<decimal> Current_Period_Won_MGO_ROI { get; set; }
        public Nullable<decimal> Previous_Period_MGO { get; set; }
        public Nullable<decimal> Previous_Period_Won_MGO { get; set; }
        public Nullable<decimal> Previous_Period_MGO_ROI { get; set; }
        public Nullable<decimal> Previous_Period_Won_MGO_ROI { get; set; }
        public Nullable<decimal> Last_year_mdf { get; set; }
        public Nullable<decimal> Scaled_Sellout { get; set; }
        public Nullable<decimal> Sellout_Silver_Below { get; set; }
        public Nullable<decimal> Sellout_Gold_Platinum { get; set; }
        public string ProjectionMethod { get; set; }
        public Nullable<int> VersionID { get; set; }
        public Nullable<decimal> WMGO_Ratio { get; set; }
        public Nullable<decimal> MGO_Ratio { get; set; }
        public Nullable<decimal> Last_year_Sellout { get; set; }
        public Nullable<decimal> Prev_To_Prv_Year_sellout { get; set; }
        public string Source_of_SellOut { get; set; }
        public Nullable<bool> HistoryExists { get; set; }
    
        public virtual MDFPlanning MDFPlanning { get; set; }
    }
}
