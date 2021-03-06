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
    
    public partial class ModelParameterConfig
    {
        public int ModelParameterConfigID { get; set; }
        public int ModelParameterFYConfigID { get; set; }
        public decimal HighPerformerProductivity { get; set; }
        public decimal MediumPerformerProductivityRatio { get; set; }
        public decimal LowPerformerProductivityRatio { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual ModelParameterFYConfig ModelParameterFYConfig { get; set; }
    }
}
