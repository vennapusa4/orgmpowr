using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MPOWR.Api.ViewModel
{
    public class MileStoneViewModel
    {
        public int Id { get; set; }
        public int MilestoneFYConfigID { get; set;}
        public int MilestoneNotificationId { get; set; }
        public short RegionId { get; set; }
        public int FinancialYearId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string RegionDetails { get; set; }
        public string FinancialYearDetails { get; set; }
        public DateTime? MilestoneDate { get; set; }
        public string Reminder1 { get; set; }
        public int? Period1 { get; set; }
        public bool? Status1 { get; set; }
        public string Reminder2 { get; set; }
        public int? Period2 { get; set; }
        public bool? Status2 { get; set; }
        public string Unit { get; set; }
        public IEnumerable<RoleVM> SendTo { get; set; }
        public IEnumerable<RoleVM> CopyTo { get; set; }
    }


    public class MileStoneViewModelGet
    {
        public int Id { get; set; }
        public int MilestoneFYConfigID { get; set; }
        public int MilestoneNotificationId { get; set; }
        public short RegionId { get; set; }
        public int FinancialYearId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string RegionDetails { get; set; }
        public string FinancialYearDetails { get; set; }
        public DateTime? MilestoneDate { get; set; }
        public string Reminder1 { get; set; }
        public int? Period1 { get; set; }
        public bool? Status1 { get; set; }
        public string Reminder2 { get; set; }
        public int? Period2 { get; set; }
        public bool? Status2 { get; set; }
        public string Unit { get; set; }
        public IEnumerable<RoleSetMilestone> SendTo { get; set; }
        public IEnumerable<RoleSetMilestone> CopyTo { get; set; }
    }

    public class SetMileStoneViewModel
    {
        //public RegionDetails RegionDetails { get; set; }
        // public FinancialYearDetails FinancialYearDetails { get; set; }
        public string RegionDetails { get; set; }
        public int RegionId { get; set; }
        public string FinancialYearDetails { get; set; }
        public int FinancialYearId { get; set; }
        public int MilestoneFYConfigID { get; set;}
        public DateTime MilestoneDate { get; set; }
        public RoleVM Roles1 { get; set; }
        public RoleVM Roles2 { get; set; }
        public List<RoleVM> allroles { get; set; }
        public IEnumerable<RoleVM> SendTo { get; set; }
        public IEnumerable<RoleVM> CopyTo { get; set; }
        // public List<RoleVM> allroles2 { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public string Reminder1 { get; set; }
        public int Period1 { get; set; }
        public bool Status1 { get; set; }
        public bool Status2 { get; set; }
        public string Reminder2 { get; set; }
        public int Period2 { get; set; }
      
    } 
    public class Milestone
    {
      
        public int MilestoneId { get; set; }
        public string ShortName { get; set; }
        public string DispalyName { get; set; }
        public string IsActive { get; set; }
        

    }
    public class RegionDetails
   {
        public int RegionId { get; set; }
        public string ShortName { get; set; }
        public string DispalyName { get; set; }
        //public dynamic RegionId { get; set; }
        //public dynamic ShortName { get; set; }
        //public dynamic DispalyName { get; set; }
    }
    public class FinancialYearDetails
    {
        public int FinancialYearID { get; set; }
        public string ShortName { get; set; }
        public string PeriodType { get; set; }
        public int Year { get; set; }
        //public dynamic FinancialYearID { get; set; }
        //public dynamic ShortName { get; set; }
        //public dynamic PeriodType { get; set; }
        //public dynamic Year { get; set; }

    }
    public class RoleVM
    {
        public int MilestoneNotificationRoleID { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        //public string ShortName { get; set; }
        //public string DispalyName { get; set; }
        public string primaryorsecondary { get; set; }
    }

    public class RoleSetMilestone
    {
        public int RoleID { get; set; }
        
    }

    public class RoleViewModel
    {
        public dynamic Roles { get; set; }
      
    }
    public class emaildata
    {
        public string to { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string attachment { get; set; }
    }
}