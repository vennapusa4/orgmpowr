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
    
    public partial class UserBusinessUnit
    {
        public int UserBusinessUnitID { get; set; }
        public Nullable<int> UserRoleUserTypeID { get; set; }
        public short BusinessUnitID { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual BusinessUnit BusinessUnit { get; set; }
        public virtual UserRoleUserType UserRoleUserType { get; set; }
    }
}
