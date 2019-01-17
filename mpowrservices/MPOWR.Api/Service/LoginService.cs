using MPOWR.Dal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

using MPOWR.Api.Controllers;
using MPOWR.Core;

namespace MPOWR.Api.Service
{
    public class LoginServiceController :  ApiController
    {
        MPOWREntities db = new MPOWREntities();
       
        public string InsertLoginTime(Token token)
        {
            LoginAuthentication LA = new LoginAuthentication();
            try
            {
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                DateTime createdDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);

                LA.UserID = token.UserID;
                string uniqueid = Guid.NewGuid().ToString();
                LA.TokenID = Regex.Replace(uniqueid, @"[^0-9]", "");
                LA.LogInTime = createdDate;
                LA.Status = true;
                db.LoginAuthentications.Add(LA);
                db.SaveChanges();
            }

            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
            }

            return  LA.TokenID;
        }

        [Route("api/LoginService/UpdateLogOffTime")]
        public string UpdateLogOffTime(Token token)
        {
            token.TokenID = Request.Headers.GetValues("authenticationToken").FirstOrDefault();
            if (token.TokenID != null || token.TokenID != "")
            {
                try
                {
                    TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                    DateTime createdDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                    var Record = db.LoginAuthentications.Find(token.TokenID);
                    if (Record.Status == true && Record != null)
                    {
                        Record.LogOffTime = createdDate;
                        Record.Status = false;
                        db.Entry(Record).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    DateTime Date = DateTime.Parse("09/10/2017"); //MM-DD-YY
                    var yesterday = DateTime.Today.AddDays(-1);
                    var TokenList = db.LoginAuthentications.Where(x => x.UserID == Record.UserID && x.Status == true && x.LogInTime <= yesterday && x.LogInTime >= Date).ToList();
                    if (TokenList.Count > 0 && TokenList != null)
                    {
                        foreach (var item in TokenList)
                        {
                            item.LogOffTime = createdDate;
                            item.Status = false;
                            db.Entry(item).State = EntityState.Modified;
                            db.SaveChanges();
                        }
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


                return "OK";
            }
            else
            {
                return "Invalid Token";
            }
        }
    }

    public class Token
    {
        
        public string UserID { get; set; }
        public string TokenID { get; set; }

    }
}