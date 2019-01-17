using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MPOWR.Dal.Models;
using MPOWR.Api.ViewModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Data.Common;
using Newtonsoft.Json.Linq;
using System.Data.Entity.Infrastructure;
using MPOWR.Api.App_Start;
using MPOWR.Core;

namespace MPOWR.Api.Controllers
{
    [AuthorizeUser]
    public class PBRoundtwoController : ApiController
    {
        MPOWREntities objDBEntity = new MPOWREntities();
        private MPOWREntities db = new MPOWREntities();

        public IQueryable GetRoundtwoDetails(int VersionID)
        {
            try
            {
                var Partners = (from OutputResult in objDBEntity.ModelOutputTables
                                select new Partners
                                {
                                     PartnerID = OutputResult.PartnerID,
                                     ParnterName = OutputResult.Partner_Name,
                                    ModelParameterID = OutputResult.ModelParameterID,
                                    BusinessUnits = ( from outModel in objDBEntity.ModelOutputTables
                                                      join partBudget in objDBEntity.PartnerBudgets on outModel.PartnerID equals partBudget.PartnerID
                                                      join partBUBudget in objDBEntity.PartnerBUBudgets on partBudget.PartnerBudgetID equals partBUBudget.PartnerBudgetID
                                                      where outModel.PartnerID == OutputResult.PartnerID && outModel.BusinessUnitID == OutputResult.BusinessUnitID && outModel.BusinessUnitID == partBUBudget.BusinessUnitID && outModel.VersionID == VersionID
                                                      select new BusinessUnits
                                                      {
                                                           Name = outModel.Business_Unit,
                                                            MDF = partBUBudget.Baseline_MDF,
                                                             AdditionalMDF = outModel.Additional_Recommended_MDF
                                                      }
                                    )
                                });

                var CountryPartners = (from CntryPart in objDBEntity.ModelParameterTables
                                       where CntryPart.VersionID == VersionID  
                                       select CntryPart
                                    );

                var FinalOutput = (from part in Partners
                                   join cntry in CountryPartners on part.ModelParameterID equals cntry.ModelParameterID
                                   select part
                               ).AsQueryable();

                return FinalOutput;
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

        public IHttpActionResult UpdateRoundtwoDetails(UpdateRound2 objUpdateRound2)
        {
            try
            {
                PartnerBUBudget objPartBU = new PartnerBUBudget();
                objPartBU.PartnerBUBudgetID = objUpdateRound2.PartnerBUBudgetID;
                objPartBU.PartnerBudgetID = objUpdateRound2.PartnerBudgetID;
                objPartBU.BusinessUnitID = objUpdateRound2.BusinessUnitID;
                objPartBU.MDFVarianceReasonID = objUpdateRound2.MDFVarianceReasonID;
                objPartBU.FocusedAreaID = objUpdateRound2.FocusedAreaID;

                var IsPartBudget = objDBEntity.PartnerBUBudgets.Find(objPartBU.PartnerBUBudgetID);
                if (IsPartBudget != null)
                {
                    IsPartBudget.ModifiedBy = objUpdateRound2.UserID;
                    IsPartBudget.ModifiedDate = DateTime.Now;
                    IsPartBudget.Additional_MDF_Reason = objUpdateRound2.Additional_MDF_Reason;
                    IsPartBudget.Additional_MDF = objUpdateRound2.Additional_MDF;
                    objDBEntity.Entry(IsPartBudget).State = EntityState.Modified;
                }
                objDBEntity.SaveChanges();
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

        [Route("api/PBRoundtwo/ApplyFilter")]
        public IHttpActionResult ApplyFilter(Round2Filter request)
        {
            
            try
            {
               
                //var data = db.Database.ExecuteSqlCommand(" @C_ID,@PT_ID,@D_ID,@F_ID,@FocusedArea",
                //  new SqlParameter("@C_ID", request.CountryID),
                //  new SqlParameter("@PT_ID", request.PartnerTypeID),
                //  new SqlParameter("@D_ID", request.District_ID),
                //  new SqlParameter("@F_ID", request.FinancialYearID),
                //  new SqlParameter("@FocusedArea", request.FocusedAreaID)
                //  );

                DbCommand command = db.Database.Connection.CreateCommand();
               // db.Database.CommandTimeout = 10000000;
                command.CommandText = "usp_Round2MDFCalculation";
                command.Parameters.Add(new SqlParameter("@FocusedArea", request.FocusedAreaID));
                command.Parameters.Add(new SqlParameter("@V_ID", request.VersionID));
                command.Parameters.Add(new SqlParameter("@USER_ID", request.UserID));

                command.CommandType = System.Data.CommandType.StoredProcedure;

                db.Database.Connection.Open();
                command.ExecuteReader();


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
            return Ok("Update Successfully");
        }


    }
}
