using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPOWR.Api.ViewModel
{
    public class PartnerBudgetViewModel
    {
        public string ID { get; set; }
        public string Partner_Name { get; set; }
        public string Membership_Type { get; set; }
        public int MembershipTypeID { get; set; }
        public string MDF_Alignment { get; set; }

        public IEnumerable<BusinessUnitViewModel> BU { get; set; }
    }

    public class PartnerBUBudgetViewModel
    {
        public long PartnerBudgetID { get; set; }
        public long PartnerBUBudgetID { get; set; }
        public string UserID { get; set; }
        public Nullable<int> VersionID { get; set; }
        public Nullable<decimal> Baseline_MDF { get; set; }
        public Nullable<decimal> CarveOut { get; set; }
        public Nullable<decimal> BU_MSA { get; set; }
        public Nullable<decimal> TotalMDF { get; set; }
    }


    public class Graph
    {
        public int CountryID { get; set; }
        public int PartnerTypeID { get; set; }
        public int DistrictID { get; set; }
        public int FinancialYearID { get; set; }
        public int BusinessUnitID { get; set; }
        public string Type { get; set; }
        public int Previous_Period_Year { get; set; }
        public int VersionID { get; set; }
        // public int MemberShipTypeID { get; set; }
    }


    public class BusinessUnitViewModel
    {
        public string Business_Unit { get; set; }
        public short BBusinessUnitID { get; set; }
        public IEnumerable<SelloutViewModel> Sellout { get; set; }
    }

    public class SelloutViewModel
    {
        public Nullable<decimal> Last_Period_Sellout { get; set; }
        public Nullable<decimal> Projected_Sellout { get; set; }
        public Nullable<decimal> YoY_change_sellout { get; set; }
    }

    public class ModelOutputTableViewModel
    {
        public string PartnerID { get; set; }
        public Nullable<short> BusinessUnitID { get; set; }
        public Nullable<int> ModelParameterID { get; set; }
        public string Id { get; set; }
        public string Partner_Name { get; set; }
        public string Membership_Type { get; set; }
        public string Allignment { get; set; }
        public string Business_Unit { get; set; }

        public IEnumerable<BUDetails> BU { get; set; }
        public IEnumerable<MSA> MSA { get; set; }

    }

    public class PartnerDetails
    {
        public string PartnerID { get; set; }
        public string Partner_Name { get; set; }
        public string Membership_Type { get; set; }
        public IEnumerable<MSA> MSA { get; set; }
        public IEnumerable<BUDetails> BU { get; set; }

    }

    //public class PatnerBusinessUnitDetails
    //{
    //    public Nullable<short> BusinessUnitID { get; set; }
    //    public Nullable<int> ModelParameterID { get; set; }
    //    public string Id { get; set; }
    //    public string Allignment { get; set; }
    //    public string Business_Unit { get; set; }
    //}

    public class PartnersCompleteList
    {
        //public IList<ModelOutputTableViewModel> ModeloutputTableViewModel { get; set; }
        public IList<PartnerDetails> PartnerDetailsList { get; set; }
        public TotalofPartners TotalofPartners { get; set; }
    }
    public class PartBU
    {
        public string ID { get; set; }
        public string Partner_Name { get; set; }
        public string Membership_Type { get; set; }
        public int MembershipTypeID { get; set; }
        public string Allignment { get; set; }
        public string Business_Unit { get; set; }

        public IEnumerable<BUDetails> BU { get; set; }
    }

    public class BUDetails
    {
        public Nullable<short> BusinessUnitID { get; set; }

        public long PartnerBudgetID { get; set; }
        public string Business_Unit { get; set; }
        public IEnumerable<SellOut> Sellout { get; set; }
        public IEnumerable<MDF> MDF { get; set; }
        public IEnumerable<MDFOrSellout> MDFOrSellout { get; set; }
        public IEnumerable<Analysis> Analysis { get; set; }

        public IEnumerable<HistoryMDF> HistoryMDF { get; set; }
        public IEnumerable<HistorySellout> HistorySellout { get; set; }
        //public IEnumerable<MSA> MSA { get; set; }

        public string PBM { get; set; }
        public string PMM { get; set; }
        //RankingAmongPeers
        //SellIn
        //SellInGrowth
        //DeltaBetweenSellInAndSellOut
        //PreviousPeriodSellOutGrowth
        public Nullable<decimal> PlannedSales { get; set; }
        public Nullable<decimal> targetAchievement { get; set; }
        public Nullable<decimal> SOW { get; set; }
        public Nullable<decimal> SOWGrowth { get; set; }
        public Nullable<decimal> FootprintGrowth { get; set; }
        public Nullable<decimal> No_of_end_customers { get; set; }
        public Nullable<decimal> Total_MDF { get; set; }
        public Nullable<decimal> Incremental_MDF { get; set; }
        public Nullable<decimal> Late_MDF { get; set; }
        public Nullable<decimal> W_MGO_Marketing_MDF { get; set; }
        public Nullable<decimal> New_Logos_MGO { get; set; }
    }
    public class SellOut
    {
        public Nullable<decimal> Last_Period_Sellout { get; set; }
        public Nullable<decimal> Projected_Sellout { get; set; }
        public Nullable<decimal> YoY_change_sellout { get; set; }
        public short BusinessUnitID { get; set; }
    }

    public class MDF
    {
        public Nullable<long> PartnerBudgetID { get; set; }

        public Nullable<short> BusinessUnitID { get; set; }

        public long PartnerBUBudgetID { get; set; }
        public Nullable<decimal> Last_Period_MDF { get; set; }
        public Nullable<decimal> Recommended_MDF { get; set; }
        public Nullable<decimal> YoY_change_MDF { get; set; }
        public Nullable<decimal> BaseLineMDF { get; set; }
        public string ReasonForVariance { get; set; }
        public string Comment { get; set; }
        public string Additional_MDF_Reason { get; set; }
        public int MDFVarianceReasonID { get; set; }
        public int FocusedAreaID { get; set; }
        public Nullable<decimal> AdditionalMdf { get; set; }
        public Nullable<decimal> Additional_Reccommended_MDF { get; set; }
    }

    //public class DecisionMDF
    //{
    //    public short CountryID { get; set; }
    //    public int PartnerTypeID { get; set; }
    //    public string PartnerID { get; set; }
    //    public short BusinessUnitID { get; set; }
    //    public Nullable<decimal> BaseLineMDF { get; set; }
    //    public string ReasonForVariance { get; set; }
    //    public string Comment { get; set; }
    //}

    public partial class DecisionMDF
    {
        public long PartnerBUBudgetID { get; set; }
        public long PartnerBudgetID { get; set; }
        public short BusinessUnitID { get; set; }
        public int MDFVarianceReasonID { get; set; }
        public int FocusedAreaID { get; set; }
        public Nullable<decimal> Baseline_MDF { get; set; }
        public string Comments { get; set; }
        public Nullable<decimal> Additional_RecommendedMDF { get; set; }
        public Nullable<decimal> Additional_MDF { get; set; }
        public string Additional_MDF_Reason { get; set; }
        public string UserID { get; set; }
        public int VersionID { get; set; }
        public Nullable<decimal> BU_MSA { get; set; }
        public Nullable<decimal> TotalMDF { get; set; }

    }

    public class MDFOrSellout
    {
        public Nullable<decimal> LastYearMDFOrSellout { get; set; }
        public Nullable<decimal> ProjectedMDFOrSellout { get; set; }
        public Nullable<decimal> MedianAvdMDFOrSellout { get; set; }
        public Nullable<decimal> ProductivityImprovementPer { get; set; }

    }

    public class Analysis
    {
        public string MDFAllignment { get; set; }
        public string AssesmentOfLastYearMDF { get; set; }
        public string PredictionAccuracy { get; set; }
    }

    public class HistoryMDF
    {
        public string FinancialYear { get; set; }
        public Nullable<decimal> MDF_1H { get; set; }
        public Nullable<decimal> MDF_2H { get; set; }
    }

    public class HistorySellout
    {
        public string FinancialYear { get; set; }
        public Nullable<decimal> SellOut_Q1 { get; set; }
        public Nullable<decimal> SellOut_Q2 { get; set; }
        public Nullable<decimal> SellOut_Q3 { get; set; }
        public Nullable<decimal> SellOut_Q4 { get; set; }
    }

    public class MSA
    {
        public Nullable<decimal> MSAValue { get; set; }
        public Nullable<decimal> RecommendedMSA { get; set; }
        public long PartnerBudgetID { get; set; }
    }

    public class VarianceReasonsVM
    {
        public int MDFVarianceReasonID { get; set; }
        public string ShortName { get; set; }
        public string Reason { get; set; }
    }

    public class FocusAreasVM
    {
        public int FocusedAreaID { get; set; }
        public string ShortName { get; set; }
        public string FocusedArea { get; set; }
        public bool IsActive { get; set; }
    }

    public class TotalofPartners
    {
        public SellOut SellOut { get; set; }
        public MDF MDF { get; set; }

        public MDFOrSellout MDFOrSellOut { get; set; }

        public HistoryMDF HistoryMDF { get; set; }
        public HistorySellout HistorySellOut { get; set; }
        public decimal? PlannedSales { get; set; }
        public decimal? targetAchievement { get; set; }
        public decimal? SOW { get; set; }
        public decimal? SOWGrowth { get; set; }
        public decimal? FootprintGrowth { get; set; }
        public decimal? No_of_end_customers { get; set; }
        public decimal? Total_MDF { get; set; }
        public decimal? Incremental_MDF { get; set; }
        public decimal? Late_MDF { get; set; }
        public decimal? W_MGO_Marketing_MDF { get; set; }
        public decimal? New_Logos_MGO { get; set; }
    }


    public class Filters
    {
        public int FinancialyearID { get; set; }
        public short CountryID { get; set; }
        public int PartnerTypeID { get; set; }
        public string Financialyear { get; set; }
        public int DistrictID { get; set; }
        public int businessUnitID { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public int VersionID { get; set; }
        public string UserID { get; set; }
        public bool WithoutHistory { get; set; }
        public string FilterColumn { get; set; }
        public string FilterDelimeter { get; set; }
        public string FilterValue { get; set; }
    }


    public class Round2Filter
    {
        public short CountryID { get; set; }
        public int PartnerTypeID { get; set; }
        public int FinancialYearID { get; set; }
        public int District_ID { get; set; }       
        public int FocusedAreaID { get; set; }
        public int VersionID { get; set; }
        public string UserID { get; set; }

    }

    public class FilterColumns
    {
        public string ID { get; set; }

        public string FilterColumn { get; set; }
        public string TableColumn { get; set; }
        
    }
}