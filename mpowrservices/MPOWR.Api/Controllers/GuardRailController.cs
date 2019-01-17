using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MPOWR.Dal.Models;
using MPOWR.Api.ViewModel;
using System.Data.Entity;
using MPOWR.Core;
using MPOWR.Api.App_Start;

namespace MPOWR.Api.Controllers
{
    [AuthorizeUser]
    public class GuardRailController : ApiController
    {

        private MPOWREntities db = new MPOWREntities();
        private GuardRailsViewModel guardRail = new GuardRailsViewModel();


        [HttpGet]
        [Route("api/GuardRail/GetGuardRailDetail")]
        public List<GuardRailDetail> GetGuardRailDetail(int regionID, int financialYearID)
        {
            List<GuardRailDetail> Details=null;
            if (regionID == 0 || financialYearID == 0)
                return Details;

            try
            {               
                var rowCount = (from grc in db.GuardRailsConfigs.AsNoTracking()
                                join grfc in db.GuardRailFYConfigs.AsNoTracking() on grc.GuardRailFYConfigID equals grfc.GuardRailFYConfigID into group1
                             from g1 in group1.DefaultIfEmpty()
                             where g1.RegionID == regionID && g1.FinancialYearID == financialYearID
                             select grc.GuardRailFYConfigID
                             ).Count();
                if (rowCount > 0)
                {
                    Details = (from grc in db.GuardRailsConfigs.AsNoTracking()
                               join grfc in db.GuardRailFYConfigs.AsNoTracking() on grc.GuardRailFYConfigID equals grfc.GuardRailFYConfigID into group1
                            from g1 in group1.DefaultIfEmpty()
                            where g1.RegionID == regionID && g1.FinancialYearID == financialYearID
                            select new GuardRailDetail
                            {
                                GuardRailConfigID = grc.GuardRailConfigID,
                                GuardRailFYConfigID = grc.GuardRailFYConfigID,
                                ProgramCarveOutComplaintValue = grc.ProgramCarveOutComplaintValue ?? 0,
                                ProgramCarveOutNonComplaintValue = grc.ProgramCarveOutNonComplaintValue ?? 0,
                                ActualMDFAllocationComplaintValue = grc.ActualMDFAllocationComplaintValue ?? 0,
                                ActualMDFAllocationNonComplaintValue = grc.ActualMDFAllocationNonComplaintValue ?? 0,
                                MDFSelloutIndexComplaintValue = grc.MDFSelloutIndexComplaintValue ?? 0,
                                MDFSelloutIndexNonComplaintValue = grc.MDFSelloutIndexNonComplaintValue ?? 0,
                                OverAllocationComplaintValue = grc.OverAllocationComplaintValue ?? 0,
                                OverAllocationNonComplaintValue = grc.OverAllocationNonComplaintValue ?? 0,
                                AllowOverAllocation = grc.AllowOverAllocation,
                                UnderAllocationComplaintValue = grc.UnderAllocationComplaintValue ?? 0,
                                UnderAllocationNonComplaintValue = grc.UnderAllocationNonComplaintValue ?? 0,
                                ReviewerRoleID1 = grc.ReviewerRoleID1 ?? 0,
                                ReviewerRoleID2 = grc.ReviewerRoleID2 ?? 0,
                                ReviewerRoleID3 = grc.ReviewerRoleID3 ?? 0,
                                ReviewerRoleID4 = grc.ReviewerRoleID4 ?? 0,

                            }
                    ).ToList();
                }
                else {
                                        
                    Details = new List<GuardRailDetail>();

                    Details.Add(new GuardRailDetail
                    {
                        GuardRailConfigID = 0,
                        GuardRailFYConfigID = 0,
                        ProgramCarveOutComplaintValue = 0,
                        ProgramCarveOutNonComplaintValue = 0,
                        ActualMDFAllocationComplaintValue = 0,
                        ActualMDFAllocationNonComplaintValue = 0,
                        MDFSelloutIndexComplaintValue = 0,
                        MDFSelloutIndexNonComplaintValue = 0,
                        OverAllocationComplaintValue = 0,
                        OverAllocationNonComplaintValue = 0,
                        AllowOverAllocation = "Y",
                        UnderAllocationComplaintValue = 0,
                        UnderAllocationNonComplaintValue = 0,
                        ReviewerRoleID1 = 0,
                        ReviewerRoleID2 = 0,
                        ReviewerRoleID3 = 0,
                        ReviewerRoleID4 = 0,

                    });
                }
               

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
            return Details;

        }
        [HttpPost]
        [Route("api/GuardRail/AddUpdateGuardRail")]
        public IHttpActionResult AddUpdateGuardRail(GuardRailsViewModel guardRailDetail)
        {
            try
            {
                short regionID = 0;
                int financialYearID = 0;
                int guardRailFYConfigID=0;
                string user = string.Empty;
                regionID = guardRailDetail.RegionID;
                financialYearID = guardRailDetail.FinancialYearID;
                user = guardRailDetail.user;
                var record = db.GuardRailFYConfigs.SingleOrDefault(b => b.RegionID == regionID && b.FinancialYearID == financialYearID);
                if (record == null)
                {
                    int dd;                    
                    var cl = (from dt in db.GuardRailFYConfigs select dt).ToList();
                    if (cl.Count == 0)
                    { dd = 0; }
                    else
                    {
                        dd = db.GuardRailFYConfigs.Max(x => x.GuardRailFYConfigID);
                    }
                    guardRailFYConfigID = dd + 1;

                    GuardRailFYConfig guardRailFYConfig = new GuardRailFYConfig();
                    if(regionID == 0 || financialYearID==0)
                    {
                        return Ok("The financial year-" + financialYearID+"  or region- "+ regionID +" cannot be null or zero");
                    }                   

                    guardRailFYConfig.FinancialYearID = financialYearID;
                    guardRailFYConfig.RegionID = regionID;
                    guardRailFYConfig.CreatedBy = user;
                    guardRailFYConfig.CreatedDate = DateTime.Now;
                    db.GuardRailFYConfigs.Add(guardRailFYConfig);
                    db.SaveChanges();

                }
                else
                {
                    guardRailFYConfigID = record.GuardRailFYConfigID;
                }
                foreach (var guardRail in guardRailDetail.GuardRailDetails)
                {
                    GuardRailsConfig guardRailsConfig = new GuardRailsConfig();
                    if (guardRailFYConfigID != 0)
                    {
                        guardRail.GuardRailFYConfigID = guardRailFYConfigID;
                    }
                    var result = db.GuardRailsConfigs.SingleOrDefault(b => b.GuardRailFYConfigID == guardRail.GuardRailFYConfigID);
                    
                    if (result == null)
                    {                       
                        guardRailsConfig.GuardRailFYConfigID = guardRail.GuardRailFYConfigID;
                        guardRailsConfig.ProgramCarveOutComplaintValue = guardRail.ProgramCarveOutComplaintValue;
                        guardRailsConfig.ProgramCarveOutNonComplaintValue = guardRail.ProgramCarveOutNonComplaintValue;
                        guardRailsConfig.ActualMDFAllocationComplaintValue = guardRail.ActualMDFAllocationComplaintValue;
                        guardRailsConfig.ActualMDFAllocationNonComplaintValue = guardRail.ActualMDFAllocationNonComplaintValue;
                        guardRailsConfig.MDFSelloutIndexComplaintValue = guardRail.MDFSelloutIndexComplaintValue;
                        guardRailsConfig.MDFSelloutIndexNonComplaintValue = guardRail.MDFSelloutIndexNonComplaintValue;
                        guardRailsConfig.OverAllocationComplaintValue = guardRail.OverAllocationComplaintValue;
                        guardRailsConfig.OverAllocationNonComplaintValue = guardRail.OverAllocationNonComplaintValue;
                        guardRailsConfig.AllowOverAllocation = guardRail.AllowOverAllocation;
                        guardRailsConfig.UnderAllocationComplaintValue = guardRail.UnderAllocationComplaintValue;
                        guardRailsConfig.UnderAllocationNonComplaintValue = guardRail.UnderAllocationNonComplaintValue;
                        guardRailsConfig.ReviewerRoleID1 = guardRail.ReviewerRoleID1;
                        guardRailsConfig.ReviewerRoleID2 = guardRail.ReviewerRoleID2;
                        guardRailsConfig.ReviewerRoleID3 = guardRail.ReviewerRoleID3;
                        guardRailsConfig.ReviewerRoleID4 = guardRail.ReviewerRoleID4;                        
                        guardRailsConfig.CreatedBy = user;
                        guardRailsConfig.CreatedDate = DateTime.Now;
                        db.GuardRailsConfigs.Add(guardRailsConfig);
                        db.SaveChanges();
                    }
                    else
                    {
                        var guardRailConfigID = result.GuardRailConfigID;
                        var guardRailsConfigs = db.GuardRailsConfigs.Find(guardRailConfigID);
                        
                        guardRailsConfigs.GuardRailFYConfigID = guardRail.GuardRailFYConfigID;
                        guardRailsConfigs.ProgramCarveOutComplaintValue = guardRail.ProgramCarveOutComplaintValue;
                        guardRailsConfigs.ProgramCarveOutNonComplaintValue = guardRail.ProgramCarveOutNonComplaintValue;
                        guardRailsConfigs.ActualMDFAllocationComplaintValue = guardRail.ActualMDFAllocationComplaintValue;
                        guardRailsConfigs.ActualMDFAllocationNonComplaintValue = guardRail.ActualMDFAllocationNonComplaintValue;
                        guardRailsConfigs.MDFSelloutIndexComplaintValue = guardRail.MDFSelloutIndexComplaintValue;
                        guardRailsConfigs.MDFSelloutIndexNonComplaintValue = guardRail.MDFSelloutIndexNonComplaintValue;
                        guardRailsConfigs.OverAllocationComplaintValue = guardRail.OverAllocationComplaintValue;
                        guardRailsConfigs.OverAllocationNonComplaintValue = guardRail.OverAllocationNonComplaintValue;
                        guardRailsConfigs.AllowOverAllocation = guardRail.AllowOverAllocation;
                        guardRailsConfigs.UnderAllocationComplaintValue = guardRail.UnderAllocationComplaintValue;
                        guardRailsConfigs.UnderAllocationNonComplaintValue = guardRail.UnderAllocationNonComplaintValue;
                        guardRailsConfigs.ReviewerRoleID1 = guardRail.ReviewerRoleID1;
                        guardRailsConfigs.ReviewerRoleID2 = guardRail.ReviewerRoleID2;
                        guardRailsConfigs.ReviewerRoleID3 = guardRail.ReviewerRoleID3;
                        guardRailsConfigs.ReviewerRoleID4 = guardRail.ReviewerRoleID4;
                        guardRailsConfigs.ModifiedBy = user;
                        guardRailsConfigs.ModifiedDate = DateTime.Now;      
                        db.Entry(guardRailsConfigs).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }

              
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


            return Ok();
        }


       
    }
}