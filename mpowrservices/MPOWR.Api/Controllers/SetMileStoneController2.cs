using MPOWR.Api.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;
using MPOWR.Dal.Models;

namespace MPOWR.Api.Controllers
{

    public class SetMileStone2Controller : ApiController
    {
        private MPOWREntities db = new MPOWREntities();
        [HttpGet]
        [Route("api/SetMileStone2/MileStoneDetails")]
        public IHttpActionResult MileStoneDetails(short FinancialYearId, short RegionId)
        {
            IEnumerable<MileStoneViewModel> MileStoneDetails;

            int MilestoneFYConfigID = db.MilestoneFYConfigs.Where(x => x.FinancialYearID == FinancialYearId && x.RegionID == RegionId).Select(x => x.MilestoneFYConfigID).SingleOrDefault();
            if (MilestoneFYConfigID > 0)
            {
                MileStoneDetails = (
                                    from details in db.MilestoneNotifications
                                    where details.MilestoneFYConfigID == MilestoneFYConfigID
                                    select new MileStoneViewModel
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
                                        Reminder2 = details.ReminderEmailMessage1,
                                        Period2 = details.SetReminderAlertDays2,
                                        Status2 = details.ReminderStatus2,
                                        Unit = "Days",
                                        SendTo = from MSNotification in db.MilestoneNotificationsRoles
                                                 join role in db.Roles on MSNotification.RoleID equals role.RoleID
                                                 where MSNotification.MilestoneNotificationID == details.MilestoneNotificationID && MSNotification.RoleOperation == "Send"
                                                 select new RoleVM
                                                 {
                                                     MilestoneNotificationRoleID = MSNotification.MilestoneNotificationRoleID,
                                                     RoleId = role.RoleID,
                                                     RoleName = role.DisplayName
                                                 },
                                        CopyTo = from MSNotification in db.MilestoneNotificationsRoles
                                                 join role in db.Roles on MSNotification.RoleID equals role.RoleID
                                                 where MSNotification.MilestoneNotificationID == details.MilestoneNotificationID && MSNotification.RoleOperation == "Copy"
                                                 select new RoleVM
                                                 {
                                                     MilestoneNotificationRoleID = MSNotification.MilestoneNotificationRoleID,
                                                     RoleId = role.RoleID,
                                                     RoleName = role.DisplayName
                                                 }
                                    }).AsEnumerable();
            }
            else
            {
                MileStoneDetails = (from details in db.MilestoneNotifications
                                    select new MileStoneViewModel
                                    {
                                        Id = (details.MilestoneOrderNo).Value,
                                        MilestoneFYConfigID = 0,
                                        MilestoneNotificationId = 0,
                                        FinancialYearId = FinancialYearId,
                                        RegionId = RegionId,
                                        UserId = "",
                                        Name = details.MilestoneName,
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
                                                 select new RoleVM
                                                 {
                                                     MilestoneNotificationRoleID = 0,
                                                     RoleId = role.RoleID,
                                                     RoleName = role.DisplayName
                                                 },
                                        CopyTo = from role in db.Roles
                                                 where role.RoleID == 0
                                                 select new RoleVM
                                                 {
                                                     MilestoneNotificationRoleID = 0,
                                                     RoleId = role.RoleID,
                                                     RoleName = role.DisplayName
                                                 }
                                    }).AsEnumerable();
            }

            return Ok(MileStoneDetails);
        }

        [HttpPost]
        [Route("api/SetMileStone2/SetMileStoneDetailss")]
        public IHttpActionResult SetMileStoneDetailss(List<SetMileStoneViewModel> inputdataa)
        {
            SetMileStoneViewModel inputdata = new SetMileStoneViewModel();
            foreach (var item in inputdataa)
            {

                inputdata = item;
                inputdata.allroles = new List<RoleVM>();
                foreach (var data in inputdata.CopyTo)
                {
                    data.primaryorsecondary = "Copy";
                    inputdata.allroles.Add(data);
                }
                foreach (var data in inputdata.SendTo)
                {
                    data.primaryorsecondary = "Send";
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
                    command.CommandText = "SP_insert_milestonedetails2";
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
                    command.CommandText = "Sp_insertMilestoneNotificationrole";
                    command.Parameters.Add(new SqlParameter("@id", id));
                    command.Parameters.Add(new SqlParameter("@primaryorsecondary", r.primaryorsecondary));
                    command.Parameters.Add(new SqlParameter("@roleids", r.RoleId));
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    // db.Database.Connection.Open();
                    command.ExecuteNonQuery();

                    //return Ok();
                }
                catch (Exception ex)
                {
                    db.Database.Connection.Close();
                    // return null;
                }

            }
        }
    }
}
