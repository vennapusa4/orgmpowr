using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPOWR.Api.ViewModel
{
    public class ModelParameterInputmodel
    {
        public ModelParametermodel Modelparameters { get; set; }
        public IEnumerable<ModelBUParametermodel> ModelBUparameters { get; set; }
    }

    public class ModelParametermodel
    {
        public int ModelParameterID { get; set; }
        //public Nullable<short> CountryID { get; set; }
        //public Nullable<short> DistrictID { get; set; }
        public Nullable<int> FinancialYearID { get; set; }
        public Nullable<int> PartnerTypeID { get; set; }
        public Nullable<decimal> Max_Sellout_HighDecline_Max_MDF { get; set; }
        public Nullable<decimal> Max_Sellout_HighDecline_Min_MDF { get; set; }
        public Nullable<decimal> Max_Sellout_ModerateDecline_Max_MDF { get; set; }
        public Nullable<decimal> Max_Sellout_ModerateDecline_Min_MDF { get; set; }
        public Nullable<decimal> Max_Sellout_Steady_Max_MDF { get; set; }
        public Nullable<decimal> Max_Sellout_Steady_Min_MDF { get; set; }
        public Nullable<decimal> Max_Sellout_ModerateGrowth_Max_MDF { get; set; }
        public Nullable<decimal> Max_Sellout_ModerateGrowth_Min_MDF { get; set; }
        public Nullable<decimal> Max_Sellout_HighGrowth_Max { get; set; }
        public Nullable<decimal> Max_Sellout_HighGrowth_Min { get; set; }
        public Nullable<decimal> Target_accomplish_HighPrecision_Score { get; set; }
        public Nullable<decimal> Target_accomplish_MediumPrecision_Score { get; set; }
        public Nullable<decimal> Max_Target_Accomplish_percentage { get; set; }
        public Nullable<decimal> Min_Target_Accomplish_percentage { get; set; }
        public Nullable<decimal> JPB_Max { get; set; }
        public Nullable<decimal> JPB_Min { get; set; }
        public Nullable<decimal> Prediction_High_Max { get; set; }
        public Nullable<decimal> Prediction_High_Min { get; set; }
        public Nullable<decimal> Prediction_Low_Max { get; set; }
        public Nullable<decimal> Prediction_Low_Min { get; set; }
        public Nullable<decimal> Weights_applied_t_1H { get; set; }
        public Nullable<decimal> Weights_applied_t_2H { get; set; }
        public Nullable<decimal> Weights_applied_t_3H { get; set; }
        public Nullable<decimal> Min_Target_Productivity { get; set; }
        public Nullable<decimal> Last_Quarter_Sellout_Scale_Factor { get; set; }
        public Nullable<decimal> partner_size_threshold { get; set; }
        public int VersionID { get; set; }
        public Nullable<decimal> WMGO_Perf_Acc_High { get; set; }
        public Nullable<decimal> WMGO_Perf_Acc_Low { get; set; }
        public Nullable<decimal> WMGO_Perf_Acc_VeryHigh { get; set; }
        public Nullable<decimal> WMGO_Perf_Acc_VeryLow { get; set; }
        public Nullable<decimal> WMGO_Perf_Acc_Medium { get; set; }
        public string UserID { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class ModelBUParametermodel
    {
        public int ModelBUParameterID { get; set; }
        public Nullable<int> ModelParameterID { get; set; }
        public Nullable<short> BusinessUnitID { get; set; }
        public Nullable<decimal> High_Performance { get; set; }
        public Nullable<decimal> Min_Partner_Investment { get; set; }
        public Nullable<decimal> New_Partner_RampUp_Scale { get; set; }
        public Nullable<decimal> Preferred_Partner_Cut_Off_Percentage { get; set; }
        public Nullable<decimal> Dist_cust_membership_weight_Silver_and_Below { get; set; }
        public Nullable<decimal> Dist_cust_membership_weight_Platinum_and_Gold { get; set; }
        public Nullable<decimal> Medium_Performance { get; set; }
        public Nullable<decimal> Low_Performance { get; set; }
        public Nullable<decimal> Growth_Revenue { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        //  public string HasAccess { get; set; }
    }

}
