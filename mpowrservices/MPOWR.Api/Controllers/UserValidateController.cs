using MPOWR.Api.Service;
using MPOWR.Api.ViewModel;
using MPOWR.Dal.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MPOWR.Api.Controllers
{
    public class UserValidateController : ApiController
    {
        private MPOWREntities db = new MPOWREntities();
        LoginServiceController loginservice = new LoginServiceController();
        [HttpPost]
        [Route("api/ValidateUser/GetToken")]
        public IHttpActionResult GetToken(ValidateUserModel user)
        {
            try
            {
                var Result = db.Users.Where(e => e.EmailID.ToUpper() == user.UserID.ToUpper()).ToList();
                if (Result.Count != 0)
                {
                    Token token = new Token()
                    {
                        UserID = user.UserID,
                        TokenID = user.TokenID
                    };
                    foreach (var dt in Result)
                    {
                        user.Data = loginservice.InsertLoginTime(token);
                    }

                }

            }
            catch (Exception e)
            {

                throw;
            }
            return Ok(user);
        }
        [HttpPost]
        [Route("api/ValidateUser/GetUserList")]
        public IHttpActionResult GetUserList(ValidateUserModel user)
        {
            try
            {
                var Result = db.LoginAuthentications.Where(e => e.TokenID == user.TokenID && e.Status == true).ToList();
                if (Result.Count != 0)
                {
                    var users = db.Users.Where(e => e.IsActive == true && (!e.EmailID.Contains("brillio.com"))).Select(u => u.EmailID.Trim()).ToList();
                    return Ok(users);
                }
                else
                {
                    return Content(HttpStatusCode.Unauthorized, user);

                }

            }
            catch (Exception e)
            {

                throw;
            }

        }
        [HttpPost]
        [Route("api/ValidateUser/UpdateUserList")]
        public IHttpActionResult UpdateUserList(ValidateUserModel model)
        {
            try
            {
                using (MPOWREntities db = new MPOWREntities())
                {
                    string list = "";
                    if (model.UserList.Count > 0)
                    {
                        list = string.Join(",", model.UserList.Select(i => i.Replace(";", "")));
                    }

                    DbCommand command = db.Database.Connection.CreateCommand();
                    command.CommandText = "usp_UpdateMpowrUserList";
                    command.Parameters.Add(new SqlParameter("@UserID", model.UserID));
                    command.Parameters.Add(new SqlParameter("@tokenID", model.TokenID));
                    command.Parameters.Add(new SqlParameter("@UpdateUserList", list));
                    command.Parameters.Add(new SqlParameter("@ApplicationID", model.ApplicationID));

                    command.CommandType = CommandType.StoredProcedure;

                    db.Database.Connection.Open();
                    command.ExecuteNonQuery();
                    db.Database.Connection.Close();
                    return Ok();
                }

            }
            catch (Exception e)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("api/ValidateUser/LogOff")]
        public IHttpActionResult LogOff(string TokenID)
        {
            try
            {
                using (MPOWREntities db = new MPOWREntities())
                {
                    var data = db.LoginAuthentications.Find(TokenID);

                    if (data != null)
                    {
                        data.LogOffTime = DateTime.Now;
                        data.Status = false;
                        db.SaveChanges();
                    }
                }
                return Ok();

            }
            catch (Exception e)
            {

                throw;
            }

        }
    }
}
