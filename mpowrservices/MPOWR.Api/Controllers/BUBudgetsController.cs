using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MPOWR.Dal.Models;
using MPOWR.Api.ViewModel;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Data.Common;
using System.Data.SqlClient;
using MPOWR.Api.Service;
using MPOWR.Dal;
using MPOWR.Api.App_Start;
using MPOWR.Api.ViewModel;
using MPOWR.Core;
using MPOWR.Model;

namespace MPOWR.Api.Controllers
{

    [AuthorizeUser]
    public class BUBudgetsController : ApiController
    {
        private MPOWREntities db = new MPOWREntities();
        BUBdgetsViewModel Budgetvm = new BUBdgetsViewModel();

        // GET: api/BUBudgets
        [HttpGet]
        public IQueryable GetBUBudgets(int VersionID,int GeoID)
        {
            try
            {
                dynamic data;

                bool isComputeBUonly = false;
                bool isAllBU = false;
                bool isOtherBU = false;
                string countryList = string.Empty;
                List<buunit> BUList = (from BUs in db.BusinessUnits where BUs.IsActive == true select new buunit { BusinessUnitID = BUs.BusinessUnitID, DisplayName = BUs.DisplayName }).ToList();
                //  var compCountryList = (from country in db.AppConfigs where country.ShortName == MPOWRConstants.ComputeCountryShortName && country.AccessLevel == (int)EnumUserType.Country select new { country.Params }).ToList();
                var IsCompute = db.Database.SqlQuery<bool>("select dbo.GetComputeFlag(" + VersionID +")").FirstOrDefault();
                //List<string> ComputeCountryList = compCountryList.Split(',').ToList();
                var planning = (from plan in db.MDFPlannings where plan.ID == VersionID select plan).ToList(); // select country.CountryID.ToString()).ToList();
                var geoConfig = db.GeoConfigDetails.Where(x => x.GeoID == GeoID).Select(x => x.IsOverAllocation).FirstOrDefault();
                if (planning != null)
                {
                    //if (planning[0].AllocationLevel == "D")
                    //{
                    //    //isComputeBUonly = true;
                    //    BUList = (from BUs in db.BusinessUnits where BUs.DisplayName != MPOWRConstants.ComputeVolume && BUs.DisplayName != MPOWRConstants.ComputeValue && BUs.IsActive == true select new buunit { BusinessUnitID = BUs.BusinessUnitID, DisplayName = BUs.DisplayName }).ToList();
                    //}
                    //if (planning[0].AllocationLevel == "C")
                    //{
                    //    countryList = planning[0].CountryOrGeoOrDistrict;
                    //    List<string> CountriesList = countryList.Split(',').ToList();
                    //    using (CountryDAL dal = new CountryDAL())
                    //    {
                    //        BUList = dal.GetBUFromCountries(ComputeCountryList.ToList<dynamic>(), CountriesList.ToList<dynamic>());
                    //    }
                    //}
                    //if (planning[0].AllocationLevel == "G")
                    //{
                    //    countryList = planning[0].CountryOrGeoOrDistrict;
                    //    var compGeoList = (from country in db.AppConfigs where country.ShortName == MPOWRConstants.IsCompute  select new { country.Params }).ToList();
                    //    if (compGeoList[0].Params.Split(',').Contains(countryList))
                    //    {
                    //        BUList = (from BUs in db.BusinessUnits where BUs.DisplayName != MPOWRConstants.ComputeVolume && BUs.DisplayName != MPOWRConstants.ComputeValue && BUs.IsActive == true select new buunit { BusinessUnitID = BUs.BusinessUnitID, DisplayName = BUs.DisplayName }).ToList();
                    //    }
                    //    else
                    //    {
                    //        BUList = (from BUs in db.BusinessUnits where BUs.DisplayName != MPOWRConstants.Compute && BUs.IsActive == true select new buunit { BusinessUnitID = BUs.BusinessUnitID, DisplayName = BUs.DisplayName }).ToList();
                    //    }
                    //}


                    if (IsCompute)
                    {
                        BUList = (from BUs in db.BusinessUnits where BUs.DisplayName != MPOWRConstants.ComputeVolume && BUs.DisplayName != MPOWRConstants.ComputeValue && BUs.IsActive == true select new buunit { BusinessUnitID = BUs.BusinessUnitID, DisplayName = BUs.DisplayName }).ToList();
                    }
                    else
                    {
                        BUList = (from BUs in db.BusinessUnits where BUs.DisplayName != MPOWRConstants.Compute && BUs.IsActive == true select new buunit { BusinessUnitID = BUs.BusinessUnitID, DisplayName = BUs.DisplayName }).ToList();
                    }




                }

                //List<BusinessUnit> buList = new List<BusinessUnit>();
                foreach (var item in BUList)
                {
                    //BusinessUnit bu = new BusinessUnit();
                    if (item.DisplayName == "Compute")
                        isComputeBUonly = true;
                    if (item.DisplayName == "Compute Volume" || item.DisplayName == "Compute Value")
                        isOtherBU = true;
                }
                if (isComputeBUonly == true && isOtherBU == true)
                    isAllBU = true;

                if (isAllBU)
                {
                    var bu = (from BU in db.BUBudgets where BU.VersionID == VersionID select BU).ToList();
                    if (bu.Count > 0)
                    {
                        data = (from partnr in db.PartnerTypes
                                from financial in db.FinancialYears
                                from mdf in db.MDFPlannings
                                where mdf.ID == VersionID && partnr.PartnerTypeID == mdf.PartnerTypeID && financial.FinancialYearID == mdf.FinancialYearID
                                // && partnr.IsActive == true && financial.IsActive == true  
                                select new CountryBudgets
                                {
                                    FinancialYearID = financial.FinancialYearID,
                                    FinancialYear = financial.ShortName,
                                    PartnerTypeID = partnr.PartnerTypeID,
                                    PartnerName = partnr.DisplayName,
                                    IsOverAllocation = geoConfig,
                                    BuUnits =
                                              from Bunit in db.BusinessUnits
                                              join BU in db.BUBudgets on Bunit.BusinessUnitID equals BU.BusinessUnitID into group1
                                              from g1 in group1.DefaultIfEmpty()
                                              where g1.VersionID == VersionID //&& Bunit.IsActive == true
                                              select new BUBudgets
                                              {
                                                  BUBudgetID = (long?)g1.BUBudgetID ?? 0,
                                                  BusinessUnitID = g1.BusinessUnitID == null ? 0 : g1.BusinessUnitID,
                                                  TotalMDF = g1.TotalMDF == null ? 0 : g1.TotalMDF,
                                                  CountryReserveMDF = g1.CountryReserveMDF == null ? 0 : g1.CountryReserveMDF,
                                                  BaselineMDF = g1.BaselineMDF == null ? 0 : g1.BaselineMDF,
                                                  BusinessUnitName = Bunit.DisplayName,
                                                  ProgramMDF = g1.ProgramMDF == null ? 0 : g1.ProgramMDF,
                                                  CarveProjects = from PMDF in db.ProgramMDFs
                                                                  where PMDF.BUBudgetID == g1.BUBudgetID
                                                                  select new ProgramMDFS
                                                                  {
                                                                      ProgramMDFID = (int?)PMDF.ProgramMDFID ?? 0,
                                                                      ProjectName = PMDF.ProjectName == null ? "" : PMDF.ProjectName,
                                                                      ProjectMDF = PMDF.ProjectMDF == null ? 0 : PMDF.ProjectMDF,
                                                                      Flag = "NU"
                                                                  }
                                              }
                                }).AsQueryable();

                    }
                    else
                    {


                        data = (
                                from partnr in db.PartnerTypes
                                from financial in db.FinancialYears
                                from mdf in db.MDFPlannings
                                where mdf.ID == VersionID && partnr.PartnerTypeID == mdf.PartnerTypeID && financial.FinancialYearID == mdf.FinancialYearID
                                 //&& partnr.IsActive == true && financial.IsActive == true  
                                select new
                                {
                                    FinancialYearID = financial.FinancialYearID,
                                    FinancialYear = financial.ShortName,
                                    PartnerTypeID = partnr.PartnerTypeID,
                                    PartnerName = "",
                                    IsOverAllocation = geoConfig,
                                    BuUnits = from Bunit in db.BusinessUnits
                                              where Bunit.IsActive == true
                                              select new BUBudgets
                                              {
                                                  BUBudgetID = 0,
                                                  BusinessUnitID = Bunit.BusinessUnitID,
                                                  TotalMDF = 0,
                                                  CountryReserveMDF = 0,
                                                  BaselineMDF = 0,
                                                  BusinessUnitName = Bunit.DisplayName,
                                                  ProgramMDF = 0,
                                                  CarveProjects = from PMDF in db.ProgramMDFs
                                                                  where PMDF.BUBudgetID == 0
                                                                  select new ProgramMDFS
                                                                  {
                                                                      ProgramMDFID = 0,
                                                                      ProjectName = "",
                                                                      ProjectMDF = 0,
                                                                      Flag = "NU"
                                                                  }


                                              }
                                }).AsQueryable();
                    }
                }
                else if (isOtherBU)
                {
                    var bu = (from BU in db.BUBudgets where BU.VersionID == VersionID select BU).ToList();
                    if (bu.Count > 0)
                    {
                        data = (from partnr in db.PartnerTypes
                                from financial in db.FinancialYears
                                from mdf in db.MDFPlannings
                                where mdf.ID == VersionID && partnr.PartnerTypeID == mdf.PartnerTypeID && financial.FinancialYearID == mdf.FinancialYearID
                                 //&& partnr.IsActive == true && financial.IsActive == true  
                                select new CountryBudgets
                                {
                                    FinancialYearID = financial.FinancialYearID,
                                    FinancialYear = financial.ShortName,
                                    PartnerTypeID = partnr.PartnerTypeID,
                                    PartnerName = partnr.DisplayName,
                                    IsOverAllocation = geoConfig,
                                    BuUnits =
                                              from Bunit in db.BusinessUnits
                                              join BU in db.BUBudgets on Bunit.BusinessUnitID equals BU.BusinessUnitID into group1

                                              from g1 in group1.DefaultIfEmpty()
                                              where g1.VersionID == VersionID //&& (Bunit.DisplayName != MPOWRConstants.Compute)
                                               //&& Bunit.IsActive == true
                                              select new BUBudgets
                                              {
                                                  BUBudgetID = (long?)g1.BUBudgetID ?? 0,
                                                  BusinessUnitID = g1.BusinessUnitID == null ? 0 : g1.BusinessUnitID,
                                                  TotalMDF = g1.TotalMDF == null ? 0 : g1.TotalMDF,
                                                  CountryReserveMDF = g1.CountryReserveMDF == null ? 0 : g1.CountryReserveMDF,
                                                  BaselineMDF = g1.BaselineMDF == null ? 0 : g1.BaselineMDF,
                                                  BusinessUnitName = Bunit.DisplayName,
                                                  ProgramMDF = g1.ProgramMDF == null ? 0 : g1.ProgramMDF,
                                                  CarveProjects = from PMDF in db.ProgramMDFs
                                                                  where PMDF.BUBudgetID == g1.BUBudgetID
                                                                  select new ProgramMDFS
                                                                  {
                                                                      ProgramMDFID = (int?)PMDF.ProgramMDFID ?? 0,
                                                                      ProjectName = PMDF.ProjectName == null ? "" : PMDF.ProjectName,
                                                                      ProjectMDF = PMDF.ProjectMDF == null ? 0 : PMDF.ProjectMDF,
                                                                      Flag = "NU"
                                                                  }
                                              }
                                }).AsQueryable();

                    }
                    else
                    {


                        data = (
                                from partnr in db.PartnerTypes
                                from financial in db.FinancialYears
                                from mdf in db.MDFPlannings
                                where mdf.ID == VersionID && partnr.PartnerTypeID == mdf.PartnerTypeID && financial.FinancialYearID == mdf.FinancialYearID
                                // && partnr.IsActive == true && financial.IsActive == true  
                                select new
                                {
                                    FinancialYearID = financial.FinancialYearID,
                                    FinancialYear = financial.ShortName,
                                    PartnerTypeID = partnr.PartnerTypeID,
                                    PartnerName = "",
                                    IsOverAllocation = geoConfig,
                                    BuUnits = from Bunit in db.BusinessUnits
                                              where (Bunit.DisplayName != MPOWRConstants.Compute) && Bunit.IsActive == true
                                              select new BUBudgets
                                              {
                                                  BUBudgetID = 0,
                                                  BusinessUnitID = Bunit.BusinessUnitID,
                                                  TotalMDF = 0,
                                                  CountryReserveMDF = 0,
                                                  BaselineMDF = 0,
                                                  BusinessUnitName = Bunit.DisplayName,
                                                  ProgramMDF = 0,
                                                  CarveProjects = from PMDF in db.ProgramMDFs
                                                                  where PMDF.BUBudgetID == 0
                                                                  select new ProgramMDFS
                                                                  {
                                                                      ProgramMDFID = 0,
                                                                      ProjectName = "",
                                                                      ProjectMDF = 0,
                                                                      Flag = "NU"
                                                                  }


                                              }
                                }).AsQueryable();
                    }
                }
                else
                {
                    var bu = (from BU in db.BUBudgets where BU.VersionID == VersionID select BU).ToList();
                    if (bu.Count > 0)
                    {
                        data = (

                                from partnr in db.PartnerTypes
                                from financial in db.FinancialYears
                                from mdf in db.MDFPlannings
                                where mdf.ID == VersionID && partnr.PartnerTypeID == mdf.PartnerTypeID && financial.FinancialYearID == mdf.FinancialYearID
                                // && partnr.IsActive == true && financial.IsActive == true  
                                select new CountryBudgets
                                {
                                    FinancialYearID = financial.FinancialYearID,
                                    FinancialYear = financial.ShortName,
                                    PartnerTypeID = partnr.PartnerTypeID,
                                    PartnerName = partnr.DisplayName,
                                    IsOverAllocation = geoConfig,
                                    BuUnits =
                                              from Bunit in db.BusinessUnits
                                              join BU in db.BUBudgets on Bunit.BusinessUnitID equals BU.BusinessUnitID into group1

                                              from g1 in group1.DefaultIfEmpty()
                                              where g1.VersionID == VersionID //&& (Bunit.DisplayName != MPOWRConstants.ComputeValue && Bunit.DisplayName != MPOWRConstants.ComputeVolume)
                                               //&&  Bunit.IsActive == true
                                              select new BUBudgets
                                              {
                                                  BUBudgetID = (long?)g1.BUBudgetID ?? 0,
                                                  BusinessUnitID = g1.BusinessUnitID == null ? 0 : g1.BusinessUnitID,
                                                  TotalMDF = g1.TotalMDF == null ? 0 : g1.TotalMDF,
                                                  CountryReserveMDF = g1.CountryReserveMDF == null ? 0 : g1.CountryReserveMDF,
                                                  BaselineMDF = g1.BaselineMDF == null ? 0 : g1.BaselineMDF,
                                                  BusinessUnitName = Bunit.DisplayName,
                                                  ProgramMDF = g1.ProgramMDF == null ? 0 : g1.ProgramMDF,
                                                  CarveProjects = from PMDF in db.ProgramMDFs
                                                                  where PMDF.BUBudgetID == g1.BUBudgetID
                                                                  select new ProgramMDFS
                                                                  {
                                                                      ProgramMDFID = (int?)PMDF.ProgramMDFID ?? 0,
                                                                      ProjectName = PMDF.ProjectName == null ? "" : PMDF.ProjectName,
                                                                      ProjectMDF = PMDF.ProjectMDF == null ? 0 : PMDF.ProjectMDF,
                                                                      Flag = "NU"
                                                                  }
                                              }
                                }).AsQueryable();

                    }
                    else
                    {


                        data = (
                                from partnr in db.PartnerTypes
                                from financial in db.FinancialYears
                                from mdf in db.MDFPlannings
                                where mdf.ID == VersionID && partnr.PartnerTypeID == mdf.PartnerTypeID && financial.FinancialYearID == mdf.FinancialYearID
                                 //&& partnr.IsActive == true && financial.IsActive == true  
                                select new
                                {
                                    FinancialYearID = financial.FinancialYearID,
                                    FinancialYear = financial.ShortName,
                                    PartnerTypeID = partnr.PartnerTypeID,
                                    PartnerName = "",
                                    IsOverAllocation = geoConfig,
                                    BuUnits =
                                              from Bunit in db.BusinessUnits
                                              where (Bunit.DisplayName != MPOWRConstants.ComputeValue && Bunit.DisplayName != MPOWRConstants.ComputeVolume)
                                              && Bunit.IsActive == true
                                              select new BUBudgets
                                              {
                                                  BUBudgetID = 0,
                                                  BusinessUnitID = Bunit.BusinessUnitID,
                                                  TotalMDF = 0,
                                                  CountryReserveMDF = 0,
                                                  BaselineMDF = 0,
                                                  BusinessUnitName = Bunit.DisplayName,
                                                  ProgramMDF = 0,
                                                  CarveProjects = from PMDF in db.ProgramMDFs
                                                                  where PMDF.BUBudgetID == 0
                                                                  select new ProgramMDFS
                                                                  {
                                                                      ProgramMDFID = 0,
                                                                      ProjectName = "",
                                                                      ProjectMDF = 0,
                                                                      Flag = "NU"
                                                                  }


                                              }
                                }).AsQueryable();
                    }
                }


                 return data;
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
            return null;


        }

        // Add & Update : api/BUBudgets/BudgetVM
        // [ResponseType(typeof(BUBdgetsViewModel))]
        [Route("api/BUBudgets/AddUpdateBUBudget")]
        public IHttpActionResult AddUpdateBUBudget(BUBdgetsViewModel BudgetVM)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            DateTime createdDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            foreach (var BuUnit in BudgetVM.BuUnits)
            {
                // var FYR = (from dt in db.FinancialYears where dt.ShortName.Contains(BudgetVM.FinancialYear) select new { FinancialYearID = dt.FinancialYearID }).FirstOrDefault();
                var bUBdget = db.BUBudgets.Find(BuUnit.BUBudgetID);
                if (bUBdget == null)
                {
                    BUBudget bUBdgett = new BUBudget();
                    bUBdgett.BaselineMDF = BuUnit.BaselineMDF;
                    bUBdgett.BusinessUnitID = BuUnit.BusinessUnitID;
                    bUBdgett.TotalMDF = BuUnit.TotalMDF;
                    bUBdgett.ProgramMDF = BuUnit.ProgramMDF;
                    bUBdgett.CountryReserveMDF = BuUnit.CountryReserveMDF;
                    bUBdgett.CreatedBy = BudgetVM.UserID;
                    bUBdgett.CreatedDate = createdDate;
                    bUBdgett.Status = BuUnit.Status;
                    bUBdgett.VersionID = BudgetVM.VersionID;
                    db.BUBudgets.Add(bUBdgett);
                    bUBdgett.ModifiedBy = BudgetVM.UserID;
                    bUBdgett.ModifiedDate = createdDate;

                    try
                    {
                        db.SaveChanges();
                        BuUnit.BUBudgetID = bUBdgett.BUBudgetID;
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        if (!BUBudgetExists(BuUnit.BUBudgetID))
                        {

                            MPOWRLogManager.LogMessage(ex.Message.ToString());
                            MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                }
                else
                {
                    bUBdget.BaselineMDF = BuUnit.BaselineMDF;
                    bUBdget.BusinessUnitID = BuUnit.BusinessUnitID;
                    bUBdget.TotalMDF = BuUnit.TotalMDF;
                    bUBdget.ProgramMDF = BuUnit.ProgramMDF;
                    bUBdget.CountryReserveMDF = BuUnit.CountryReserveMDF;
                    bUBdget.ModifiedBy = BudgetVM.UserID;
                    bUBdget.ModifiedDate = createdDate; 
                    bUBdget.Status = bUBdget.Status;
                    bUBdget.VersionID = BudgetVM.VersionID;
                    db.Entry(bUBdget).State = EntityState.Modified;
                    //try
                    //{
                    //    db.SaveChanges();
                    //}
                    //catch (DbUpdateConcurrencyException ex)
                    //{
                    //    if (!BUBudgetExists(BuUnit.BUBudgetID))
                    //    {

                    //        MPOWRLogManager.LogMessage(ex.Message.ToString());
                    //        MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                    //        return NotFound();
                    //    }
                    //    else
                    //    {
                    //        throw;
                    //    }
                    //}
                }
                foreach (var dat in BuUnit.CarveProjects)
                {
                    ProgramMDF PMDF = db.ProgramMDFs.Find(dat.ProgramMDFID);

                    if (PMDF == null && dat.Flag == "IN")
                    {

                        ProgramMDF programmdf = new ProgramMDF();
                        programmdf.BUBudgetID = BuUnit.BUBudgetID;
                        programmdf.ProjectName = dat.ProjectName;
                        programmdf.ProjectMDF = dat.ProjectMDF;
                        programmdf.CreatedBy = BudgetVM.UserID;
                        programmdf.CreatedDate = createdDate;
                        programmdf.ModifiedBy = BudgetVM.UserID;
                        programmdf.ModifiedDate = createdDate;
                        db.ProgramMDFs.Add(programmdf);
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            if (!BUBudgetExists(BuUnit.BUBudgetID))
                            {

                                MPOWRLogManager.LogMessage(ex.Message.ToString());
                                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                    else if (PMDF != null && dat.Flag == "UP")
                    {
                        PMDF.BUBudgetID = BuUnit.BUBudgetID;
                        PMDF.ProjectName = dat.ProjectName;
                        PMDF.ProjectMDF = dat.ProjectMDF;
                        PMDF.ModifiedBy = BudgetVM.UserID;
                        PMDF.ModifiedDate = createdDate;
                        db.Entry(PMDF).State = EntityState.Modified;
                        //try
                        //{
                        //    db.SaveChanges();
                        //}
                        //catch (DbUpdateConcurrencyException ex)
                        //{
                        //    if (!BUBudgetExists(BuUnit.BUBudgetID))
                        //    {

                        //        MPOWRLogManager.LogMessage(ex.Message.ToString());
                        //        MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                        //        return NotFound();
                        //    }
                        //    else
                        //    {
                        //        throw;
                        //    }
                        //}
                    }
                    else if (PMDF != null && dat.Flag == "DL")
                    {
                        db.ProgramMDFs.Remove(PMDF);
                        //try
                        //{
                        //    db.SaveChanges();
                        //}
                        //catch (DbUpdateConcurrencyException ex)
                        //{
                        //    if (!BUBudgetExists(BuUnit.BUBudgetID))
                        //    {

                        //        MPOWRLogManager.LogMessage(ex.Message.ToString());
                        //        MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                        //        return NotFound();
                        //    }
                        //    else
                        //    {
                        //        throw;
                        //    }
                        //}
                    }
                   
                }
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!BUBudgetExists(BuUnit.BUBudgetID))
                    {

                        MPOWRLogManager.LogMessage(ex.Message.ToString());
                        MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            RProgram(BudgetVM);
            return Ok();
        }

        public void RProgram(BUBdgetsViewModel BudgetVM)
        {

            DbCommand command = db.Database.Connection.CreateCommand();
            command.CommandText = "HPE_MDF_RMODEL";
            command.Parameters.Add(new SqlParameter("@V_ID", Convert.ToInt32(BudgetVM.VersionID)));
            command.Parameters.Add(new SqlParameter("@USER_ID", BudgetVM.UserID));
            command.Parameters.Add(new SqlParameter("@ISDEFAULTMP", true));
            command.CommandTimeout = 1000;
            command.CommandType = System.Data.CommandType.StoredProcedure;

            try
            {
                db.Database.Connection.Open();

                var reader = command.ExecuteReader();


                var result = ((IObjectContextAdapter)db).ObjectContext.Translate<dynamic>
                  (reader);
                reader.NextResult();



            }
            catch (Exception ex)
            {

                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
            finally
            {
                db.Database.Connection.Close();
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

        private bool BUBudgetExists(long id)
        {
            try
            {
                return db.BUBudgets.Count(e => e.BUBudgetID == id) > 0;
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

        /// <summary>
        /// Get all financialYear
        /// </summary>
        /// <param name="Financial"></param>
        /// <returns></returns>

        [Route("api/BUBudgets/GetFinancialYear")]
        [HttpPost]
        public List<GetFinancial> GetFinancialyear(GetFinancial Financial)
        {
            try
            {
                using (MPOWREntities db = new MPOWREntities())
                {
                    List<GetFinancial> FinacialYear = new List<GetFinancial>();
                    var version_FY = (from data in db.MDFPlannings   select data.FinancialYearID).Distinct().ToList();

                    Financial.FinancialyearID = db.Database.SqlQuery<int>("select dbo.GetPlanningFinancialPeriodId()").FirstOrDefault();

                    List<int> Year = new List<int>();
                    Year.AddRange(version_FY);
                    Year.Add(Financial.FinancialyearID);
                    Year.Add(Financial.FinancialyearID + 1);
                    if (Financial.AllocationLevel == null || Financial.AllocationLevel =="")
                    {
                        FinacialYear = (from dat in db.FinancialYears
                                        where Year.Contains(dat.FinancialYearID) //&& dat.IsActive == true
                                        select new GetFinancial
                                        {
                                            FinancialyearID = dat.FinancialYearID,
                                            Financialyear = dat.ShortName,
                                            IsActive = dat.IsActive
                                        }).OrderBy(x => x.FinancialyearID).ToList();
                    }

                    else
                    {
                        FinacialYear = (from dat in db.FinancialYears
                                        where Year.Contains(dat.FinancialYearID)
                                        select new GetFinancial
                                        {
                                            FinancialyearID = dat.FinancialYearID,
                                            Financialyear = dat.ShortName,
                                            IsActive = dat.IsActive,
                                            Version = (from ver in db.MDFPlannings
                                                       where ver.FinancialYearID == dat.FinancialYearID && ver.CountryOrGeoOrDistrict == Financial.CountryOrGeoOrDistrict
                                                       && ver.AllocationLevel == Financial.AllocationLevel
                                                       && ver.PartnerTypeID == Financial.PartnerTypeID
                                                       && ver.MembershipGroupID == Financial.MembershipGroupID
                                                       select new Versions
                                                       {
                                                           VersionID = ver.ID,
                                                           VersionNo = ver.VersionNo,
                                                           VersionName = ver.VersionName,
                                                           IsFinal = ver.IsFinal,
                                                           MembershipGroupID = ver.MembershipGroupID
                                                           
                                                       }).OrderBy(x => x.VersionID)
                                        }).OrderBy(x => x.FinancialyearID).ToList();
                    }

                    return FinacialYear;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                throw new MPOWRException(message, ex);
            }
        }

        [Route("api/BUBudgets/GetFinancialyearSearch")]
        [HttpPost]
        public List<GetFinancial> GetFinancialyearSearch(GetFinancial Financial)
        {
            try
            {
                using (MPOWREntities db = new MPOWREntities())
                {
                    List<GetFinancial> FinacialYear = new List<GetFinancial>();
                    var version_FY = (from data in db.MDFPlannings   select data.FinancialYearID).Distinct().ToList();

                    Financial.FinancialyearID = db.Database.SqlQuery<int>("select dbo.GetPlanningFinancialPeriodId()").FirstOrDefault();

                    List<int> Year = new List<int>();
                    Year.AddRange(version_FY);
                    Year.Add(Financial.FinancialyearID);
                    Year.Add(Financial.FinancialyearID + 1);
                    Year.Add(Financial.FinancialyearID - 1);
                    Year.Add(Financial.FinancialyearID - 2);
                    if (Financial.AllocationLevel == null || Financial.AllocationLevel == "")
                    {
                        FinacialYear = (from dat in db.FinancialYears
                                        where Year.Contains(dat.FinancialYearID) //&& dat.IsActive == true
                                        select new GetFinancial
                                        {
                                            FinancialyearID = dat.FinancialYearID,
                                            Financialyear = dat.ShortName,
                                            IsActive = dat.IsActive
                                        }).OrderBy(x => x.FinancialyearID).ToList();
                    }

                    else
                    {
                        FinacialYear = (from dat in db.FinancialYears
                                        where Year.Contains(dat.FinancialYearID) //&& dat.IsActive == true
                                        select new GetFinancial
                                        {
                                            FinancialyearID = dat.FinancialYearID,
                                            Financialyear = dat.ShortName,
                                            IsActive = dat.IsActive,
                                            Version = (from ver in db.MDFPlannings
                                                       where ver.FinancialYearID == dat.FinancialYearID && ver.CountryOrGeoOrDistrict == Financial.CountryOrGeoOrDistrict
                                                       && ver.AllocationLevel == Financial.AllocationLevel
                                                       && ver.PartnerTypeID == Financial.PartnerTypeID
                                                        
                                                       select new Versions
                                                       {
                                                           VersionID = ver.ID,
                                                           VersionNo = ver.VersionNo,
                                                           VersionName = ver.VersionName,
                                                           IsFinal = ver.IsFinal,
                                                           MembershipGroupID = ver.MembershipGroupID
                                                       }).OrderBy(x => x.VersionID)
                                        }).OrderBy(x => x.FinancialyearID).ToList();
                    }

                    return FinacialYear;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                throw new MPOWRException(message, ex);
            }
        }
    }
}