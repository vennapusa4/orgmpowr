using System;
using System.Collections.Generic;

namespace MPOWR.Model
{
    /// <summary>
    /// Properties required for creating json 
    /// </summary>
    public class VersionMDF : Versions
    {
        public string FinancialPeriod { get; set; }
        public string CountryName { get; set; }
        public int GeoID { get; set; }
        public string CountryID { get; set; }
        public string CountryOrGeoOrDistrict { get; set; }
        public string AllocationLevel { get; set; }
        public string DistrictID { get; set; }
        public int FinancialYearID { get; set; }
        public int PartnerTypeID { get; set; }
        public int? MembershipID { get; set; }
        //public string Membership { get; set; }
        //  public DateTime CreatedDate { get; set; }
        public decimal? TotalMDF { get; set; }
        public decimal? BaselineMDF { get; set; }
        public decimal? ProgramMDFCaveOuts { get; set; }
        public decimal? CountryReverseMDF { get; set; }
        public bool Flag { get; set; }
        public string UserID { get; set; }


    }

    public class NewVersionAdd :Versions
    {
        public decimal? TotalMDF { get; set; }
        public decimal? BaselineMDF { get; set; }
        public decimal? ProgramMDFCaveOuts { get; set; }
        public decimal? CountryReverseMDF { get; set; }
        public string FinancialPeriod { get; set; }
        public int FinancialYearID { get; set; }
        public bool isEnable { get; set; }
        public string BusinessUnitID { get; set; }
        public string BusinessUnit { get; set; }
        public bool buflag { get; set; }
        public IEnumerable<int> business { get; set; }
        public int? MembershipID { get; set; }
        public bool disabled { get; set; }
        //public string Membership { get; set; }
    }
    public class GetFinancial : GEO
    {
        public IEnumerable<Versions> Version { get; set; }

    }
    public class Versions
    {
        public int? VersionID { get; set; }
        public int VersionNo { get; set; }
        public string VersionName { get; set; }
        public bool? IsFinal { get; set; }
        public bool? IsAllFinancialYear { get; set; }
        public int? MembershipGroupID { get; set; }
        public string MembershipName { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class CopyVersion :GEO
    {
        public int OldFinancialyearID { get; set; }
        public int OldVersionNo { get; set; }
        public int NewFinancialyearID { get; set; }
        public int NewVersionNo { get; set; }
        public string VersionName { get; set; }
        public int? VersionID { get; set; }
        public int? NewVersionID { get; set; }


    }
    public class GEO
    {
        public short RegionID { get; set; }
        public short CountryID { get; set; }
        public short? DistrictID { get; set; }
        public string CountryOrGeoOrDistrict { get; set; }
        public string AllocationLevel { get; set; }
        public int PartnerTypeID { get; set; }
        public int FinancialyearID { get; set; }
        public string Financialyear { get; set; }
        public int? MembershipGroupID { get; set; }
        public string UserID { get; set; }
        public bool IsFinal { get; set; }
        public bool? IsActive { get; set; }
    }

}
