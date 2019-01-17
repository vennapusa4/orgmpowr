using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPOWR.Api.ViewModel
{
    public class GuardRailsViewModel
    {
        public short RegionID { get; set; }
        public int FinancialYearID { get; set; }
        public string user { get; set; }
        public GuardRailDetail[] GuardRailDetails { get; set; }       
    }
    public class GuardRailDetail
    {
        public int GuardRailConfigID { get; set; }
        public int GuardRailFYConfigID { get; set; }
        public decimal ProgramCarveOutComplaintValue { get; set; }
        public decimal ProgramCarveOutNonComplaintValue { get; set; }
        public decimal ActualMDFAllocationComplaintValue { get; set; }
        public decimal ActualMDFAllocationNonComplaintValue { get; set; }
        public decimal MDFSelloutIndexComplaintValue { get; set; }
        public decimal MDFSelloutIndexNonComplaintValue { get; set; }
        public decimal OverAllocationComplaintValue { get; set; }
        public decimal OverAllocationNonComplaintValue { get; set; }
        public string AllowOverAllocation { get; set; }
        public decimal UnderAllocationComplaintValue { get; set; }
        public decimal UnderAllocationNonComplaintValue { get; set; }
        public int ReviewerRoleID1 { get; set; }
        public int ReviewerRoleID2 { get; set; }
        public int ReviewerRoleID3 { get; set; }
        public int ReviewerRoleID4 { get; set; }        
    }
}