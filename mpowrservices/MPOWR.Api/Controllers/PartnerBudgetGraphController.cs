using MPOWR.Api.App_Start;
using MPOWR.Dal.Models;
using MPOWR.Api.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using MPOWR.Core;

namespace MPOWR.Api.Controllers
{
    [AuthorizeUser]
    public class PartnerBudgetGraphController : ApiController
    {
        private MPOWREntities db = new MPOWREntities();

        public IHttpActionResult PartnerBudgetGraphData(Graph request)
        {
            try
            {
                var Result = string.Empty;

                DbCommand command = db.Database.Connection.CreateCommand();
                command.CommandText = "usp_PartnerBudget_Graph";
                command.Parameters.Add(new SqlParameter("@BusinessUnitID", request.BusinessUnitID));
                command.Parameters.Add(new SqlParameter("@V_ID", request.VersionID));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                db.Database.Connection.Open();
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
                if (Result== null || Result == "")
                {
                    json = Result;
                }
                else { json = JObject.Parse(Result); }
                

                db.Database.Connection.Close();

                return Ok(json);
            }
            
            catch (MPOWRException ex)
            {
                db.Database.Connection.Close();
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

                return null;

            }
            catch (Exception ex)
            {
                db.Database.Connection.Close();
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }

    }
}