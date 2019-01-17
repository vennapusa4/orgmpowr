using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MPOWR.Dal.Models;
using MPOWR.Api.ViewModel;
using System.Data.Common;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using MPOWR.Core;
using MPOWR.Api.App_Start;

namespace MPOWR.Api.Controllers
{
    [AuthorizeUser]
    public class RoleFeatureMgntController : ApiController
    {
        private MPOWREntities db = new MPOWREntities();

        //RoleFeatureViewModel roleFeature = new RoleFeatureViewModel();

        [HttpGet]
        [Route("api/RoleFeatureMgnt/GetRoles")]
        public IHttpActionResult GetRoles()
        {
            try
            {
                var Result = string.Empty;

                DbCommand command = db.Database.Connection.CreateCommand();
                command.CommandText = "dbo.usp_GetRole";               
                command.CommandType = CommandType.StoredProcedure;

                db.Database.Connection.Open();
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (Result == string.Empty)
                            Result = reader.GetValue(0).ToString();
                        else
                            Result += reader.GetValue(0).ToString();
                    }
                }
                object json = JToken.Parse(Result);
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
            finally
            {
                db.Database.Connection.Close();
            }
        }
        [Route("api/RoleFeatureMgnt/GetRoleFeatureActionActivity")]
        public IHttpActionResult GetRoleFeatureActionActivity(int roleID)
        {
            try
            {
                var Result = string.Empty;
                
                DbCommand command = db.Database.Connection.CreateCommand();
                command.CommandText = "dbo.usp_GetRoleFeatureActionActivity";
                command.Parameters.Add(new SqlParameter("@RoleID", roleID));
                command.CommandType =CommandType.StoredProcedure;

                db.Database.Connection.Open();
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (Result == string.Empty)
                            Result = reader.GetValue(0).ToString();
                        else
                            Result += reader.GetValue(0).ToString();
                    }
                }
                object json = JToken.Parse(Result);
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
            finally
            {
                db.Database.Connection.Close();
            }
        }
        [Route("api/RoleFeatureMgnt/GetSearchedRole")]
        public IHttpActionResult GetSearchedRole(string roleShortName)
        {
                      
            try
            {
                var Result = string.Empty;

                DbCommand command = db.Database.Connection.CreateCommand();
                command.CommandText = "dbo.usp_GetRole";                
                command.Parameters.Add(new SqlParameter("@ShortName", roleShortName));
                command.CommandType = CommandType.StoredProcedure;

                db.Database.Connection.Open();
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (Result == string.Empty)
                            Result = reader.GetValue(0).ToString();
                        else
                            Result += reader.GetValue(0).ToString();
                    }
                }
                object json = JToken.Parse(Result);
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
            finally
            {
                db.Database.Connection.Close();
            }
        }

        [HttpPost]
        [Route("api/RoleFeatureMgnt/CreateNewRole")]
        public IHttpActionResult CreateNewRole([FromBody]RoleFeatureViewModelRequest request)
        {
            try
            {
                var Result = string.Empty;

                DbCommand command = db.Database.Connection.CreateCommand();
                command.CommandText = "usp_CreateRole";
                command.Parameters.Add(new SqlParameter("@ShortName", request.ShortName));
                command.Parameters.Add(new SqlParameter("@DisplayName", request.RoleName));
                command.Parameters.Add(new SqlParameter("@User", request.UserID));
                command.CommandType = CommandType.StoredProcedure;

                db.Database.Connection.Open();
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (Result == string.Empty)
                            Result = reader.GetValue(0).ToString();                       
                    }
                }
                if (Result == string.Empty)
                    return Ok("Inserted Successfully");
                else
                    return Ok(Result);
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
            
        }
        [HttpPost]
        [Route("api/RoleFeatureMgnt/AddUpdateRoleFeature")]
        public IHttpActionResult AddUpdateRoleFeature([FromBody]RoleFeatureActivityRequest RoleFeatureActivities)
        {
            try
            {
                string user = string.Empty;
                int roleID = 0;
                foreach (var roleUser in RoleFeatureActivities.RoleUser)
                {
                    user = roleUser.UserID;
                    roleID = roleUser.RoleID;
                }
                    foreach (var rolePartnerTypes in RoleFeatureActivities.RolePartnerType)
                {
                    
                    RolePartnerType rolePartnerType = new RolePartnerType();
                    var result = db.RolePartnerTypes.SingleOrDefault(b => b.RoleID == roleID && b.PartnerTypeID == rolePartnerTypes.PartnerTypeID);

                   
                    if (result == null)
                    {
                        rolePartnerType.RoleID = roleID;
                        rolePartnerType.PartnerTypeID = rolePartnerTypes.PartnerTypeID;
                        rolePartnerType.IsChecked = int.Parse((rolePartnerTypes.PartnerTypeIsChecked == false ? "0" : "1")) == 1;
                        rolePartnerType.CreatedBy = user;
                        rolePartnerType.CreatedDate = DateTime.Now;
                        rolePartnerType.ModifiedBy = user;
                        rolePartnerType.ModifiedDate = DateTime.Now;
                        db.RolePartnerTypes.Add(rolePartnerType);
                        db.SaveChanges();
                    }
                    else
                    {
                        var rolePartnerTypeID = result.RolePartnerTypeID;
                        var rolePartnerTypeUpdate = db.RolePartnerTypes.Find(rolePartnerTypeID);
                        rolePartnerTypeUpdate.RoleID = roleID;
                        rolePartnerTypeUpdate.PartnerTypeID = rolePartnerTypes.PartnerTypeID;
                        rolePartnerTypeUpdate.IsChecked = int.Parse((rolePartnerTypes.PartnerTypeIsChecked == false ? "0" : "1")) == 1;
                        rolePartnerTypeUpdate.ModifiedBy = user;
                        rolePartnerTypeUpdate.ModifiedDate = DateTime.Now;                        
                        db.Entry(rolePartnerTypeUpdate).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                  
                }

                foreach (var RoleFeature in RoleFeatureActivities.RoleFeatureActivity)
                {
                    RoleFeatureActivity roleFeatureActivity = new RoleFeatureActivity();
                    var result = db.RoleFeatureActivities.SingleOrDefault(b => b.RoleID == roleID && b.FeatureActionID == RoleFeature.FeatureActionID);
                    if (result==null)
                    {
                        roleFeatureActivity.RoleID = roleID;
                        roleFeatureActivity.FeatureActionID = RoleFeature.FeatureActionID;
                        roleFeatureActivity.IsChecked = int.Parse((RoleFeature.FeatureActionIsChecked == false ? "0" : "1")) == 1;
                        roleFeatureActivity.CreatedBy = user;
                        roleFeatureActivity.CreatedDate = DateTime.Now;
                        roleFeatureActivity.ModifiedBy = user;
                        roleFeatureActivity.ModifiedDate = DateTime.Now;
                        db.RoleFeatureActivities.Add(roleFeatureActivity);
                        db.SaveChanges();
                    }
                    else
                    {
                        var roleFeatureActivityID = result.RoleFeatureActivityID;
                        var roleFeatureActivityUpdate = db.RoleFeatureActivities.Find(roleFeatureActivityID);

                        roleFeatureActivityUpdate.RoleID = roleID;
                        roleFeatureActivityUpdate.FeatureActionID = RoleFeature.FeatureActionID;
                        roleFeatureActivityUpdate.IsChecked = int.Parse((RoleFeature.FeatureActionIsChecked == false ? "0" : "1")) == 1;
                        roleFeatureActivityUpdate.ModifiedBy = user;
                        roleFeatureActivityUpdate.ModifiedDate = DateTime.Now;
                        db.Entry(roleFeatureActivityUpdate).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                

            }
            catch (MPOWRException ex)
            {   MPOWRLogManager.LogMessage(ex.Message.ToString());
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
