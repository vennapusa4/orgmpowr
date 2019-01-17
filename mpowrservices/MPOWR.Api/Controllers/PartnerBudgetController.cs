using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MPOWR.Api.ViewModel;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Text;
using System.Data.Common;
using Newtonsoft.Json.Linq;
using MPOWR.Api.App_Start;
using MPOWR.Dal.Models;
using MPOWR.Core;

namespace MPOWR.Api.Controllers
{
     [AuthorizeUser]
    public class PartnerBudgetController : ApiController
    {
        MPOWREntities objDBEntity = new MPOWREntities();

        [HttpPost]
        [Route("api/PartnerBudget/GetPartnerDetails")]
        public IHttpActionResult GetPartnerDetails(Filters filters)
        {
            try
            {
                //if (filters.SortColumn == "MGO_Ratio")
                //{
                //    filters.SortColumn = "WMGO_Ratio";
                //}
                StringBuilder outputResult = new StringBuilder();
                //string yearShortName = filters.Financialyear.Replace('_', ' ');
                int financialYearID = filters.FinancialyearID;// objDBEntity.FinancialYears.AsNoTracking().Where(x => x.ShortName == yearShortName).Select(x => x.FinancialYearID).FirstOrDefault();
                DbCommand command = objDBEntity.Database.Connection.CreateCommand();
                command.CommandText = "usp_GetPartnerDetails_Pagination1";
                command.Parameters.Add(new SqlParameter("@BusinessUnitID", filters.businessUnitID));
                command.Parameters.Add(new SqlParameter("@PageIndex", filters.PageIndex));
                command.Parameters.Add(new SqlParameter("@PageSize", filters.PageSize));
                command.Parameters.Add(new SqlParameter("@SortColumn", filters.SortColumn));
                command.Parameters.Add(new SqlParameter("@SortOrder", filters.SortOrder));
                command.Parameters.Add(new SqlParameter("@V_ID", filters.VersionID));
                command.Parameters.Add(new SqlParameter("@USER_ID", filters.UserID));
              
                command.Parameters.Add(new SqlParameter("@WithoutHistory", filters.WithoutHistory));

                command.Parameters.Add(new SqlParameter("@FilterColumnName", filters.FilterColumn));
                command.Parameters.Add(new SqlParameter("@FilterDelimeter", filters.FilterDelimeter));
                command.Parameters.Add(new SqlParameter("@FilterValue", filters.FilterValue));
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandTimeout = 100000;
                objDBEntity.Database.CommandTimeout = 1000000;
                objDBEntity.Database.Connection.Open();
                var reader = command.ExecuteReader();
                var Result = string.Empty;

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (Result == string.Empty)
                        {
                            Result = reader.GetValue(0).ToString();
                        }
                        else
                        {
                            Result = Result + reader.GetValue(0).ToString();
                        }
                    }
                }
                reader.Close();
                Object json;

                if (Result == null || Result == "")
                {
                    json = Result;
                }
                else { json = JToken.Parse(Result); }

                    objDBEntity.Database.Connection.Close();
                    return Ok(json);
                
            }

            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

                return null;

            }
            catch (Exception ex)
            {

                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }
        [HttpGet]
        [Route("api/PartnerBudget/GetPartnerBudgetSearchFilter")]
        public IHttpActionResult GetPartnerBudgetSearchFilter(int businessUnitID, int VersionID, string FilterColumnName, string FilterDelimeter, string FilterValue)
        {
            try
            {

                DbCommand command = objDBEntity.Database.Connection.CreateCommand();
                command.CommandText = "usp_GetPartnerDetails_Search_Filter";
                command.Parameters.Add(new SqlParameter("@BusinessUnitID", businessUnitID));
                command.Parameters.Add(new SqlParameter("@WithoutHistory", false));        
                command.Parameters.Add(new SqlParameter("@V_ID", VersionID));
                command.Parameters.Add(new SqlParameter("@FilterColumnName", FilterColumnName));
                command.Parameters.Add(new SqlParameter("@FilterDelimeter", FilterDelimeter));
                command.Parameters.Add(new SqlParameter("@FilterValue", FilterValue));

                objDBEntity.Database.CommandTimeout = 1000000;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                objDBEntity.Database.Connection.Open();
                var reader = command.ExecuteReader();
                var Result = string.Empty;

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (Result == string.Empty)
                        {
                            Result = reader.GetValue(0).ToString();
                        }
                        else
                        {
                            Result = Result + reader.GetValue(0).ToString();
                        }
                    }
                }
                reader.Close();
                Object json;

                if (Result == null || Result == "")
                {
                    json = Result;
                }
                else { json = JToken.Parse(Result); }

                objDBEntity.Database.Connection.Close();
                return Ok(json);
            }

            catch (Exception ex)
            {

                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }
        [HttpGet]
        [Route("api/PartnerBudget/GetPartnerBudgetSearch")]
        public IHttpActionResult GetPartnerBudgetSearch(int businessUnitID,string SearchColumn, string find, int VersionID,bool WithoutHistory, string FilterColumnName, string FilterDelimeter, string FilterValue)
        {
            try
            {
                DbCommand command = objDBEntity.Database.Connection.CreateCommand();
                command.CommandText = "usp_GetPartnerDetails_Search";
                command.Parameters.Add(new SqlParameter("@BusinessUnitID", businessUnitID));
                command.Parameters.Add(new SqlParameter("@SearchColumn", SearchColumn));
                command.Parameters.Add(new SqlParameter("@find", find));
                command.Parameters.Add(new SqlParameter("@V_ID", VersionID));
                command.Parameters.Add(new SqlParameter("@WithoutHistory", WithoutHistory));
                if(SearchColumn.Length == 0) { 
                    command.Parameters.Add(new SqlParameter("@FilterColumnName", FilterColumnName));
                    command.Parameters.Add(new SqlParameter("@FilterDelimeter", FilterDelimeter));
                    command.Parameters.Add(new SqlParameter("@FilterValue", FilterValue));
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("@FilterColumnName", null));
                    command.Parameters.Add(new SqlParameter("@FilterDelimeter", null));
                    command.Parameters.Add(new SqlParameter("@FilterValue", null));
                }

                objDBEntity.Database.CommandTimeout = 1000000;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                objDBEntity.Database.Connection.Open();
                var reader = command.ExecuteReader();
                var Result = string.Empty;

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (Result == string.Empty)
                        {
                            Result = reader.GetValue(0).ToString();
                        }
                        else
                        {
                            Result = Result + reader.GetValue(0).ToString();
                        }
                    }
                }
                reader.Close();
                Object json;

                if (Result == null || Result == "")
                {
                    json = Result;
                }
                else { json = JToken.Parse(Result); }

                 objDBEntity.Database.Connection.Close();
                return Ok(json);
            }
             
            catch (Exception ex)
            {

                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }

        [HttpPost]
        [Route("api/PartnerBudget/GetPartnerDetailsSummary")]
        public string GetPartnerDetailsSummary(Filters filters)
        {
            try
            {
                DbCommand command = objDBEntity.Database.Connection.CreateCommand();
                command.CommandText = "usp_GetPartnerBudgetSummary";
                command.Parameters.Add(new SqlParameter("@BusinessUnitID", filters.businessUnitID));
                command.Parameters.Add(new SqlParameter("@V_ID", filters.VersionID));


                objDBEntity.Database.CommandTimeout = 1000000;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                objDBEntity.Database.Connection.Open();
                var reader = command.ExecuteReader();
                var Result = string.Empty;

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (Result == string.Empty)
                        {
                            Result = reader.GetValue(0).ToString();
                        }
                        else
                        {
                            Result = Result + reader.GetValue(0).ToString();
                        }
                    }
                }
                reader.Close();
                
                objDBEntity.Database.Connection.Close();
                return Result;
            }

            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

                return null;

            }
            catch (Exception ex)
            {

                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }

        [HttpGet]
        [Route("api/PartnerBudget/GetPartnerBudgetBUDetails")]
        public string GetPartnerBudgetBUDetails(int PartnerBudgetID,int VersionID,bool WithoutHistory)
        {
            try
            {
                DbCommand command = objDBEntity.Database.Connection.CreateCommand();
                command.CommandText = "usp_GetPartnerBUBudget";
                command.Parameters.Add(new SqlParameter("@PartnerBudgetID", PartnerBudgetID));
                command.Parameters.Add(new SqlParameter("@V_ID", VersionID));
                command.Parameters.Add(new SqlParameter("@WithoutHistory", WithoutHistory));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                objDBEntity.Database.Connection.Open();
                var reader = command.ExecuteReader();
                var Result = string.Empty;

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (Result == string.Empty)
                        {
                            Result = reader.GetValue(0).ToString();
                        }
                        else
                        {
                            Result = Result + reader.GetValue(0).ToString();
                        }
                    }
                }
                reader.Close();
                 
                objDBEntity.Database.Connection.Close();
                return Result;
            }

            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

                return null;

            }
            catch (Exception ex)
            {

                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }

        [HttpPost]
        [Route("api/PartnerBudget/GetRemainingBudget")]
        public IHttpActionResult GetRemainingBudget(Filters filters)
        {
          //  decimal remainingBudget = 0;
            try
            {
               // string yearShortName = filters.Financialyear.Replace('_', ' ');
               // int financialYearID = Int32.Parse(filters.Financialyear);// objDBEntity.FinancialYears.AsNoTracking().Where(x => x.ShortName == yearShortName).Select(x => x.FinancialYearID).FirstOrDefault();

                //remainingBudget = objDBEntity.sp_CalculateRemainingBudget(filters.CountryID, filters.PartnerTypeID, filters.DistrictID, financialYearID)
                //    .FirstOrDefault().Value;
                var Result = string.Empty;
              
                DbCommand command = objDBEntity.Database.Connection.CreateCommand();
                command.CommandText = "usp_CalculateRemainingBudget";
                command.Parameters.Add(new SqlParameter("@V_ID", filters.VersionID));

                command.CommandType = System.Data.CommandType.StoredProcedure;

                objDBEntity.Database.Connection.Open();
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (Result == string.Empty)
                        {
                            Result = reader.GetValue(0).ToString();
                        }
                        else
                        {
                            Result = Result + reader.GetValue(0).ToString();
                        }
                    }
                }
                reader.Close();
                Object json;

                if (Result == null || Result == "")
                {
                    json = Result;
                }
                else { json = JToken.Parse(Result); }
                 objDBEntity.Database.Connection.Close();

                return Ok(json);
            }
            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

                return null;

            }
            catch (Exception ex)
            {

                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
            //return remainingBudget;
        }

        //public PartnersCompleteList GetPartnerDetails(short CountryID, int PartnerTypeID, string Financialyear, int DistrictID,int businessUnitID=0)
        //{
        //    PartnersCompleteList partnersCompleteList = new PartnersCompleteList();

        //    try
        //    {
        //       //code review - need to use predicate and can acheive in a single LinQ query.

        //            string yearShortName = Financialyear.Replace('_', ' ');
        //            int financialYearID = objDBEntity.FinancialYears.AsNoTracking().Where(x => x.ShortName == yearShortName).Select(x => x.FinancialYearID).FirstOrDefault();
        //            string financialYearExact = Financialyear.Split('_')[0];
        //        List<ModelOutputTable> modelOutputEntity;
        //        List<PartnerBudget> partnerBudgetEntity;
        //        List<BusinessUnit> businessUnitEntity;
        //        List<PartnerBUBudget> partnerBUBudgetEntity;

        //        if (businessUnitID == 0)
        //        {

        //             modelOutputEntity = objDBEntity.ModelOutputTables.AsNoTracking().Where(x => x.CountryID == CountryID && x.PartnerTypeID == PartnerTypeID && x.FinancialYearID == financialYearID &&
        //                (x.DistrictID == DistrictID || x.DistrictID == null) 
        //                //&& x.ModelParameterID!=1
        //                ).ToList();

        //             partnerBudgetEntity = objDBEntity.PartnerBudgets.AsNoTracking()
        //                    .Where(x => x.FinancialYearID == financialYearID && x.CountryID == CountryID && (x.DistrictID == DistrictID || x.DistrictID == null) &&
        //                    x.PartnerTypeID == PartnerTypeID 
        //                    //&& x.ModelParameterID != 1
        //                    ).ToList();

        //            if (partnerBudgetEntity.Count <= 0)
        //            {
        //                objDBEntity.sp_InsertPartnerBudgetData(0, PartnerTypeID, CountryID, DistrictID, financialYearID);

        //                partnerBudgetEntity = objDBEntity.PartnerBudgets.AsNoTracking()
        //               .Where(x => x.FinancialYearID == financialYearID && x.CountryID == CountryID && (x.DistrictID == DistrictID || x.DistrictID == null) &&
        //               x.PartnerTypeID == PartnerTypeID 
        //               //&& x.ModelParameterID != 1
        //               ).ToList();
        //            }
        //            long[] partnerBudgetIds = partnerBudgetEntity.Select(x => x.PartnerBudgetID).ToArray();

        //                businessUnitEntity = objDBEntity.BusinessUnits.AsNoTracking().ToList();
        //                partnerBUBudgetEntity = objDBEntity.PartnerBUBudgets.AsNoTracking().Where(x => partnerBudgetIds.Contains(x.PartnerBudgetID) ).ToList();
        //            }
        //            else
        //            {
        //            modelOutputEntity = objDBEntity.ModelOutputTables.AsNoTracking().Where(x => x.CountryID == CountryID && x.PartnerTypeID == PartnerTypeID && x.FinancialYearID == financialYearID &&
        //               (x.DistrictID == DistrictID || x.DistrictID == null)
        //               //&& x.ModelParameterID != 1 
        //               && x.BusinessUnitID==businessUnitID).ToList();

        //            partnerBudgetEntity = objDBEntity.PartnerBudgets.AsNoTracking()
        //                   .Where(x => x.FinancialYearID == financialYearID && x.CountryID == CountryID && (x.DistrictID == DistrictID || x.DistrictID == null) &&
        //                   x.PartnerTypeID == PartnerTypeID 
        //                  // && x.ModelParameterID != 1 
        //                   ).ToList();

        //            if (partnerBudgetEntity.Count <= 0)
        //            {
        //                objDBEntity.sp_InsertPartnerBudgetData(0, PartnerTypeID, CountryID, DistrictID, financialYearID);

        //                partnerBudgetEntity = objDBEntity.PartnerBudgets.AsNoTracking()
        //               .Where(x => x.FinancialYearID == financialYearID && x.CountryID == CountryID && (x.DistrictID == DistrictID || x.DistrictID == null) &&
        //               x.PartnerTypeID == PartnerTypeID
        //               //&& x.ModelParameterID != 1 
        //               ).ToList();
        //            }
        //            long[] partnerBudgetIds = partnerBudgetEntity.Select(x => x.PartnerBudgetID).ToArray();
        //            businessUnitEntity = objDBEntity.BusinessUnits.AsNoTracking().Where(x=>x.BusinessUnitID== businessUnitID).ToList();
        //                partnerBUBudgetEntity = objDBEntity.PartnerBUBudgets.AsNoTracking().Where(x => partnerBudgetIds.Contains(x.PartnerBudgetID) && x.BusinessUnitID == businessUnitID).ToList();
        //            }


        //            var mdfEntity = objDBEntity.MDFVarianceReasons.AsNoTracking().ToList();
        //            var partnerSalesEntity = objDBEntity.PartnerSales.AsNoTracking().Where(x => x.CountryID == CountryID && x.PartnerTypeID == PartnerTypeID &&
        //            (x.FinancialYearID == financialYearID || x.FinancialYear == financialYearExact) && (x.DistrictID == DistrictID || x.DistrictID == null) ).ToList();


        //            if (modelOutputEntity.Count > 0)
        //            {
        //                var distinctKeys = modelOutputEntity.Select(e => new { e.PartnerID, e.Partner_Name, e.Membership_Type }).Distinct().ToList();
        //                List<PartnerDetails> ModelOutputTableList = new List<PartnerDetails>();
        //            var count = 0;
        //                foreach (var aa in distinctKeys)
        //                {
        //                count = count + 1;
        //                    PartnerDetails modelOutputNew = new PartnerDetails();
        //                    modelOutputNew.PartnerID = aa.PartnerID;
        //                    modelOutputNew.Partner_Name = aa.Partner_Name;
        //                    modelOutputNew.Membership_Type = aa.Membership_Type;
        //                    modelOutputNew.MSA = (from partnerBudget in partnerBudgetEntity
        //                                          where partnerBudget.PartnerID == aa.PartnerID && aa.Membership_Type == partnerBudget.Membership_Type
        //                                          // where partnerBudget.PartnerID == modelOutput.PartnerID && partBUBudget.BusinessUnitID == modelOutput.BusinessUnitID
        //                                          select new MSA
        //                                          {
        //                                              MSAValue = partnerBudget.MSA ?? 0,
        //                                              RecommendedMSA = partnerBudget.MSA ?? 0,
        //                                              PartnerBudgetID = partnerBudget.PartnerBudgetID
        //                                          }).ToList();
        //                    modelOutputNew.BU = (from partnerBudget in partnerBudgetEntity
        //                                         join modelOutput in modelOutputEntity on partnerBudget.PartnerID equals modelOutput.PartnerID//.Where(x => x.PartnerID == aa.PartnerID)
        //                                         where partnerBudget.PartnerID == aa.PartnerID && modelOutput.PartnerID == aa.PartnerID
        //                                         select new BUDetails
        //                                         {
        //                                             Business_Unit = modelOutput.Business_Unit,
        //                                             PartnerBudgetID = partnerBudget.PartnerBudgetID,
        //                                             BusinessUnitID = modelOutput.BusinessUnitID,
        //                                             Sellout = (from bunit in businessUnitEntity
        //                                                        where bunit.BusinessUnitID == modelOutput.BusinessUnitID
        //                                                        select new SellOut
        //                                                        {
        //                                                            Last_Period_Sellout = modelOutput.Last_Period_Sellout,
        //                                                            Projected_Sellout = modelOutput.Projected_Sellout,
        //                                                            YoY_change_sellout = modelOutput.YoY_change_sellout,
        //                                                            BusinessUnitID = bunit.BusinessUnitID
        //                                                        }).ToList(),
        //                                             MDF = (from partBUBudget in partnerBUBudgetEntity
        //                                                        //join variance in mdfEntity on partBUBudget.MDFVarianceReasonID equals variance.MDFVarianceReasonID into varianceReasons
        //                                                        //from variance in varianceReasons.DefaultIfEmpty()
        //                                                    where partBUBudget.PartnerBudgetID == partnerBudget.PartnerBudgetID && partBUBudget.BusinessUnitID == modelOutput.BusinessUnitID
        //                                                    select new MDF
        //                                                    {
        //                                                        PartnerBUBudgetID = partBUBudget.PartnerBUBudgetID,
        //                                                        Last_Period_MDF = modelOutput.Last_Period_MDF ?? 0,
        //                                                        Recommended_MDF = modelOutput.Recommended_MDF ?? 0,
        //                                                        YoY_change_MDF = modelOutput.YoY_change_MDF ?? 0,
        //                                                        BaseLineMDF = partBUBudget.Baseline_MDF ?? 0,
        //                                                        ReasonForVariance = partBUBudget.MDFVarianceReasonID > 0 ?
        //                                                        mdfEntity.Where(x => x.MDFVarianceReasonID == partBUBudget.MDFVarianceReasonID).Select(x => x.Reason).FirstOrDefault() : string.Empty,
        //                                                        Comment = partBUBudget.Comments,
        //                                                        MDFVarianceReasonID = partBUBudget.MDFVarianceReasonID > 0 ? partBUBudget.MDFVarianceReasonID.Value : 0
        //                                                    }).ToList(),
        //                                             MDFOrSellout = (from mdfsellOut in modelOutputEntity
        //                                                             where mdfsellOut.PartnerID == modelOutput.PartnerID && mdfsellOut.BusinessUnitID == modelOutput.BusinessUnitID
        //                                                             select new MDFOrSellout
        //                                                             {
        //                                                                 LastYearMDFOrSellout = mdfsellOut.Last_Period_MDF,
        //                                                                 ProjectedMDFOrSellout = mdfsellOut.Recommended_MDF,
        //                                                                 MedianAvdMDFOrSellout = mdfsellOut.Median_Avg_MDF_Sellout,
        //                                                                 ProductivityImprovementPer = mdfsellOut.Projected_Productivity
        //                                                             }).ToList(),
        //                                             Analysis = (from analysis in modelOutputEntity
        //                                                         where analysis.ModelParamROutputID == modelOutput.ModelParamROutputID
        //                                                         //where analysis.PartnerID == modelOutput.PartnerID && analysis.BusinessUnitID == modelOutput.BusinessUnitID
        //                                                         select new Analysis
        //                                                         {
        //                                                             MDFAllignment = analysis.MDF_Alignment,
        //                                                             AssesmentOfLastYearMDF = analysis.PREV_MDF_Assessment,
        //                                                             PredictionAccuracy = analysis.Prediction_Accuracy
        //                                                         }).ToList(),
        //                                             PBM = modelOutput.PBM,
        //                                             PMM = modelOutput.PMM,
        //                                             PlannedSales = modelOutput.Planned_Sales,
        //                                             targetAchievement = modelOutput.Target_Achievement,
        //                                             SOW = modelOutput.SOW,
        //                                             SOWGrowth = modelOutput.SOW_Growth,
        //                                             FootprintGrowth = modelOutput.Footprint_Growth,
        //                                             No_of_end_customers = modelOutput.No_of_end_customers,
        //                                             Total_MDF = modelOutput.Total_MDF,
        //                                             Incremental_MDF = modelOutput.Incremental_MDF,
        //                                             Late_MDF = modelOutput.Late_MDF,
        //                                             W_MGO_Marketing_MDF = modelOutput.W_MGO_Marketing_MDF,
        //                                             New_Logos_MGO = modelOutput.New_Logos_MGO,
        //                                             HistoryMDF = (from sales in partnerSalesEntity
        //                                                           where sales.PartnerID == modelOutput.PartnerID && sales.CountryID == CountryID
        //                                                           select new HistoryMDF
        //                                                           {
        //                                                               FinancialYear = sales.FinancialYear,
        //                                                               MDF_1H = sales.MDF_1H,
        //                                                               MDF_2H = sales.MDF_2H
        //                                                           }).ToList(),
        //                                             HistorySellout = (from sales in partnerSalesEntity
        //                                                               where sales.PartnerID == modelOutput.PartnerID && sales.CountryID == CountryID
        //                                                               select new HistorySellout
        //                                                               {
        //                                                                   FinancialYear = sales.FinancialYear,
        //                                                                   SellOut_Q1 = sales.SellOut_Q1,
        //                                                                   SellOut_Q2 = sales.SellOut_Q2,
        //                                                                   SellOut_Q3 = sales.SellOut_Q3,
        //                                                                   SellOut_Q4 = sales.SellOut_Q4
        //                                                               }).ToList()
        //                                         }).ToList();

        //                    ModelOutputTableList.Add(modelOutputNew);


        //                }
        //                partnersCompleteList.PartnerDetailsList = ModelOutputTableList;
        //                partnersCompleteList.TotalofPartners = GenerateTotalofPartners(ModelOutputTableList);
        //            }


        //        return partnersCompleteList;


        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //}
        //TotalofPartners GenerateTotalofPartners(IList<PartnerDetails> modelOutput)
        //{
        //    TotalofPartners totalPartners = new TotalofPartners();
        //    SellOut totalSellOut = new SellOut();
        //    MDF totalMDF = new MDF();
        //    MDFOrSellout mdfOrSellOut = new MDFOrSellout();
        //    HistoryMDF historyMDF = new HistoryMDF();
        //    HistorySellout historySellOut = new HistorySellout();

        //    totalSellOut.Last_Period_Sellout = 0;
        //    totalSellOut.Projected_Sellout = 0;
        //    totalSellOut.YoY_change_sellout = 0;

        //    totalMDF.BaseLineMDF = 0;
        //    totalMDF.Last_Period_MDF = 0;
        //    totalMDF.YoY_change_MDF = 0;
        //    totalMDF.Recommended_MDF = 0;

        //    totalPartners.PlannedSales = 0;

        //    historyMDF.MDF_1H = 0;
        //    historyMDF.MDF_2H = 0;

        //    historySellOut.SellOut_Q1 = 0;
        //    historySellOut.SellOut_Q2 = 0;
        //    historySellOut.SellOut_Q3 = 0;
        //    historySellOut.SellOut_Q4 = 0;

        //    foreach (PartnerDetails model in modelOutput)
        //    {
        //        foreach (BUDetails bu in model.BU)
        //        {
        //            totalSellOut.Last_Period_Sellout += bu.Sellout.Sum(x => x.Last_Period_Sellout);
        //            totalSellOut.Projected_Sellout += bu.Sellout.Sum(x => x.Projected_Sellout);
        //            if (bu.MDF != null && bu.MDF.Count() > 0)
        //            {
        //                totalMDF.BaseLineMDF += bu.MDF.Sum(x => x.BaseLineMDF);
        //                totalMDF.Last_Period_MDF += bu.MDF.Sum(x => x.Last_Period_MDF);
        //                totalMDF.Recommended_MDF += bu.MDF.Sum(x => x.Recommended_MDF);
        //            }

        //            if (bu.HistoryMDF != null && bu.HistoryMDF.Count() > 0)
        //            {
        //                historyMDF.MDF_1H += bu.HistoryMDF.Sum(x => x.MDF_1H);
        //                historyMDF.MDF_2H += bu.HistoryMDF.Sum(x => x.MDF_2H);
        //            }

        //            if (bu.HistorySellout != null && bu.HistorySellout.Count() > 0)
        //            {
        //                historySellOut.SellOut_Q1 += bu.HistorySellout.Sum(x => x.SellOut_Q1);
        //                historySellOut.SellOut_Q2 += bu.HistorySellout.Sum(x => x.SellOut_Q2); ;
        //                historySellOut.SellOut_Q3 += bu.HistorySellout.Sum(x => x.SellOut_Q3); ;
        //                historySellOut.SellOut_Q4 += bu.HistorySellout.Sum(x => x.SellOut_Q4); ;
        //            }

        //        }

        //        totalPartners.PlannedSales += model.BU.Sum(x => x.PlannedSales);
        //        totalPartners.targetAchievement += model.BU.Sum(x => x.targetAchievement);
        //        totalPartners.SOW += model.BU.Sum(x => x.SOW);
        //        totalPartners.SOWGrowth += model.BU.Sum(x => x.SOWGrowth);
        //        totalPartners.FootprintGrowth += model.BU.Sum(x => x.FootprintGrowth);
        //        totalPartners.No_of_end_customers += model.BU.Sum(x => x.No_of_end_customers);
        //        totalPartners.Total_MDF += model.BU.Sum(x => x.Total_MDF);

        //        totalPartners.Incremental_MDF += model.BU.Sum(x => x.Incremental_MDF);
        //        totalPartners.Late_MDF += model.BU.Sum(x => x.Late_MDF);
        //        totalPartners.W_MGO_Marketing_MDF += model.BU.Sum(x => x.W_MGO_Marketing_MDF);

        //        totalPartners.New_Logos_MGO += model.BU.Sum(x => x.New_Logos_MGO);

        //    }

        //    totalSellOut.YoY_change_sellout = totalSellOut.Last_Period_Sellout > 0 ? ((totalSellOut.Projected_Sellout / totalSellOut.Last_Period_Sellout) - 1) * 100 : 0;
        //    totalMDF.YoY_change_MDF = totalMDF.Last_Period_MDF > 0 ? ((totalMDF.Recommended_MDF / totalMDF.Last_Period_MDF) - 1) * 100 : 0;
        //    mdfOrSellOut.ProjectedMDFOrSellout = totalSellOut.Projected_Sellout > 0 ? (totalMDF.Recommended_MDF / totalSellOut.Projected_Sellout) * 100 : 0;

        //    totalPartners.SellOut = totalSellOut;
        //    totalPartners.MDF = totalMDF;
        //    totalPartners.MDFOrSellOut = mdfOrSellOut;
        //    totalPartners.HistoryMDF = historyMDF;
        //    totalPartners.HistorySellOut = historySellOut;

        //    return totalPartners;
        //}

        //public IList<ModelOutputTableViewModel> GetRound2Details(short CountryID, int PartnerTypeID, string Financialyear, int DistrictID, int FocusAreaID)
        //{
        //    IList<ModelOutputTableViewModel> modelOutputModel = new List<ModelOutputTableViewModel>();

        //    try
        //    {
        //        string yearShortName = Financialyear.Replace('_', ' ');
        //        int financialYearID = objDBEntity.FinancialYears.AsNoTracking().Where(x => x.ShortName == yearShortName).Select(x => x.FinancialYearID).FirstOrDefault();
        //        string financialYearExact = Financialyear.Split('_')[0];
        //        var CountryPartners = objDBEntity.ModelParameterTables.AsNoTracking()
        //            .Where(x => x.CountryID == CountryID && x.PartnerTypeID == PartnerTypeID && x.FinancialYearID == financialYearID &&
        //            (x.DistrictID == DistrictID || x.DistrictID == null)).ToList();
        //        if (CountryPartners.Count > 0)
        //        {

        //            int[] modelParameterIDs = CountryPartners.Select(x => x.ModelParameterID).ToArray();

        //            var modelOutputEntity = objDBEntity.ModelOutputTables.AsNoTracking().Where(x => modelParameterIDs.Contains(x.ModelParameterID.Value)).ToList();



        //            var partnerBudgetEntity = objDBEntity.PartnerBudgets.AsNoTracking()
        //                .Where(x => x.FinancialYearID == financialYearID && x.CountryID == CountryID && (x.DistrictID == DistrictID || x.DistrictID == null) &&
        //                x.PartnerTypeID == PartnerTypeID).ToList();

        //            if (partnerBudgetEntity.Count <= 0)
        //            {
        //                objDBEntity.sp_InsertPartnerBudgetData(PartnerTypeID, CountryID, DistrictID, financialYearID);

        //                partnerBudgetEntity = objDBEntity.PartnerBudgets.AsNoTracking()
        //               .Where(x => x.FinancialYearID == financialYearID && x.CountryID == CountryID && (x.DistrictID == DistrictID || x.DistrictID == null) &&
        //               x.PartnerTypeID == PartnerTypeID).ToList();
        //            }

        //            var businessUnitEntity = objDBEntity.BusinessUnits.AsNoTracking().ToList();

        //            long[] partnerBudgetIds = partnerBudgetEntity.Select(x => x.PartnerBudgetID).ToArray();

        //            var partnerBUBudgetEntity = objDBEntity.PartnerBUBudgets.AsNoTracking().Where(x => partnerBudgetIds.Contains(x.PartnerBudgetID)).ToList();
        //            var mdfEntity = objDBEntity.MDFVarianceReasons.AsNoTracking().ToList();
        //            var partnerSalesEntity = objDBEntity.PartnerSales.AsNoTracking().Where(x => x.CountryID == CountryID && x.PartnerTypeID == PartnerTypeID &&
        //            (x.FinancialYearID == financialYearID || x.FinancialYear == financialYearExact) && (x.DistrictID == DistrictID || x.DistrictID == null)).ToList();
        //            if (modelOutputEntity.Count > 0)
        //            {
        //                var Partners = (from modelOutput in modelOutputEntity
        //                                select new ModelOutputTableViewModel
        //                                {
        //                                    Id = modelOutput.PartnerID,
        //                                    ModelParameterID = modelOutput.ModelParameterID,
        //                                    Partner_Name = modelOutput.Partner_Name,
        //                                    Membership_Type = modelOutput.Membership_Type,
        //                                    Allignment = modelOutput.MDF_Alignment,
        //                                    Business_Unit = modelOutput.Business_Unit,
        //                                    MSA = (from partnerBudget in partnerBudgetEntity
        //                                           where partnerBudget.PartnerID == modelOutput.PartnerID && modelOutput.Membership_Type == partnerBudget.Membership_Type
        //                                           // where partnerBudget.PartnerID == modelOutput.PartnerID && partBUBudget.BusinessUnitID == modelOutput.BusinessUnitID
        //                                           select new MSA
        //                                           {
        //                                               MSAValue = partnerBudget.MSA ?? 0,
        //                                               RecommendedMSA = partnerBudget.MSA ?? 0,
        //                                               PartnerBudgetID = partnerBudget.PartnerBudgetID
        //                                           }),
        //                                    BU = (from partnerBudget in partnerBudgetEntity
        //                                          where partnerBudget.PartnerID == modelOutput.PartnerID
        //                                          select new BUDetails
        //                                          {
        //                                              Business_Unit = modelOutput.Business_Unit,
        //                                              PartnerBudgetID = partnerBudget.PartnerBudgetID,
        //                                              BusinessUnitID = modelOutput.BusinessUnitID,
        //                                              Sellout = (from bunit in businessUnitEntity
        //                                                         where bunit.BusinessUnitID == modelOutput.BusinessUnitID
        //                                                         select new SellOut
        //                                                         {
        //                                                             Last_Period_Sellout = modelOutput.Last_Period_Sellout,
        //                                                             Projected_Sellout = modelOutput.Projected_Sellout,
        //                                                             YoY_change_sellout = modelOutput.YoY_change_sellout,
        //                                                             BusinessUnitID = bunit.BusinessUnitID
        //                                                         }),
        //                                              MDF = (from partBUBudget in partnerBUBudgetEntity
        //                                                         //join variance in mdfEntity on partBUBudget.MDFVarianceReasonID equals variance.MDFVarianceReasonID into varianceReasons
        //                                                         //from variance in varianceReasons.DefaultIfEmpty()
        //                                                     where partBUBudget.PartnerBudgetID == partnerBudget.PartnerBudgetID && partBUBudget.BusinessUnitID == modelOutput.BusinessUnitID
        //                                                     select new MDF
        //                                                     {
        //                                                         PartnerBUBudgetID = partBUBudget.PartnerBUBudgetID,
        //                                                         Last_Period_MDF = modelOutput.Last_Period_MDF ?? 0,
        //                                                         Recommended_MDF = modelOutput.Recommended_MDF ?? 0,
        //                                                         YoY_change_MDF = modelOutput.YoY_change_MDF ?? 0,
        //                                                         BaseLineMDF = partBUBudget.Baseline_MDF ?? 0,
        //                                                         AdditionalMdf = partBUBudget.Additional_MDF ?? 0,
        //                                                         Additional_Reccommended_MDF = partBUBudget.Additional_RecommendedMDF ?? 0,
        //                                                         Additional_MDF_Reason = partBUBudget.Additional_MDF_Reason,
        //                                                         ReasonForVariance = partBUBudget.MDFVarianceReasonID > 0 ?
        //                                                         mdfEntity.Where(x => x.MDFVarianceReasonID == partBUBudget.MDFVarianceReasonID).Select(x => x.Reason).FirstOrDefault() : string.Empty,
        //                                                         Comment = partBUBudget.Comments,
        //                                                         MDFVarianceReasonID = partBUBudget.MDFVarianceReasonID > 0 ? partBUBudget.MDFVarianceReasonID.Value : 0
        //                                                     }),
        //                                              MDFOrSellout = (from mdfsellOut in modelOutputEntity
        //                                                              where mdfsellOut.PartnerID == modelOutput.PartnerID && mdfsellOut.BusinessUnitID == modelOutput.BusinessUnitID
        //                                                              select new MDFOrSellout
        //                                                              {
        //                                                                  LastYearMDFOrSellout = mdfsellOut.Last_Period_MDF,
        //                                                                  ProjectedMDFOrSellout = mdfsellOut.Recommended_MDF,
        //                                                                  MedianAvdMDFOrSellout = mdfsellOut.Median_Avg_MDF_Sellout,
        //                                                                  ProductivityImprovementPer = mdfsellOut.Projected_Productivity
        //                                                              }),
        //                                              Analysis = (from analysis in modelOutputEntity
        //                                                          where analysis.ModelParamROutputID == modelOutput.ModelParamROutputID
        //                                                          //where analysis.PartnerID == modelOutput.PartnerID && analysis.BusinessUnitID == modelOutput.BusinessUnitID
        //                                                          select new Analysis
        //                                                          {
        //                                                              MDFAllignment = analysis.MDF_Alignment,
        //                                                              AssesmentOfLastYearMDF = analysis.PREV_MDF_Assessment,
        //                                                              PredictionAccuracy = analysis.Prediction_Accuracy
        //                                                          }),
        //                                              PBM = modelOutput.PBM,
        //                                              PMM = modelOutput.PMM,
        //                                              PlannedSales = modelOutput.Planned_Sales,
        //                                              targetAchievement = modelOutput.Target_Achievement,
        //                                              SOW = modelOutput.SOW,
        //                                              SOWGrowth = modelOutput.SOW_Growth,
        //                                              FootprintGrowth = modelOutput.Footprint_Growth,
        //                                              No_of_end_customers = modelOutput.No_of_end_customers,
        //                                              Total_MDF = modelOutput.Total_MDF,
        //                                              Incremental_MDF = modelOutput.Incremental_MDF,
        //                                              Late_MDF = modelOutput.Late_MDF,
        //                                              W_MGO_Marketing_MDF = modelOutput.W_MGO_Marketing_MDF,
        //                                              New_Logos_MGO = modelOutput.New_Logos_MGO,
        //                                              HistoryMDF = (from sales in partnerSalesEntity
        //                                                            where sales.PartnerID == modelOutput.PartnerID && sales.CountryID == CountryID
        //                                                            select new HistoryMDF
        //                                                            {
        //                                                                FinancialYear = sales.FinancialYear,
        //                                                                MDF_1H = sales.MDF_1H,
        //                                                                MDF_2H = sales.MDF_2H
        //                                                            }),
        //                                              HistorySellout = (from sales in partnerSalesEntity
        //                                                                where sales.PartnerID == modelOutput.PartnerID && sales.CountryID == CountryID
        //                                                                select new HistorySellout
        //                                                                {
        //                                                                    FinancialYear = sales.FinancialYear,
        //                                                                    SellOut_Q1 = sales.SellOut_Q1,
        //                                                                    SellOut_Q2 = sales.SellOut_Q2,
        //                                                                    SellOut_Q3 = sales.SellOut_Q3,
        //                                                                    SellOut_Q4 = sales.SellOut_Q4
        //                                                                })
        //                                          })
        //                                }
        //                               ).ToList();

        //                modelOutputModel = Partners;


        //            }
        //        }
        //        return modelOutputModel;


        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //}

        //public IQueryable GetPartnerDetails(short CountryID, int PartnerTypeID)
        //{
        //    try
        //    {
        //        var CountryPartners = (from CntryPart in objDBEntity.ModelParameterTables
        //                               where CntryPart.CountryID == CountryID && CntryPart.PartnerTypeID == PartnerTypeID
        //                               select CntryPart
        //                              );


        //        var Partners = (from OutputResult in objDBEntity.ModelOutputTables//.GroupBy(grp => grp.PartnerID).Select(test => test.FirstOrDefault()).ToList()
        //                        select new ModelOutputTableViewModel
        //                        {
        //                            Id = OutputResult.PartnerID,
        //                            ModelParameterID = OutputResult.ModelParameterID,
        //                            Partner_Name = OutputResult.Partner_Name,
        //                            Membership_Type = OutputResult.Membership_Type,
        //                            Allignment = OutputResult.MDF_Alignment,
        //                            Business_Unit = OutputResult.Business_Unit,
        //                            BU = (from modelOut in objDBEntity.ModelOutputTables
        //                                  join pb in objDBEntity.PartnerBudgets on modelOut.PartnerID equals pb.PartnerID into GroupParners
        //                                  from varGroupParners in GroupParners.DefaultIfEmpty()
        //                                  where modelOut.PartnerID == OutputResult.PartnerID && modelOut.BusinessUnitID == OutputResult.BusinessUnitID
        //                                  select new BUDetails
        //                                  {
        //                                      Business_Unit = modelOut.Business_Unit,
        //                                       PartnerBudgetID = varGroupParners.PartnerBudgetID,
        //                                        BusinessUnitID = modelOut.BusinessUnitID,
        //                                      Sellout = (from modelOutput in objDBEntity.ModelOutputTables
        //                                                 join bu in objDBEntity.BusinessUnits on modelOutput.BusinessUnitID equals bu.BusinessUnitID
        //                                                 where modelOutput.PartnerID == OutputResult.PartnerID && modelOutput.BusinessUnitID == modelOut.BusinessUnitID
        //                                                 select new SellOut
        //                                                 {
        //                                                     Last_Period_Sellout = modelOutput.Last_Period_Sellout,
        //                                                     Projected_Sellout = modelOutput.Projected_Sellout,
        //                                                     YoY_change_sellout = modelOutput.YoY_change_sellout,
        //                                                     BusinessUnitID = bu.BusinessUnitID
        //                                                 }
        //                                                 ),

        //                                      MDF = (from modelMDF in objDBEntity.ModelOutputTables
        //                                             join partBudget in objDBEntity.PartnerBudgets on modelMDF.PartnerID equals partBudget.PartnerID
        //                                             join partBUBudget in objDBEntity.PartnerBUBudgets on partBudget.PartnerBudgetID equals partBUBudget.PartnerBudgetID
        //                                             join variance in objDBEntity.MDFVarianceReasons on partBUBudget.MDFVarianceReasonID equals variance.MDFVarianceReasonID into varGroup
        //                                             from varGrp in varGroup.DefaultIfEmpty()
        //                                             where modelMDF.PartnerID == OutputResult.PartnerID && modelMDF.BusinessUnitID == OutputResult.BusinessUnitID && partBUBudget.BusinessUnitID == OutputResult.BusinessUnitID
        //                                             select new MDF
        //                                             {
        //                                                 PartnerBUBudgetID = partBUBudget.PartnerBUBudgetID,
        //                                                 Last_Period_MDF = modelMDF.Last_Period_MDF ?? 0,
        //                                                 Recommended_MDF = modelMDF.Recommended_MDF ?? 0,
        //                                                 YoY_change_MDF = modelMDF.YoY_change_MDF ?? 0,
        //                                                 BaseLineMDF = partBUBudget.Baseline_MDF ?? 0,
        //                                                 ReasonForVariance = varGrp.Reason,
        //                                                 Comment = partBUBudget.Comments,
        //                                                 MDFVarianceReasonID = varGrp.MDFVarianceReasonID
        //                                             }
        //                                             ),

        //                                      MDFOrSellout = (from modelMDFSell in objDBEntity.ModelOutputTables
        //                                                      where modelMDFSell.PartnerID == OutputResult.PartnerID && modelMDFSell.BusinessUnitID == OutputResult.BusinessUnitID
        //                                                      select new MDFOrSellout
        //                                                      {
        //                                                          LastYearMDFOrSellout = modelMDFSell.Last_Period_MDF,
        //                                                          ProjectedMDFOrSellout = modelMDFSell.Recommended_MDF,
        //                                                          MedianAvdMDFOrSellout = modelMDFSell.Median_Avg_MDF_Sellout,
        //                                                          ProductivityImprovementPer = modelMDFSell.Projected_Productivity
        //                                                      }
        //                                                      ),

        //                                      Analysis = (from modelAnalysis in objDBEntity.ModelOutputTables
        //                                                  where modelAnalysis.PartnerID == OutputResult.PartnerID && modelAnalysis.BusinessUnitID == OutputResult.BusinessUnitID
        //                                                  select new Analysis
        //                                                  {
        //                                                      MDFAllignment = modelAnalysis.MDF_Alignment,
        //                                                      AssesmentOfLastYearMDF = modelAnalysis.PREV_MDF_Assessment,
        //                                                      PredictionAccuracy = modelAnalysis.Prediction_Accuracy
        //                                                  }
        //                                                  ),

        //                                      MSA = (from partBudget in objDBEntity.PartnerBudgets
        //                                             join buBudget in objDBEntity.PartnerBUBudgets on partBudget.PartnerBudgetID equals buBudget.PartnerBudgetID
        //                                             where partBudget.PartnerID == OutputResult.PartnerID && buBudget.BusinessUnitID == OutputResult.BusinessUnitID
        //                                             select new MSA
        //                                             {
        //                                                 MSAValue = partBudget.MSA ?? 0,
        //                                                 RecommendedMSA = partBudget.MSA ?? 0,
        //                                                 PartnerBudgetID = partBudget.PartnerBudgetID
        //                                             }
        //                                            ),
        //                                      PBM = OutputResult.PBM,
        //                                      PMM = OutputResult.PMM,
        //                                      PlannedSales = OutputResult.Planned_Sales,
        //                                      targetAchievement = OutputResult.Target_Achievement,
        //                                      SOW = OutputResult.SOW,
        //                                      SOWGrowth = OutputResult.SOW_Growth,
        //                                      FootprintGrowth = OutputResult.Footprint_Growth,
        //                                      No_of_end_customers = OutputResult.No_of_end_customers,
        //                                      Total_MDF = OutputResult.Total_MDF,
        //                                      Incremental_MDF = OutputResult.Incremental_MDF,
        //                                      Late_MDF = OutputResult.Late_MDF,
        //                                      W_MGO_Marketing_MDF = OutputResult.W_MGO_Marketing_MDF,
        //                                      New_Logos_MGO = OutputResult.New_Logos_MGO,

        //                                      HistoryMDF = (from hist in objDBEntity.PartnerSales
        //                                                    where hist.CountryID == CountryID && hist.PartnerTypeID == PartnerTypeID && hist.PartnerID == OutputResult.PartnerID //&& hist.BusinessUnit == OutputResult.Business_Unit 
        //                                                    select new HistoryMDF
        //                                                    {
        //                                                        FinancialYear = hist.FinancialYear,
        //                                                        MDF_1H = hist.MDF_1H,
        //                                                        MDF_2H = hist.MDF_2H
        //                                                    }
        //                                                              ),
        //                                      HistorySellout = (from sell in objDBEntity.PartnerSales
        //                                                        where sell.CountryID == CountryID && sell.PartnerTypeID == PartnerTypeID && sell.PartnerID == OutputResult.PartnerID //&& sell.BusinessUnit == OutputResult.Business_Unit
        //                                                        select new HistorySellout
        //                                                        {
        //                                                            FinancialYear = sell.FinancialYear,
        //                                                            SellOut_Q1 = sell.SellOut_Q1,
        //                                                            SellOut_Q2 = sell.SellOut_Q2,
        //                                                            SellOut_Q3 = sell.SellOut_Q3,
        //                                                            SellOut_Q4 = sell.SellOut_Q4
        //                                                        }
        //                                                             )
        //                                  }
        //                                            ),
        //                                  });

        //        var FinalOutput = (from part in Partners
        //                           join cntry in CountryPartners on part.ModelParameterID equals cntry.ModelParameterID
        //                           select part
        //                          ).AsQueryable();


        //        return FinalOutput;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}

        public IQueryable GetMDFVarianceReasonDetails()
        {
            try
            {
                var MDFVarReasons = (from reasons in objDBEntity.MDFVarianceReasons
                                     where reasons.IsActive == true
                                     select new VarianceReasonsVM
                                     {
                                         MDFVarianceReasonID = reasons.MDFVarianceReasonID,
                                         ShortName = reasons.ShortName,
                                         Reason = reasons.Reason
                                     }).AsQueryable();
                return MDFVarReasons;
            }
            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

                return null;

            }
            catch (Exception ex)
            {

                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }

        }


        // Need to Change
        [HttpGet]
        [Route("api/PartnerBudget/GetFocusAreaDetails")]
        public IQueryable GetFocusAreaDetails(short? CountryID)
        {
            try
            {
                var FocusAreas = from focus in objDBEntity.FocusedAreas
                                 select new FocusAreasVM
                                 {
                                     FocusedAreaID = focus.FocusedAreaID,
                                     ShortName = focus.ShortName,
                                     FocusedArea = focus.FocusedArea1,
                                     IsActive = focus.IsActive
                                 };
                return FocusAreas.AsQueryable();
            }
            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

                return null;

            }
            catch (Exception ex)
            {

                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }

        [HttpPost]
        [Route("api/PartnerBudget/CreateUpdatePartnerBUBudget")]
        public IHttpActionResult CreateUpdatePartnerBUBudget([FromBody]DecisionMDF DecMDF, bool IsFrom_Round2)
        {
            try
            {
                PartnerBUBudget objPartBU = new PartnerBUBudget();
                objPartBU.PartnerBUBudgetID = DecMDF.PartnerBUBudgetID;
                objPartBU.PartnerBudgetID = DecMDF.PartnerBudgetID;
                objPartBU.BusinessUnitID = DecMDF.BusinessUnitID;
                objPartBU.MDFVarianceReasonID = DecMDF.MDFVarianceReasonID;
                //objPartBU.FocusedAreaID = DecMDF.FocusedAreaID;
                objPartBU.TotalMDF = DecMDF.TotalMDF;
                objPartBU.Baseline_MDF = DecMDF.TotalMDF + DecMDF.BU_MSA;
                objPartBU.Comments = DecMDF.Comments;
                objPartBU.Additional_MDF = DecMDF.Additional_MDF;
                objPartBU.Additional_MDF_Reason = DecMDF.Additional_MDF_Reason;

                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                DateTime createdDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                
                var IsPartBudget = objDBEntity.PartnerBUBudgets.Find(objPartBU.PartnerBUBudgetID);
                if (IsPartBudget == null)
                {
                    objPartBU.CreatedBy = DecMDF.UserID;
                    objPartBU.CreatedDate = createdDate;
                    objPartBU.ModifiedBy = DecMDF.UserID;
                    objPartBU.ModifiedDate = createdDate;
                    objDBEntity.PartnerBUBudgets.Add(objPartBU);
                }
                else
                {
                    if (IsFrom_Round2 == false)
                    {
                        IsPartBudget.ModifiedBy = DecMDF.UserID;
                        IsPartBudget.ModifiedDate = createdDate;
                        IsPartBudget.Comments = DecMDF.Comments;
                        IsPartBudget.TotalMDF = DecMDF.TotalMDF;
                        IsPartBudget.Baseline_MDF = DecMDF.TotalMDF + DecMDF.BU_MSA;
                        IsPartBudget.MDFVarianceReasonID = DecMDF.MDFVarianceReasonID > 0 ? DecMDF.MDFVarianceReasonID : IsPartBudget.MDFVarianceReasonID;

                    }
                    else
                    {
                        IsPartBudget.ModifiedBy = DecMDF.UserID;
                        IsPartBudget.ModifiedDate = createdDate;
                        //IsPartBudget.Additional_MDF = DecMDF.Additional_MDF > 0 ? DecMDF.Additional_MDF : IsPartBudget.Additional_MDF;
                        IsPartBudget.Additional_MDF = DecMDF.Additional_MDF ;
                        IsPartBudget.Additional_MDF_Reason = DecMDF.Additional_MDF_Reason != null ? DecMDF.Additional_MDF_Reason : IsPartBudget.Additional_MDF_Reason;

                    }

                }
                  objDBEntity.SaveChanges();
                 var MDFPlanning = objDBEntity.MDFPlannings.Find(DecMDF.VersionID);
                if (MDFPlanning != null)
                {
                    MDFPlanning.ModifiedBy = DecMDF.UserID;
                    MDFPlanning.ModifiedDate = createdDate;

                    objDBEntity.Entry(MDFPlanning).State = EntityState.Modified;
                    objDBEntity.SaveChanges();
                }
                return Ok();
            }
            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

                return null;

            }
            catch (Exception ex)
            {

                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }

        }

        [HttpPost]
        [Route("api/PartnerBudget/CreateUpdatePartnerBudget")]
        public IHttpActionResult CreateUpdatePartnerBudget([FromBody]PartnerBudget partnerBudget)
       {
            try
            {
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                DateTime createdDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                var IsPartBudget = objDBEntity.PartnerBudgets.Find(partnerBudget.PartnerBudgetID);
                //if (IsPartBudget == null)
                //{
                //    objDBEntity.PartnerBudgets.Add(partnerBudget);
                //}
                if (IsPartBudget != null)
                {
                    IsPartBudget.MSA = partnerBudget.MSA;
                    IsPartBudget.Aruba_MSA = partnerBudget.Aruba_MSA;
                    objDBEntity.SaveChanges();
                }
 
                var MDFPlanning = objDBEntity.MDFPlannings.Find(partnerBudget.VersionID);
                if(MDFPlanning != null)
                { 
                    MDFPlanning.ModifiedBy = partnerBudget.ModifiedBy;
                    MDFPlanning.ModifiedDate = createdDate;

                    objDBEntity.Entry(MDFPlanning).State = EntityState.Modified;
                    objDBEntity.SaveChanges();
                }
                return Ok();
            }
            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

                return null;

            }
            catch (Exception ex)
            {

                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }

        }

        [HttpPost]
        [Route("api/PartnerBudget/UpdatePartnerBUBudget")]
        public IHttpActionResult UpdatePartnerBUBudget(PartnerBUBudgetViewModel partnerBUBudget)
        {
            try
            {
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                DateTime createdDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                var IsPartBUBudget = objDBEntity.PartnerBUBudgets.Find(partnerBUBudget.PartnerBUBudgetID);

                if (IsPartBUBudget != null)
                {
                    IsPartBUBudget.CarveOut = partnerBUBudget.CarveOut ?? 0;
                    IsPartBUBudget.MSA = partnerBUBudget.BU_MSA ?? 0;
                    IsPartBUBudget.TotalMDF = partnerBUBudget.TotalMDF ?? 0 ; //Column InterChange
                    IsPartBUBudget.Baseline_MDF = partnerBUBudget.TotalMDF ?? 0  + partnerBUBudget.BU_MSA ?? 0 ; 
                    objDBEntity.SaveChanges();
                }

                var MDFPlanning = objDBEntity.MDFPlannings.Find(partnerBUBudget.VersionID);
                if (MDFPlanning != null)
                {
                    MDFPlanning.ModifiedBy = partnerBUBudget.UserID;
                    MDFPlanning.ModifiedDate = createdDate;
                    objDBEntity.Entry(MDFPlanning).State = EntityState.Modified;
                    objDBEntity.SaveChanges();
                }
                return Ok();
            }
            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

                return null;

            }
            catch (Exception ex)
            {

                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }

        }

        [HttpGet]
        [Route("api/PartnerBudget/GetFilterColumns")]
        public IHttpActionResult GetFilterColumns(int VersionID, bool ExtendedView,bool Round2)
        {
            try
            {
                using (var MPOWREntity = new MPOWREntities())
                {
                    //var filter = (from result in MPOWREntity.usp_GetFilterColumns(VersionID, ExtendedView)

                    //                 select new
                    //                 {
                    //                     FilterColumnNames = result.FilterColumnNames,
                    //                     TableColumn = result.TableColumn
                    //                 }).ToList();


                    //var reader = command.ExecuteReader();
                    var Result = string.Empty;
                    DbCommand command = MPOWREntity.Database.Connection.CreateCommand();
                    command.CommandText = "usp_GetFilterColumns";
                    command.Parameters.Add(new SqlParameter("@V_ID", VersionID));
                    command.Parameters.Add(new SqlParameter("@ExpandedView", ExtendedView));
                    command.Parameters.Add(new SqlParameter("@Round2", Round2));

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    MPOWREntity.Database.Connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (Result == string.Empty)
                            {
                                Result = reader.GetValue(0).ToString();
                            }
                            else
                            {
                                Result = Result + reader.GetValue(0).ToString();
                            }
                        }
                    }
                    reader.Close();
                    Object json;
                    if (Result == null || Result == "")
                    {
                        json = Result;
                    }
                   else { json = JToken.Parse(Result); }


                    MPOWREntity.Database.Connection.Close();

                    return Ok(json);
                }
            }

            catch (Exception ex)
            {

                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }

        }
    }



}
