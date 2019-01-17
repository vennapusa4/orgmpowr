using MPOWR.Dal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MPOWR.Api.ViewModel
{
    public class BUBdgetsViewModel
    {
        public short? DistrictID { get; set; }
        public Nullable<short> CountryID { get; set; }
        public Nullable<short> BusinessUnitID { get; set; }
        public Nullable<int> FinancialYearID { get; set; }
        public Nullable<decimal> TotalMDF { get; set; }
        public Nullable<decimal> ProgramMDF { get; set; }
        public Nullable<decimal> CountryReserveMDF { get; set; }
        public Nullable<decimal> BaselineMDF { get; set; }
        public long BUBudgetID { get; set; }
        public Nullable<decimal> ProjectMDF { get; set; }
        public int ProgramMDFID { get; set; }
        public string BusinessUnitName { get; set; }
        public string CountryName { get; set; }
        public string PartnerName { get; set; }

        public Nullable<int> PartnerTypeID { get; set; }
        public ProgramMDFS[] CarveProjects { get; set; }
        public BUBudgets[] BuUnits { get; set; } 
        public string FinancialYear { get; set; }
        public string UserID { get; set; }
        public string Status { get; set; }
        public int VersionID { get; set; }

    }

    public class BUBudgets
    {
        public long BUBudgetID { get; set; }
        public short? BusinessUnitID { get; set; }
        public decimal? TotalMDF { get; set; }
        public decimal? CountryReserveMDF { get; set; }
        public decimal? BaselineMDF { get; set; }
        public string BusinessUnitName { get; set; }

        public string Status { get; set; }
        public decimal? ProgramMDF { get; set; }
        public IEnumerable<ProgramMDFS> CarveProjects { get; set; }
        public string HasAccess { get; set; }
        public bool IsOverAllocation { get; set; }
    }
    public class ProgramMDFS
    {
        public int ProgramMDFID { get; set; }
        public string ProjectName { get; set; }
        public decimal? ProjectMDF { get; set; }
        public string Flag { get; set; }
}
    public class CountryBudgets
    {
        public int FinancialYearID { get; set; }
        public string FinancialYear { get; set; }
        public int PartnerTypeID { get; set; }
        public string PartnerName { get; set; }
        public short CountryID { get; set; }
        public string CountryName { get; set; }

        public IEnumerable<BUBudgets> BuUnits { get; set; }
        public bool IsOverAllocation { get; set; }
    }

}