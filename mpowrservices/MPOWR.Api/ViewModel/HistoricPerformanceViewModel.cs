using MPOWR.Dal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MPOWR.Api.ViewModel
{
    public class HistoricPerformanceViewModel
    {

        public List<Historical> Historicdata { get; set; }
         public List<MDFCollection> MDFData { get; set; }
        public long PartnerSalesID { get; set; }
        public Nullable<short> RegionID { get; set; }
        public string Region { get; set; }
        public Nullable<short> SubRegionID { get; set; }
        public string SubRegion { get; set; }
        public Nullable<short> CountryID { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string PartnerID { get; set; }
        public string PartnerName { get; set; }
        public Nullable<short> ParnterTypeID { get; set; }
        public string ParnterType { get; set; }
        public Nullable<short> MembershipTypeID { get; set; }
        public string Membership { get; set; }
        public string BG { get; set; }
        public Nullable<short> BusinessUnitID { get; set; }
        public string BusinessUnit { get; set; }
        public Nullable<short> FinancialYearID { get; set; }
        public string FinancialYear { get; set; }
        public Nullable<decimal> Planned_1H { get; set; }
        public Nullable<decimal> Planned_2H { get; set; }
        public Nullable<decimal> Quota_1H { get; set; }
        public Nullable<decimal> Quota_2H { get; set; }
        public Nullable<decimal> MDF_1H { get; set; }
        public Nullable<decimal> MDF_2H { get; set; }
        public Nullable<decimal> SellOut_Amount_Q1 { get; set; }
        public Nullable<decimal> SellOut_Amount_Q2 { get; set; }
        public Nullable<decimal> SellOut_Amount_Q3 { get; set; }
        public Nullable<decimal> SellOut_Amount_Q4 { get; set; }
        public Nullable<decimal> SellOut_Q1 { get; set; }
        public Nullable<decimal> SellOut_Q2 { get; set; }
        public Nullable<decimal> SellOut_Q3 { get; set; }
        public Nullable<decimal> SellOut_Q4 { get; set; }
        public Nullable<decimal> SellOut_1H { get; set; }
        public Nullable<decimal> SellOut_2H { get; set; }
        public Nullable<decimal> SOB_Q1 { get; set; }
        public Nullable<decimal> SOB_Q2 { get; set; }
        public Nullable<decimal> SOB_Q3 { get; set; }
        public Nullable<decimal> SOB_Q4 { get; set; }
        public Nullable<decimal> SOB_1H { get; set; }
        public Nullable<decimal> SOB_2H { get; set; }
        public Nullable<decimal> SellOutNet_Q1 { get; set; }
        public Nullable<decimal> SellOutNet_Q2 { get; set; }
        public Nullable<decimal> SellOutNet_Q3 { get; set; }
        public Nullable<decimal> SellOutNet_Q4 { get; set; }
        public Nullable<decimal> SellOutForPlatinumAndGold_Q1 { get; set; }
        public Nullable<decimal> SellOutForPlatinumAndGold_Q2 { get; set; }
        public Nullable<decimal> SellOutForPlatinumAndGold_Q3 { get; set; }
        public Nullable<decimal> SellOutForPlatinumAndGold_Q4 { get; set; }
        public Nullable<decimal> SellOutForPlatinumAndGold_1H { get; set; }
        public Nullable<decimal> SellOutForPlatinumAndGold_2H { get; set; }
        public Nullable<decimal> SellOutForSilverAndBelow_Q1 { get; set; }
        public Nullable<decimal> SellOutForSilverAndBelow_Q2 { get; set; }
        public Nullable<decimal> SellOutForSilverAndBelow_Q3 { get; set; }
        public Nullable<decimal> SellOutForSilverAndBelow_Q4 { get; set; }
        public Nullable<decimal> SellOutForSilverAndBelow_1H { get; set; }
        public Nullable<decimal> SellOutForSilverAndBelow_2H { get; set; }
        public Nullable<decimal> PBM { get; set; }
        public Nullable<decimal> PMM { get; set; }
        public Nullable<decimal> Ranking { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserType { get; set; }


    }

    public class MDFCollection
    {
        public decimal? Total { get; set; }
        public decimal? SELLOUTQ1 { get; set; }
        public decimal? SELLOUTQ2 { get; set; }
        public decimal? SELLOUTQ3 { get; set; }
        public decimal? SELLOUTQ4 { get; set; }
        public decimal? MDF_1H { get; set; }
        public decimal? MDF_2H { get; set; }
        public Nullable<decimal> SellOut_1H { get; set; }
        public Nullable<decimal> SellOut_2H { get; set; }

        public Nullable<short> BusinessUnitID { get; set; }
        public string PartnerName { get; set; }
        public string BusinessUnit { get; set; }
        public string FinancialYear { get; set; }
        public string SellOut { get; set; }
        public string MDF { get; set; }
       
        public string BGMembership { get; set; }
    }
    public class Historical
    {
        public string BGMembership { get; set; }
     
        public decimal? SellOutForPlatinumAndGold { get; set; }
        public decimal? SellOutForSilverAndBelow { get; set; }
        public decimal? LastPeriodSellout { get; set; }
        public decimal? LastPeriodSelloutGrowth { get; set; }
        public decimal? LastPeriodSelloutGrowthPer { get; set; }
        public decimal? LastPeriodMDF { get; set; }
        public decimal? OverallROI { get; set; }
        public IEnumerable<MDFCollection> rows { get; set; }
        public string PartnerName { get; set; }
        public string Name { get; set; }
        //public decimal ShareOfWalletGrowth { get; set; }
        //public decimal FootprintGrowth { get; set; }
    }

    public class HistoricalVM
    {
        public int? Year { get; set; }
        public int? CountryID { get; set; }
        public int? PartnerTypeID { get; set; }
        public int? DistrictID { get; set; }
        public bool MDF { get; set; }
        public bool NotMDF { get; set; }
        public string BUs { get; set; }
        public int? VersionID { get; set; }

    }
    public class Businessunits
    {
        public int? ID { get; set; }
       public string Name { get; set; }
}
}