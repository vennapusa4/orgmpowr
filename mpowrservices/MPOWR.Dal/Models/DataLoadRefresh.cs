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
    
    public partial class DataLoadRefresh
    {
        public int ID { get; set; }
        public Nullable<int> ApplicationID { get; set; }
        public string Geo { get; set; }
        public Nullable<bool> Sellout { get; set; }
        public Nullable<bool> MDFHistory { get; set; }
        public Nullable<bool> Planned_ProjectedSales { get; set; }
        public string Quarter { get; set; }
        public Nullable<System.DateTime> LatestDataRefresh { get; set; }
        public string MailSentDate { get; set; }
        public Nullable<System.DateTime> MailDispalyDate { get; set; }
        public string FilePath { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsPopup { get; set; }
        public Nullable<int> PopupDays { get; set; }
        public Nullable<System.DateTime> MPOWR_RefreshDate { get; set; }
    }
}
