using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using MPOWR.Dal.Models;
using MPOWR.Api.ViewModel;
using System.Data.SqlClient;
using System.Data.Common;
using MPOWR.Api.App_Start;
using MPOWR.Core;
using MPOWR.Model;
using MPOWR.Dal;
using System.Data.Entity;

namespace MPOWR.Api.Controllers
{
    [AuthorizeUser]
    public class HistoricPerformanceController : ApiController
    {
        private MPOWREntities db = new MPOWREntities();
        Historical historical = new Historical();
        HistoricPerformanceViewModel historicvm = new HistoricPerformanceViewModel();

        // GET: api/HistoricPerformance
        //public IQueryable<PartnerSale> GetPartnerSales()
        //{
        //    return db.PartnerSales;
        //}
        [HttpGet]
        public IQueryable<Businessunits> GetBusinessUnits()
        {
            try
            {
                var BUs = (from BU in db.BusinessUnits where BU.IsActive == true select new Businessunits { ID = BU.BusinessUnitID, Name = BU.DisplayName }).AsNoTracking().AsQueryable();
                return BUs;
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
        public List<buunit> GetBusinessUnitByVersion(int VersionID)
        {
            try
            {

                string countryList = string.Empty;
                List<buunit> BUList = (from BUs in db.BusinessUnits where BUs.IsActive == true select new buunit { ID = BUs.BusinessUnitID, Name = BUs.DisplayName }).ToList();
                List<buunit> BUListInActive = (from BUs in db.BusinessUnits where BUs.IsActive == false select new buunit { BusinessUnitID = BUs.BusinessUnitID, DisplayName = BUs.DisplayName, ID = BUs.BusinessUnitID, Name = BUs.DisplayName }).ToList();
                var BUListToAdd = (from InActive in BUListInActive
                                  join BU in db.BUBudgets on InActive.ID equals BU.BusinessUnitID
                                  where BU.VersionID == VersionID
                                  select new buunit { BusinessUnitID = InActive.BusinessUnitID, DisplayName = InActive.DisplayName, ID = InActive.ID, Name = InActive.Name }).ToList();
                
                var IsCompute = db.Database.SqlQuery<bool>("select dbo.GetComputeFlag(" + VersionID + ")").FirstOrDefault();
                var planning = (from plan in db.MDFPlannings where   plan.ID == VersionID select plan).ToList(); // select country.CountryID.ToString()).ToList();
                if (planning != null && planning.Count != 0)
                {
                    //if (planning[0].AllocationLevel == "D")
                    //{
                    //    //isComputeBUonly = true;
                    //    BUList = (from BUs in db.BusinessUnits where BUs.IsActive == true && BUs.DisplayName != MPOWRConstants.ComputeVolume && BUs.DisplayName != MPOWRConstants.ComputeValue select new buunit { ID = BUs.BusinessUnitID, Name = BUs.DisplayName }).ToList();
                    //}
                    //if (planning[0].AllocationLevel == "C")
                    //{
                    //    countryList = planning[0].CountryOrGeoOrDistrict;
                    //    List<string> CountriesList = countryList.Split(',').ToList();
                    //    using (CountryDAL dal = new CountryDAL())
                    //    {
                    //        BUList = dal.GetBUFromCountries(ComputeCountryList.ToList<dynamic>(), CountriesList.ToList<dynamic>());
                    //    }
                    //    foreach (var list in BUList)
                    //    {
                    //        list.ID = list.BusinessUnitID;
                    //        list.Name = list.DisplayName;
                    //    }
                    //}
                    //if (planning[0].AllocationLevel == "G")
                    //{
                    //    countryList = planning[0].CountryOrGeoOrDistrict;
                    //   var compGeoList = (from country in db.AppConfigs where country.ShortName == MPOWRConstants.IsCompute select new { country.Params }).ToList();
                    //    if (compGeoList[0].Params.Split(',').Contains(countryList))
                    //    {
                    //        BUList = (from BUs in db.BusinessUnits where BUs.IsActive == true && BUs.DisplayName != MPOWRConstants.ComputeVolume && BUs.DisplayName != MPOWRConstants.ComputeValue select new buunit { ID = BUs.BusinessUnitID, Name = BUs.DisplayName }).ToList();
                    //    }
                    //    else
                    //    {
                    //        BUList = (from BUs in db.BusinessUnits where BUs.IsActive == true && BUs.DisplayName != MPOWRConstants.Compute select new buunit { ID = BUs.BusinessUnitID, Name = BUs.DisplayName }).ToList();
                    //    }
                    //}
                    if (IsCompute)
                    {
                        BUList = (from BUs in db.BusinessUnits where BUs.DisplayName != MPOWRConstants.ComputeVolume && BUs.DisplayName != MPOWRConstants.ComputeValue && BUs.IsActive == true select new buunit { ID = BUs.BusinessUnitID, Name = BUs.DisplayName }).ToList();
                    }
                    else
                    {
                        BUList = (from BUs in db.BusinessUnits where BUs.DisplayName != MPOWRConstants.Compute && BUs.IsActive == true select new buunit { ID = BUs.BusinessUnitID, Name = BUs.DisplayName }).ToList();
                    }
                }
                if(BUListToAdd.Count > 0 )
                {
                    BUList.AddRange(BUListToAdd);
                }
                return BUList;
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
        public IHttpActionResult HistoricPerformanceData(HistoricalVM historic)
        {

            string sp = "";
            if (historic.PartnerTypeID == 2)
            {
                   sp = "usp_GetHistoricalPerformanceData_Reslr ";
            }
            else
            {
                sp = "usp_GetHistoricalPerformanceData_Dist ";
            }

            DbCommand command = db.Database.Connection.CreateCommand();
            command.CommandText = sp;
            command.Parameters.Add(new SqlParameter("PartnerType", Convert.ToInt32(historic.PartnerTypeID)));
            command.Parameters.Add(new SqlParameter("@BUs", historic.BUs));
            command.Parameters.Add(new SqlParameter("@MDF", historic.MDF));
            command.Parameters.Add(new SqlParameter("@NotMDF", historic.NotMDF));
            command.Parameters.Add(new SqlParameter("@Version", historic.VersionID));
            //command.Parameters.Add(new SqlParameter("@Country", historic.CountryID));
            //command.Parameters.Add(new SqlParameter("@District", historic.DistrictID));
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.CommandTimeout = 1000000;
            

            try
            {
                db.Database.Connection.Open();
               
                var reader = command.ExecuteReader();
               
                historicvm.Historicdata =
                ((IObjectContextAdapter)db).ObjectContext.Translate<Historical>
                (reader).ToList();
                reader.NextResult();
                historicvm.MDFData =
                    ((IObjectContextAdapter)db).ObjectContext.Translate<MDFCollection>
            (reader).ToList();

                reader.Close();
                db.Database.Connection.Close();


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
            finally
            {
                db.Database.Connection.Close();
            }
            List<Historical> HistoricPerformanceData;
            db.Database.CommandTimeout = 1000000;
            if (historic.PartnerTypeID == 2)
            {
                HistoricPerformanceData = (from HD in historicvm.Historicdata
                                           select new Historical
                                           {
                                               Name = HD.BGMembership ,
                                               LastPeriodSellout = HD.LastPeriodSellout,
                                               LastPeriodSelloutGrowth = HD.LastPeriodSelloutGrowth,
                                               LastPeriodSelloutGrowthPer = HD.LastPeriodSelloutGrowthPer,
                                               LastPeriodMDF = HD.LastPeriodMDF,
                                               OverallROI = HD.OverallROI,
                                               rows = from mdf in historicvm.MDFData
                                                      where mdf.BGMembership == HD.BGMembership
                                                      select new MDFCollection
                                                      {
                                                          BusinessUnit = mdf.BusinessUnit,
                                                          BusinessUnitID = mdf.BusinessUnitID,
                                                          FinancialYear = mdf.FinancialYear,
                                                          SellOut = mdf.SellOut,
                                                          SELLOUTQ1 = mdf.SELLOUTQ1,
                                                          SELLOUTQ2 = mdf.SELLOUTQ2,
                                                          SELLOUTQ3 = mdf.SELLOUTQ3,
                                                          SELLOUTQ4 = mdf.SELLOUTQ4,
                                                          SellOut_1H=mdf.SellOut_1H,
                                                          SellOut_2H=mdf.SellOut_2H,
                                                          MDF_1H = mdf.MDF_1H,
                                                          MDF_2H = mdf.MDF_2H
                                                      }
                                           }).ToList();
            }
            else
            {
                HistoricPerformanceData = (from HD in historicvm.Historicdata
                                           select new Historical
                                           {
                                               Name = HD.PartnerName,
                                               SellOutForSilverAndBelow = HD.SellOutForSilverAndBelow,
                                               SellOutForPlatinumAndGold = HD.SellOutForPlatinumAndGold,
                                               LastPeriodSellout = HD.LastPeriodSellout,
                                               LastPeriodSelloutGrowth = HD.LastPeriodSelloutGrowth,
                                               LastPeriodSelloutGrowthPer = HD.LastPeriodSelloutGrowthPer,
                                               LastPeriodMDF = HD.LastPeriodMDF,
                                               OverallROI = HD.OverallROI,
                                               rows = from mdf in historicvm.MDFData
                                                      where mdf.PartnerName == HD.PartnerName
                                                      select new MDFCollection
                                                      {
                                                          BusinessUnit = mdf.BusinessUnit,
                                                          BusinessUnitID = mdf.BusinessUnitID,
                                                          FinancialYear = mdf.FinancialYear,
                                                          SellOut = mdf.SellOut,
                                                          SELLOUTQ1 = mdf.SELLOUTQ1,
                                                          SELLOUTQ2 = mdf.SELLOUTQ2,
                                                          SELLOUTQ3 = mdf.SELLOUTQ3,
                                                          SELLOUTQ4 = mdf.SELLOUTQ4,
                                                          SellOut_1H = mdf.SellOut_1H,
                                                          SellOut_2H = mdf.SellOut_2H,
                                                          MDF_1H = mdf.MDF_1H,
                                                          MDF_2H = mdf.MDF_2H

                                                      }
                                           }).ToList();
            }
            return Ok(HistoricPerformanceData);
        }

        // DELETE: api/HistoricPerformance/5
        [ResponseType(typeof(PartnerSale))]
        public IHttpActionResult DeletePartnerSale(long id)
        {
            try
            {
                PartnerSale partnerSale = db.PartnerSales.Find(id);
                if (partnerSale == null)
                {
                    return NotFound();
                }

                db.PartnerSales.Remove(partnerSale);
                db.SaveChanges();

                return Ok(partnerSale);
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

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }
            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
               
            }
            catch (Exception ex)
            {

                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
              
            }
        }

        private bool PartnerSaleExists(long id)
        {
            try
            {
                return db.PartnerSales.Count(e => e.PartnerSalesID == id) > 0;
            }
            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
               
            }
            catch (Exception ex)
            {

                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
              
            }
            return false;
        }
    }
}