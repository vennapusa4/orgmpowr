using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MPOWR.Dal.Models;


namespace MPOWR.Api.ViewModel
{
    public class ModelOutput
    {
        public int ModelParamROutputID { get; set; }
        public Nullable<int> ModelParameterID { get; set; }
        public string PartnerID { get; set; }
        public string Partner_Name { get; set; }
        public string Membership_Type { get; set; }
        public string Business_Unit { get; set; }
        public Nullable<int> MDFVarianceReasonID { get; set; }
        public Nullable<decimal> Last_Period_Sellout { get; set; }
        public Nullable<decimal> Projected_Sellout { get; set; }
        public Nullable<decimal> YoY_change_sellout { get; set; }
        public Nullable<decimal> Last_Period_MDF { get; set; }
        public Nullable<decimal> Recommended_MDF { get; set; }
        public Nullable<decimal> YoY_change_MDF { get; set; }
        public Nullable<decimal> Last_Period_Productivity { get; set; }
        public Nullable<decimal> Projected_Productivity { get; set; }

        //blic virtual MDFVarianceReason MDFVarianceReason { get; set; }
        public virtual ModelParameterTable ModelParameterTable { get; set; }
        public virtual ModelParameterTable ModelParameterTable1 { get; set; }
    }
}