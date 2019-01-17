using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MPOWR.ViewModel;
using System.Security.Cryptography;
using System.Text;
using MPOWR.Dal.Models;
using System.Data.OleDb;
using MPOWR.Api.ViewModel;
using MPOWR.Api.Service;
using MPOWR.Api.App_Start;
using System.Configuration;
using System.IO;
using System.Data.Common;
using System.Data.SqlClient;
using MPOWR.Core;
using System.Net.Mail;
using ExcelDataReader;
using MPOWR.Bal;

namespace MPOWR.Api.Controllers
{
    public class UsersController : ApiController
    {
        private MPOWREntities db = new MPOWREntities();
        UsersViewModel uservm = new UsersViewModel();
        BUBdgetsViewModel Budgetvm = new BUBdgetsViewModel();
        LoginServiceController loginservice = new LoginServiceController();
        // GET: api/Users
        public IQueryable<User> GetUsers()
        {

            return db.Users;
        }
        [HttpGet]
        [Route("api/Users/CreateUsers")]
        public IHttpActionResult CreateUsers(UsersViewModel uservm)
        {
            try
            { 
            DataSet DtExcel = new DataSet();
            string fileName = ConfigurationSettings.AppSettings["ExcelConnection"];
            FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
            var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            //DtExcel = excelReader.AsDataSet();
            DtExcel = excelReader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });
            excelReader.Close();
            //oledbConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='D:\\tuserlist.xlsx';Extended Properties=Excel 8.0;");

            //oleCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Users$]", oledbConnection);
            //oleCommand.Fill(DtExcel);

            List<UserManagementModel> users = new List<UserManagementModel>();

            List<string> wrongusers = new List<string>();
            List<string> Upadtedusers = new List<string>();
            if (DtExcel != null && DtExcel.Tables.Count > 0)
            {
                var Geos = db.Geos.Where(x=>x.IsActive == true).Select(x => x.ShortName.ToUpper()).ToList();
                var countries = db.Countries.Where(x => x.IsActive == true).Select(x => x.ShortName.ToUpper()).ToList();
                var districts = db.Districts.Where(x => x.IsActive == true).Select(x => x.ShortName.ToUpper()).ToList();

                foreach (DataRow row in DtExcel.Tables[0].Rows)
                {
                    UserManagementModel user = new UserManagementModel();
                    var accessname = row["AccessLevel"].ToString();
                    user.EmailID = row["UserId"].ToString();
                    var existing_user = (from usertable in db.Users where (usertable.UserID == user.EmailID) select usertable).FirstOrDefault();
                    user.BUAccess = false;
                    user.IsAdmin = false;
                    MailAddress mail = new MailAddress(user.EmailID);
                    var username = mail.User.Split('.');
                    user.FirstName = username[0];
                    user.FirstName = user.FirstName.First().ToString().ToUpper() + user.FirstName.Substring(1);
                        user.FirstName = System.Text.RegularExpressions.Regex.Replace(user.FirstName, @"[^0-9a-zA-Z]+", "");
                        if (username.Count() > 1)
                    {
                        user.LastName = username[1];
                        user.LastName = user.LastName.First().ToString().ToUpper() + user.LastName.Substring(1);
                        user.LastName = System.Text.RegularExpressions.Regex.Replace(user.LastName, @"[^0-9a-zA-Z]+", "");
                        }
                    user.exists = false;
                    if (existing_user != null)
                    {
                        user.exists = true;
                        user.UserID = row["UserId"].ToString();
                        user.IsAdmin = (from ur in db.UserRoleUserTypes
                                        where ur.UserID == user.EmailID
                                        select ur.IsAdmin).FirstOrDefault() ?? false;
                        user.BUAccess = (from ur in db.UserRoleUserTypes
                                         where ur.UserID == user.EmailID
                                         select ur.EditNonBUModParam).FirstOrDefault();
                        user.FirstName = existing_user.FirstName;
                        user.LastName = existing_user.LastName;
                        user.UserRoleUserType = (from ur in db.UserRoleUserTypes
                                                 where ur.UserID == user.EmailID
                                                 select ur).FirstOrDefault();
                        Upadtedusers.Add(user.EmailID);
                    }

                    user.LoggedUser = "System Admin";

                    user.IsActive = row["IsActive"].ToString();

                    var userGeos = row["GEO"].ToString().ToUpper().Replace(", ", ",").Split(',').ToList();
                    var userCountries = row["Country"].ToString().ToUpper().Replace(", ", ",").Split(',').ToList();
                    var userDistricts = row["District"].ToString().ToUpper().Replace(", ", ",").Split(',').ToList();
                    var userBUS = row["BU"].ToString().ToUpper().Replace(", ", ",").Split(',').ToList();
                    user.districts = new List<UserDistrict>();
                    user.countries = new List<UserCountry>();
                    user.GeoDetails = new List<UserGeo>();
                    user.partnerType = new List<UserRTM>();
                    user.BusinessUnitDetails = new List<UserBusinessUnit>();
                    var userAccess = row["AccessLevel"].ToString();
                    var userRole = row["Role"].ToString();
                    user.userTypeDetails = new UserType();
                    user.userTypeDetails.UserTypeID = (from A in db.UserTypes
                                                       where A.DisplayName == userAccess
                                                       select A.UserTypeID).FirstOrDefault();
                    user.role = new Role();
                    user.role.RoleID = (from A in db.Roles
                                        where A.DisplayName == userRole
                                        select A.RoleID).FirstOrDefault();

                    var PartnerTypes = new List<int>();
                    if (user.exists == true)
                    {
                        PartnerTypes = db.UserRTMs.Where(x => x.UserRoleUserTypeID == user.UserRoleUserType.UserRoleUserTypeID).Select(x => x.PartnerTypeID).ToList();
                    }
                    else
                    {
                        PartnerTypes = db.PartnerTypes.Where(x => x.IsActive == true).Select(x => x.PartnerTypeID).ToList();

                    }
                    foreach (var item in PartnerTypes)
                    {
                        user.partnerType.Add(new UserRTM() { PartnerTypeID = item });
                    };
                    var userBuids = db.BusinessUnits.Where(x => userBUS.Contains(x.DisplayName) && x.IsActive == true).Select(x => x.BusinessUnitID).ToArray();
                    foreach (var item in userBuids)
                    {
                        user.BusinessUnitDetails.Add(new UserBusinessUnit() { BusinessUnitID = item });
                    };

                    if (accessname == "World Wide")
                    {
                            users.Add(user);

                    }
                    else if (accessname == "Geo")
                    {
                        if (userGeos.Intersect(Geos).Count() == userGeos.Count())
                        {

                            var userGeoIds = db.Geos.Where(x => userGeos.Contains(x.DisplayName) && x.IsActive == true).Select(x => x.GeoID).ToArray();
                            foreach (var item in userGeoIds)
                            {
                                user.GeoDetails.Add(new UserGeo() { GeoID = item });
                            };


                            users.Add(user);
                        }
                        else
                        {
                            wrongusers.Add(user.EmailID);
                            Upadtedusers.Remove(user.EmailID);
                        }
                    }
                    else if (accessname == "Country")
                    {
                        if (userGeos.Intersect(Geos).Count() == userGeos.Count() && userCountries.Intersect(countries).Count() == userCountries.Count())
                        {
                            var userGeoIds = db.Geos.Where(x => userGeos.Contains(x.DisplayName) && x.IsActive == true).Select(x => x.GeoID).ToArray();
                            foreach (var item in userGeoIds)
                            {
                                user.GeoDetails.Add(new UserGeo() { GeoID = item });
                            };
                            var userCountriesids = db.Countries.Where(x => userCountries.Contains(x.DisplayName) && x.IsActive == true).Select(x => x.CountryID).ToArray();
                            foreach (var item in userCountriesids)
                            {
                                user.countries.Add(new UserCountry() { CountryID = item });
                            };
                            users.Add(user);
                        }
                        else
                        {
                            wrongusers.Add(user.EmailID);
                            Upadtedusers.Remove(user.EmailID);
                        }
                    }
                    else if (accessname == "District")
                    {
                        if (userGeos.Intersect(Geos).Count() == userGeos.Count() && userCountries.Intersect(countries).Count() == userCountries.Count()
                            && userDistricts.Intersect(districts).Count() == userDistricts.Count())
                        {
                            var userGeoIds = db.Geos.Where(x => userGeos.Contains(x.DisplayName) && x.IsActive == true).Select(x => x.GeoID).ToArray();
                            foreach (var item in userGeoIds)
                            {
                                user.GeoDetails.Add(new UserGeo() { GeoID = item });
                            };
                            var userCountriesids = db.Countries.Where(x => userCountries.Contains(x.DisplayName) && x.IsActive == true).Select(x => x.CountryID).ToArray();
                            foreach (var item in userCountriesids)
                            {
                                user.countries.Add(new UserCountry() { CountryID = item });
                            };
                            var userdistrictids = db.Districts.Where(x => userDistricts.Contains(x.DisplayName) && x.IsActive == true).Select(x => x.DistrictID).ToArray();
                            foreach (var item in userdistrictids)
                            {
                                user.districts.Add(new UserDistrict() { DistrictID = item });
                            };

                            if (user.exists == false)
                            {
                                user.partnerType =new List<UserRTM>();
                                       PartnerTypes = db.PartnerTypes.Where(X => X.DisplayName == "Reseller" && X.IsActive == true).Select(x => x.PartnerTypeID).ToList();
                                foreach (var item in PartnerTypes)
                                {
                                    user.partnerType.Add(new UserRTM() { PartnerTypeID = item });
                                };
                            }


                            users.Add(user);


                        }

                        else
                        {
                            wrongusers.Add(user.EmailID);
                            Upadtedusers.Remove(user.EmailID);
                        }
                    }

                }
            }
            UserManagementController a = new UserManagementController();
            foreach (var item in users)
            {
             // a.UserManagement_Create_Update(item);
            }
        }
             catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }


            return Ok();
        }
        //Check User Authentication
        [Route("api/Users/checkUser")]
        public IHttpActionResult checkUser(UsersViewModel uservm)
        {
            System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(uservm.Pasword), 0, Encoding.UTF8.GetByteCount(uservm.Pasword));
            var Result = db.Users.Count(e => e.UserID.Trim().ToLower() == uservm.UserID.Trim().ToLower() && e.Password == crypto && e.IsActive == true);

            if (Result > 0)
            {
                Token token = new Token()
                {
                    UserID = uservm.UserID,
                    TokenID = uservm.TokenID
                };
                int FinancialYearID = db.Database.SqlQuery<int>("select dbo.GetPlanningFinancialPeriodId()").FirstOrDefault();
                var FY = db.FinancialYears.Find(FinancialYearID);
                dynamic ShowDataUploadAccess;
                CommonBL CommonBL = new CommonBL();
                ShowDataUploadAccess = CommonBL.GetAppConfigValue(MPOWRConstants.ShowDataUploadAccess);
                bool DataUploadAccess = ShowDataUploadAccess == "0" ? false : true;
                var Result1 = (from dat in db.Users
                               from app in db.AppConfig
                               where dat.UserID == uservm.UserID && app.ShortName == MPOWRConstants.ApplicationName
                               join UT in db.UserRoleUserTypes on dat.UserID equals UT.UserID
                               select new UsersVM
                               {
                                   FirstName = dat.FirstName,
                                   UserID = dat.UserID,
                                   IsAdmin = UT.IsAdmin,
                                   GlossaryApprover = UT.GlossaryApprover,
                                   UserTypeID = UT.UserTypeID,
                                   FinancialYearID = FinancialYearID,
                                   CurrentFinancialYearID = FinancialYearID,
                                   FinancialYear = FY.ShortName,
                                   RoleID = UT.RoleID,
                                   AppName = app.ShortName,
                                   ApplicationID = app.Params,
                                   DataUpload = UT.DataUpload,
                                   DataUploadAccess = DataUploadAccess,
                                   //Geo changes - 10th Jan, 2018
                                   Geos = (from geo in db.UserGeos where geo.UserRoleUserTypeID == UT.UserRoleUserTypeID
                                                            select geo.GeoID).ToList(),
                                   Countries = (from usercontry in db.UserCountries where usercontry.UserRoleUserTypeID == UT.UserRoleUserTypeID
                                                select usercontry.CountryID).ToList(),
                                   PartnerTypes = (from rolepartner in db.UserRTMs where rolepartner.UserRoleUserTypeID == UT.UserRoleUserTypeID
                                                   select rolepartner.PartnerTypeID).ToList(),
                                   Features = from activity in db.RoleFeatureActivities
                                              join action in db.FeatureActions on activity.FeatureActionID equals action.FeatureActionID
                                              join feature in db.Features on action.FeatureID equals feature.FeatureID
                                              join fat in db.FeatureActionTypes on action.FeatureActionTypeID equals fat.FeatureActionTypeID
                                              where activity.RoleID == UT.RoleID
                                              orderby feature.SortOrder
                                              select new FeatureModel
                                              {
                                                  IsFeatureActionChecked = activity.IsChecked,
                                                  FeatureID = action.FeatureID,
                                                  FeatureActionID = action.FeatureActionID,
                                                  FeatureActionTypeID = action.FeatureActionTypeID,
                                                  FeatureActionType = fat.DisplayName
                                              },
                                   BUs = (from usrbu in db.UserBusinessUnits where usrbu.UserRoleUserTypeID == UT.UserRoleUserTypeID
                                          select usrbu.BusinessUnitID).ToList()
                               }).ToList();
                foreach (var dt in Result1)
                {
                    dt.TokenID = loginservice.InsertLoginTime(token);
                }

                //MPOWRLogManager.LogMessage("This Information is to check the information logs are getting logged in blog");
                //MPOWRLogManager.LogException("This Error is to check the logs in blob as error");

                return Ok(Result1);
            }

            return NotFound();

        }

       // [AuthorizeUser]
        [Route("api/user/GetCurrentUser")]
        public HttpResponseMessage GetCurrentUser(string userName, string appName)
        {
            MPOWRResponse response = new MPOWRResponse { Status = MPOWRConstants.Failed, Data = null, Message = "" };
            try
            {
                var Result1 = (from dat in db.Users
                               where dat.UserID == userName
                               join UT in db.UserRoleUserTypes on dat.UserID equals UT.UserID
                               from app in db.AppConfig
                               where app.ShortName == MPOWRConstants.ApplicationName
                               select new
                               {
                                   firstName = dat.FirstName,
                                   lastName = dat.LastName,
                                   ApplicationID = app.Params,
                                   LoginStatus = true,
                                   UserName = dat.UserID,
                                   IsAdmin = UT.IsAdmin,
                                   RoleID = UT.RoleID
                               }).ToList();



                response.Data = Result1[0];
                response.Status = MPOWRConstants.Success;
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, response);
            }
            catch (MPOWRException contraException)
            {
                MPOWRLogManager.LogMessage(MPOWRConstants.ExceptionFrom + contraException.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + MPOWRConstants.ExceptionFrom + contraException.ToString());
                response.Message = contraException.Message;
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, response);
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(MPOWRConstants.ExceptionFrom + ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + MPOWRConstants.ExceptionFrom + ex.ToString());
                response.Message = MPOWRMessages.GENERIC_API_EXCEPTION;
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, response);
            }



        }
        // last updated method

        [Route("api/Users/GetLastUpdated")]
        public IHttpActionResult GetLastUpdated(int  VersionId)
        {
            try
            {
                var Result = string.Empty;
                DbCommand command = db.Database.Connection.CreateCommand();
                command.CommandText = "usp_GetLastRefreshDate";
                command.Parameters.Add(new SqlParameter("@VERSION_ID", VersionId));               
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
             
                return Ok(Result);
            }
            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogMessage("api/Users/GetLastUpdated2 " + ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

                return null;

            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage("api/Users/GetLastUpdated1 " + ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator+ "api/Users/GetLastUpdated" + ex.ToString());
                return null;
            }

        }

        public IHttpActionResult ExitUser(user user)
        {
            try
            {
                //string path = ConfigurationManager.AppSettings["Path"];
                //if (!System.IO.File.Exists(path))
                //{
                //    System.IO.FileStream fs = System.IO.File.Create(path);
                //    fs.Close();

                //}
                //using (StreamWriter writer = new StreamWriter(path, true))
                //{
                //    string a = user.Username;

                //    a += "               " + DateTime.Now;
                //    a += "               Logout Successfully";

                //    writer.WriteLine(a);
                //    writer.Close();
                //}
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
            return Ok();

        }
    }



    public class user
    {
        public string Username { get; set; }
    }
}
