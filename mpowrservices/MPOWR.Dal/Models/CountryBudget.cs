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
    
    public partial class CountryBudget
    {
        public long CountryBudgetID { get; set; }
        public Nullable<short> CountryID { get; set; }
        public Nullable<int> FinancialYearID { get; set; }
        public Nullable<decimal> MDF { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual Country Country { get; set; }
        public virtual FinancialYear FinancialYear { get; set; }
    }
}
