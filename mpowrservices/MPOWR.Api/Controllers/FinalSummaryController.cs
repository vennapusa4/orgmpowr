using System;
using System.Web.Http;
using MPOWR.Api.ViewModel;
using System.Data.Common;
using MPOWR.Dal.Models;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using MPOWR.Api.App_Start;
using MPOWR.Core;

namespace MPOWR.Api.Controllers
{
    [AuthorizeUser]
    public class FinalSummaryController : ApiController
    {
        private MPOWREntities db = new MPOWREntities();

        [HttpPost]
        public IHttpActionResult GetFinalSummayData(FinalSummaryViewModelRequest request)
        {
            try
            {
                var Result = string.Empty;

                DbCommand command = db.Database.Connection.CreateCommand();
                command.CommandText = "usp_GetFinalSummary_Details";
                command.Parameters.Add(new SqlParameter("@PREVIOUS_PERIOD_YEAR", request.Previous_Period_Year));
                command.Parameters.Add(new SqlParameter("@Current_Financial_Period_Year_Id",request.Current_Financial_Period_Year_Id));
                command.Parameters.Add(new SqlParameter("@V_ID", request.VersionID));

                command.CommandType = System.Data.CommandType.StoredProcedure;

                db.Database.Connection.Open();
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                            Result += reader.GetValue(0).ToString();
                    }
                }
                db.Database.Connection.Close();
                return Ok(Result);
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
                db.Database.Connection.Close();
                
               
            }
            return null;
        }

        [Route("api/FinalSummary/FinalSummaryGraphData")]
        public IHttpActionResult FinalSummaryGraphData(Graph request)
        {
            try
            {
                
               var Result = string.Empty;
                 

                DbCommand command = db.Database.Connection.CreateCommand();
                command.CommandText = "usp_FinalSummaryGraph";
                command.Parameters.Add(new SqlParameter("@Previous_Period_Year", request.Previous_Period_Year));
                command.Parameters.Add(new SqlParameter("@BusinessUnitID", request.BusinessUnitID));
                command.Parameters.Add(new SqlParameter("@V_ID", request.VersionID));


                command.CommandType = System.Data.CommandType.StoredProcedure;

                db.Database.Connection.Open();
                var reader = command.ExecuteReader();
                
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                       Result = Result + reader.GetValue(0).ToString();
                        
                    }
                }
                object json = JObject.Parse(Result);
                db.Database.Connection.Close();

                return Ok(json);
            }
            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

            }
            catch (Exception ex)
            {
                db.Database.Connection.Close();
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
               
            }
            return null;
        }
    }
}