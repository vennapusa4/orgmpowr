using MPOWR.Dal.Models;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.Http;

namespace MPOWR.Api.Controllers
{
    public class LastUpdatedController : ApiController
    {
        private MPOWREntities db = new MPOWREntities();
        [Route("api/LastUpdated/GetLastUpdated")]
        public IHttpActionResult GetLastUpdated(string userid,int partnerid)
        {
            try
            {
                var Result = string.Empty;
                DbCommand command = db.Database.Connection.CreateCommand();
                command.CommandText = "Sp_GetLastRefreshDate";
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@partnerid", partnerid));
                command.CommandType = System.Data.CommandType.StoredProcedure;
                db.Database.Connection.Open();
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                                Result += reader.GetValue(i).ToString();


                    }
                }
                db.Database.Connection.Close();

                return Ok(Result);
            }
            catch (Exception e)
            {

                throw;
            }

        }
        [Route("api/LastUpdated/inseretLastUpdated")]
        public IHttpActionResult inseretLastUpdated(string userid, int partnerid)
        {
            try
            {
                DbCommand command = db.Database.Connection.CreateCommand();
                command.CommandText = "Sp_InsertLastRefreshDate";
                command.Parameters.Add(new SqlParameter("@date", DateTime.Now));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@partnerid", partnerid));
                command.CommandType = System.Data.CommandType.StoredProcedure;
                db.Database.Connection.Open();
                command.ExecuteNonQuery();
                db.Database.Connection.Close();
                return Ok();

            }
            catch (Exception e)
            {

                throw;
            }

        }
    }
}
