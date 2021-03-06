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
    
    public partial class MDFPlanning
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MDFPlanning()
        {
            this.BUBudgets = new HashSet<BUBudget>();
            this.ModelOutputSummaryTables = new HashSet<ModelOutputSummaryTable>();
            this.ModelOutputTables = new HashSet<ModelOutputTable>();
            this.ModelParameterTables = new HashSet<ModelParameterTable>();
            this.PartnerBudgets = new HashSet<PartnerBudget>();
            this.ModelDefaultOutputTables = new HashSet<ModelDefaultOutputTable>();
        }
    
        public int ID { get; set; }
        public int VersionNo { get; set; }
        public string VersionName { get; set; }
        public int FinancialYearID { get; set; }
        public int PartnerTypeID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<bool> Flag { get; set; }
        public Nullable<bool> IsFinal { get; set; }
        public string AllocationLevel { get; set; }
        public string CountryOrGeoOrDistrict { get; set; }
        public Nullable<int> MembershipGroupID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BUBudget> BUBudgets { get; set; }
        public virtual FinancialYear FinancialYear { get; set; }
        public virtual User User { get; set; }
        public virtual PartnerType PartnerType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ModelOutputSummaryTable> ModelOutputSummaryTables { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ModelOutputTable> ModelOutputTables { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ModelParameterTable> ModelParameterTables { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PartnerBudget> PartnerBudgets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ModelDefaultOutputTable> ModelDefaultOutputTables { get; set; }
    }
}
