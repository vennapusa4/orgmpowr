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
    
    public partial class MilestoneNotificationsRole
    {
        public int MilestoneNotificationRoleID { get; set; }
        public int MilestoneNotificationID { get; set; }
        public int RoleID { get; set; }
        public string RoleOperation { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual MilestoneNotification MilestoneNotification { get; set; }
        public virtual Role Role { get; set; }
    }
}