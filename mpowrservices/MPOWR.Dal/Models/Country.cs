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
    
    public partial class Country
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Country()
        {
            this.CountryBudgets = new HashSet<CountryBudget>();
            this.Districts = new HashSet<District>();
            this.PartnerSalesRefreshHistories = new HashSet<PartnerSalesRefreshHistory>();
            this.UserCountries = new HashSet<UserCountry>();
            this.ContraWeb_Upload_CountryMapping = new HashSet<ContraWeb_Upload_CountryMapping>();
        }
    
        public short CountryID { get; set; }
        public string ShortName { get; set; }
        public string DisplayName { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<short> GeoID { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string TypeofSellout { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CountryBudget> CountryBudgets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<District> Districts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PartnerSalesRefreshHistory> PartnerSalesRefreshHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserCountry> UserCountries { get; set; }
        public virtual Geo Geo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContraWeb_Upload_CountryMapping> ContraWeb_Upload_CountryMapping { get; set; }
    }
}
