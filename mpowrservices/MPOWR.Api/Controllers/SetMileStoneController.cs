using MPOWR.Dal.Models;
using MPOWR.Api.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Configuration;
using MPOWR.Core;
using MPOWR.Api.App_Start;

namespace MPOWR.Api.Controllers
{
    [AuthorizeUser]
    public class SetMileStoneController : ApiController
    {
        private MPOWREntities db = new MPOWREntities();
        RegionDetails Rdetails = new RegionDetails();
        FinancialYearDetails Fdetails = new FinancialYearDetails();
        RoleViewModel role = new RoleViewModel();
        [HttpGet]
        [Route("api/SetMileStone/GetRegionsandYears")]
        public dynamic GetRegionsandYears()
        {
            try
            {
                var Result = string.Empty;

                DbCommand command = db.Database.Connection.CreateCommand();
                command.CommandText = "usp_GetRegionDetails";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                db.Database.Connection.Open();
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (Result == string.Empty)
                                Result = reader.GetValue(i).ToString();
                            else
                                Result += reader.GetValue(i).ToString();
                        }


                    }
                }
                db.Database.Connection.Close();
                object json = JToken.Parse(Result);
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
        [HttpGet]
        [Route("api/SetMileStone/GetRoles")]
        public RoleViewModel GetRoles()
        {
           
            try
            {

                role.Roles = (from roles in db.Roles
                                     select new
                                     {
                                         RoleID = roles.RoleID,
                                         RoleName = roles.DisplayName.Trim()
                                     }).AsQueryable();
                return role;
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
        [Route("api/SetMileStone/MileStoneDetails")]
        public IHttpActionResult MileStoneDetails(short FinancialYearId, short RegionId)
        {
            try
            {
                IEnumerable<MileStoneViewModelGet> MileStoneDetails;
                MileStoneViewModelGet MileStoneDetailsDefault;


                int MilestoneFYConfigID = db.MilestoneFYConfigs.Where(x => x.FinancialYearID == FinancialYearId && x.RegionID == RegionId).Select(x => x.MilestoneFYConfigID).SingleOrDefault();
                if (MilestoneFYConfigID > 0)
                {
                    MileStoneDetails = (
                                        from details in db.MilestoneNotifications
                                        where details.MilestoneFYConfigID == MilestoneFYConfigID
                                        select new MileStoneViewModelGet
                                        {
                                            Id = (details.MilestoneOrderNo).Value,
                                            MilestoneFYConfigID = MilestoneFYConfigID,
                                            MilestoneNotificationId = details.MilestoneNotificationID,
                                            FinancialYearId = FinancialYearId,
                                            RegionId = RegionId,
                                            UserId = details.CreatedBy,
                                            Name = details.MilestoneName,
                                            MilestoneDate = details.MilestoneDate,
                                            Reminder1 = details.ReminderEmailMessage1,
                                            Period1 = details.SetReminderAlertDays1,
                                            Status1 = details.ReminderStatus1,
                                            Reminder2 = details.ReminderEmailMessage2,
                                            Period2 = details.SetReminderAlertDays2,
                                            Status2 = details.ReminderStatus2,
                                            Unit = "Days",
                                            SendTo = from MSNotification in db.MilestoneNotificationsRoles
                                                     join role in db.Roles on MSNotification.RoleID equals role.RoleID
                                                     where MSNotification.MilestoneNotificationID == details.MilestoneNotificationID && MSNotification.RoleOperation == "Send To"
                                                     select new RoleSetMilestone
                                                     {

                                                         RoleID = role.RoleID

                                                     },
                                            CopyTo = from MSNotification in db.MilestoneNotificationsRoles
                                                     join role in db.Roles on MSNotification.RoleID equals role.RoleID
                                                     where MSNotification.MilestoneNotificationID == details.MilestoneNotificationID && MSNotification.RoleOperation == "Copy To"
                                                     select new RoleSetMilestone
                                                     {

                                                         RoleID = role.RoleID,

                                                     }
                                        }).AsEnumerable();
                    return Ok(MileStoneDetails);
                }
                else
                {
                    List<MileStoneViewModelGet> MileStoneDetailsdefault = new List<MileStoneViewModelGet>();
                    MileStoneDetailsDefault = (
                                        new MileStoneViewModelGet
                                        {
                                            Id = 1,
                                            MilestoneFYConfigID = 0,
                                            MilestoneNotificationId = 0,
                                            FinancialYearId = FinancialYearId,
                                            RegionId = RegionId,
                                            UserId = "",
                                            Name = "Add MileStone",
                                            MilestoneDate = DateTime.Now,
                                            Reminder1 = "",
                                            Period1 = 0,
                                            Status1 = null,
                                            Reminder2 = "",
                                            Period2 = 0,
                                            Status2 = null,
                                            Unit = "Days",
                                            SendTo = from role in db.Roles
                                                     where role.RoleID == 0
                                                     select new RoleSetMilestone
                                                     {

                                                         RoleID = role.RoleID,

                                                     },
                                            CopyTo = from role in db.Roles
                                                     where role.RoleID == 0
                                                     select new RoleSetMilestone
                                                     {

                                                         RoleID = role.RoleID,

                                                     }
                                        });
                    MileStoneDetailsdefault.Add(MileStoneDetailsDefault);
                    return Ok(MileStoneDetailsdefault);

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
        }


        // Insert & Update MileStoneDetails
        [HttpPost]
        [Route("api/SetMileStone/SetMileStoneDetails")]
        public IHttpActionResult SetMileStoneDetailss(List<SetMileStoneViewModel> inputdataa)
        {
            try
            {

                SetMileStoneViewModel inputdata = new SetMileStoneViewModel();
                foreach (var item in inputdataa)
                {

                    inputdata = item;
                    inputdata.allroles = new List<RoleVM>();
                    foreach (var data in inputdata.CopyTo)
                    {
                        data.primaryorsecondary = "Copy To";
                        inputdata.allroles.Add(data);
                    }
                    foreach (var data in inputdata.SendTo)
                    {
                        data.primaryorsecondary = "Send To";
                        inputdata.allroles.Add(data);
                    }
                    SetMileStoneViewModel milestoneinsertdata = new SetMileStoneViewModel();



                    var MilestoneFYConfigID = (from dat in db.MilestoneFYConfigs
                                               where dat.FinancialYearID == inputdata.FinancialYearId
                                               && dat.RegionID == inputdata.RegionId
                                               select new
                                               {
                                                   MilestoneFYConfigID = dat.MilestoneFYConfigID
                                               }).ToList();

                    foreach (var DATA in MilestoneFYConfigID)
                    {
                        if (DATA.MilestoneFYConfigID != 0)
                        {
                            milestoneinsertdata.MilestoneFYConfigID = DATA.MilestoneFYConfigID;
                        }

                    }

                    try
                    {
                        var Result = string.Empty;
                        DbCommand command = db.Database.Connection.CreateCommand();
                        command.CommandText = "usp_insert_milestonedetails2";
                        command.Parameters.Add(new SqlParameter("@FinancialYearId", inputdata.FinancialYearId));
                        command.Parameters.Add(new SqlParameter("@RegionId", inputdata.RegionId));
                        command.Parameters.Add(new SqlParameter("@MilestoneID", inputdata.Id));
                        command.Parameters.Add(new SqlParameter("@MilestoneName", inputdata.Name));
                        command.Parameters.Add(new SqlParameter("@MilestoneFYConfigID", milestoneinsertdata.MilestoneFYConfigID));
                        command.Parameters.Add(new SqlParameter("@Notification1Status", inputdata.Status1));
                        command.Parameters.Add(new SqlParameter("@Notification2Status", inputdata.Status2));
                        command.Parameters.Add(new SqlParameter("@Notification1", inputdata.Reminder1));
                        command.Parameters.Add(new SqlParameter("@Notification2", inputdata.Reminder2));
                        command.Parameters.Add(new SqlParameter("@NotificationAlert1", inputdata.Period1));
                        command.Parameters.Add(new SqlParameter("@NotificationAlert2", inputdata.Period2));
                        command.Parameters.Add(new SqlParameter("@SubmissionDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("@output", SqlDbType.Char, 500));
                        command.Parameters["@output"].Direction = ParameterDirection.Output;
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        db.Database.Connection.Open();
                        command.ExecuteNonQuery();
                        SqlParameter output = new SqlParameter("@output", SqlDbType.Int);
                        output.Direction = ParameterDirection.Output;
                        command.Parameters.Add(output);
                        //command.ExecuteNonQuery();
                        var outputdata = command.Parameters["@output"];
                        int value = Int32.Parse(outputdata.Value.ToString());
                        InsertRoles(inputdata.allroles, value);
                        // dynamic refid = command.Parameters["@output"];

                        db.Database.Connection.Close();

                    }
                    catch (Exception ex)
                    {
                        db.Database.Connection.Close();
                        return null;
                    }


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
        [Route("api/SetMileStone/PostEmail")]
        public IHttpActionResult PostEmail(emaildata data)

        {

            try
            {
                MailMessage mail = new MailMessage();
                mail.IsBodyHtml = true;
                mail.Subject = data.subject;
                mail.Body = data.body;
                mail.Bcc.Add(new MailAddress(ConfigurationManager.AppSettings["From"]));
                mail.From = new MailAddress(ConfigurationManager.AppSettings["From"]);
                mail.To.Add(data.to);               
                System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient();
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
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

        //to create new role
        [HttpPost]
        [Route("api/SetMileStone/CreateMileStoneDetails")]
        public IHttpActionResult CreateMileStoneDetails(SetMileStoneViewModel inputdata)
        {
            try
            {
                SetMileStoneViewModel milestoneinsertdata = new SetMileStoneViewModel();



                var MilestoneFYConfigID = (from dat in db.MilestoneFYConfigs
                                           where dat.FinancialYearID == inputdata.FinancialYearId
                                           && dat.RegionID == inputdata.RegionId
                                           select new
                                           {
                                               MilestoneFYConfigID = dat.MilestoneFYConfigID
                                           }).ToList();

                foreach (var DATA in MilestoneFYConfigID)
                {
                    if (DATA.MilestoneFYConfigID != 0)
                    {
                        milestoneinsertdata.MilestoneFYConfigID = DATA.MilestoneFYConfigID;
                    }

                }

                

                var Result = string.Empty;
                DbCommand command = db.Database.Connection.CreateCommand();
                command.CommandText = "usp_insert_milestonename";
                command.Parameters.Add(new SqlParameter("@FinancialYearId", inputdata.FinancialYearId));
                command.Parameters.Add(new SqlParameter("@RegionId", inputdata.RegionId));
                command.Parameters.Add(new SqlParameter("@MilestoneID", inputdata.Id));
                command.Parameters.Add(new SqlParameter("@MilestoneName", inputdata.Name));
                command.Parameters.Add(new SqlParameter("@MilestoneFYConfigID", milestoneinsertdata.MilestoneFYConfigID));
                command.Parameters.Add(new SqlParameter("@Notification1Status", DBNull.Value));
                command.Parameters.Add(new SqlParameter("@Notification2Status", DBNull.Value));
                command.Parameters.Add(new SqlParameter("@Notification1", DBNull.Value));
                command.Parameters.Add(new SqlParameter("@Notification2", DBNull.Value));
                command.Parameters.Add(new SqlParameter("@NotificationAlert1", DBNull.Value));
                command.Parameters.Add(new SqlParameter("@NotificationAlert2", DBNull.Value));
                command.Parameters.Add(new SqlParameter("@SubmissionDate", DBNull.Value));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                db.Database.Connection.Open();
                command.ExecuteNonQuery();
                db.Database.Connection.Close();

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
            return Ok();
        }

        //to insert the milestone details and update
        [HttpPut]
        [Route("api/SetMileStone/SetMileStoneDetails")]
        public IHttpActionResult SetMileStoneDetails(List<SetMileStoneViewModel> inputdataa)
        {
            SetMileStoneViewModel inputdata = new SetMileStoneViewModel();
            foreach (var item in inputdataa)
            {

                inputdata = item;
                inputdata.allroles = new List<RoleVM>();
                foreach (var data in inputdata.CopyTo)
                {
                    data.primaryorsecondary = "Copy To";
                    inputdata.allroles.Add(data);
                }
                foreach (var data in inputdata.SendTo)
                {
                    data.primaryorsecondary = "Send To";
                    inputdata.allroles.Add(data);
                }
                SetMileStoneViewModel milestoneinsertdata = new SetMileStoneViewModel();



                var MilestoneFYConfigID = (from dat in db.MilestoneFYConfigs
                                           where dat.FinancialYearID == inputdata.FinancialYearId
                                           && dat.RegionID == inputdata.RegionId
                                           select new
                                           {
                                               MilestoneFYConfigID = dat.MilestoneFYConfigID
                                           }).ToList();

                foreach (var DATA in MilestoneFYConfigID)
                {
                    if (DATA.MilestoneFYConfigID != 0)
                    {
                        milestoneinsertdata.MilestoneFYConfigID = DATA.MilestoneFYConfigID;
                    }

                }

                try
                {
                    var Result = string.Empty;
                    DbCommand command = db.Database.Connection.CreateCommand();
                    command.CommandText = "usp_insert_milestonedetails2";
                    command.Parameters.Add(new SqlParameter("@FinancialYearId", inputdata.FinancialYearId));
                    command.Parameters.Add(new SqlParameter("@RegionId", inputdata.RegionId));
                    command.Parameters.Add(new SqlParameter("@MilestoneID", inputdata.Id));
                    command.Parameters.Add(new SqlParameter("@MilestoneName", inputdata.Name));
                    command.Parameters.Add(new SqlParameter("@MilestoneFYConfigID", milestoneinsertdata.MilestoneFYConfigID));
                    command.Parameters.Add(new SqlParameter("@Notification1Status", inputdata.Status1));
                    command.Parameters.Add(new SqlParameter("@Notification2Status", inputdata.Status2));
                    if (inputdata.Reminder1 == null)
                    {
                        command.Parameters.Add(new SqlParameter("@Notification1", DBNull.Value));
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@Notification1", inputdata.Reminder1));
                    }
                    if (inputdata.Reminder2==null)
                    {
                        command.Parameters.Add(new SqlParameter("@Notification2", DBNull.Value));
                    }
                    else
                    { 
                    command.Parameters.Add(new SqlParameter("@Notification2", inputdata.Reminder2));
                    }
                    command.Parameters.Add(new SqlParameter("@NotificationAlert1", inputdata.Period1));
                    command.Parameters.Add(new SqlParameter("@NotificationAlert2", inputdata.Period2));
                    command.Parameters.Add(new SqlParameter("@SubmissionDate", inputdata.MilestoneDate));
                    command.Parameters.Add(new SqlParameter("@output", SqlDbType.Char, 500));
                    command.Parameters["@output"].Direction = ParameterDirection.Output;
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    db.Database.Connection.Open();
                    command.ExecuteNonQuery();
                    SqlParameter output = new SqlParameter("@output", SqlDbType.Int);
                    output.Direction = ParameterDirection.Output;
                    command.Parameters.Add(output);
                    //command.ExecuteNonQuery();
                    var outputdata = command.Parameters["@output"];
                    int value = Int32.Parse(outputdata.Value.ToString());
                    InsertRoles(inputdata.allroles, value);
                    // dynamic refid = command.Parameters["@output"];

                    db.Database.Connection.Close();

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
            return Ok();
        }
        //to insert into milestonenotificationrole
        public void InsertRoles(List<RoleVM> R, int id)
        {

            DbCommand cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "deleterolesfrommilestone";
            cmd.Parameters.Add(new SqlParameter("@milestonenotificationid", id));

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            // db.Database.Connection.Open();
            cmd.ExecuteNonQuery();
            RoleVM r = new RoleVM();
            foreach (var ROLES in R)
            {
               
                r = ROLES;
                try
                {
                    var Result = string.Empty;
                    DbCommand command = db.Database.Connection.CreateCommand();
                    command.CommandText = "usp_insertMilestoneNotificationrole";
                    command.Parameters.Add(new SqlParameter("@id", id));
                    command.Parameters.Add(new SqlParameter("@primaryorsecondary", r.primaryorsecondary));
                    command.Parameters.Add(new SqlParameter("@roleids", r.RoleId));
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                   // db.Database.Connection.Open();
                    command.ExecuteNonQuery();
                   
                    //return Ok();
                }
                catch (MPOWRException ex)
                {
                    db.Database.Connection.Close();
                    MPOWRLogManager.LogMessage(ex.Message.ToString());
                    MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                    

                }
                catch (Exception ex)
                {
                    db.Database.Connection.Close();
                    MPOWRLogManager.LogMessage(ex.Message.ToString());
                    MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                   
                }

            }
        }

    }
}
