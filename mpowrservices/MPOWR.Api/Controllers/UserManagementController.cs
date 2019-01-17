using MPOWR.Dal.Models;
using MPOWR.Api.Service;
using MPOWR.Api.ViewModel;
using MPOWR.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Http;
using System.Web.UI;
using MPOWR.Core;
using MPOWR.Api.App_Start;
using MPOWR.Dal;
using System.Collections.ObjectModel;
using MPOWR.Model;
using System.Collections;

namespace MPOWR.Api.Controllers
{
    [AuthorizeUser]
    public class UserManagementController : ApiController
    {

        private MPOWREntities db = new MPOWREntities();

        #region CRUD OPERATIONS


        /// <summary>
        /// This method is to Add the user details to the table
        /// </summary>
        /// <returns></returns>
        string Container = ConfigurationManager.AppSettings["environment"];
        [Route("api/UserManagement/UserManagement_Create_Update")]
        public IHttpActionResult UserManagement_Create_Update(UserManagementModel user)
        {

            UsersViewModel UserInput = new UsersViewModel();
            UserRoleUsertypeViewModel InputUserRoleType = new UserRoleUsertypeViewModel();
            UserCountryViewModel InputUserRoleType_country = new UserCountryViewModel();
            UserDistrictViewModel InputUserRoleType_District = new UserDistrictViewModel();
            UserBusinessUnitViewModel InputUserRoleType_BusinessUnit = new UserBusinessUnitViewModel();
            UserPartnerTypeViewModel InputUserRoleType_Rtm = new UserPartnerTypeViewModel();

            User userTable = new User();
            UserRoleUserType urutTable = new UserRoleUserType();
            UserBusinessUnit urBuTable = new UserBusinessUnit();
            UserGeo urGeoTable = new UserGeo();
            UserCountry urCtryTable = new UserCountry();
            UserDistrict urDistTable = new UserDistrict();
            UserRTM urPtypeTable = new UserRTM();
            string password = null;
            string userid = null;
            int urutId = 0;
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            DateTime createdDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            Random generator = new Random();
            System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();


            //UserTable create and update
            var existing_user = (from usertable in db.Users where (usertable.UserID == user.EmailID) select usertable).FirstOrDefault();
            try
            {
                UserInput.FirstName = user.FirstName;
                UserInput.LastName = user.LastName;
                UserInput.EmailID = user.EmailID;
                UserInput.IsActive = Convert.ToBoolean(user.IsActive);
                //UserInput.ModifiedBy = user.EmailID;
                //UserInput.ModifiedDate = DateTime.Now;



                if (existing_user == null && user.UserID == null)
                {
                    //adding new user into User table
                    userTable = new User();
                    password = generator.Next(1, 10000).ToString("D4");
                    byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
                    UserInput.Password = crypto;
                    UserInput.UserID = user.EmailID;

                    UserInput.CreatedBy = user.LoggedUser;
                    UserInput.CreatedDate = createdDate;
                    UserInput.ModifiedBy = user.LoggedUser;
                    UserInput.ModifiedDate = createdDate;
                    userTable = MapperService.Map(UserInput);
                    db.Users.Add(userTable);
                    db.SaveChanges();
                    userid = userTable.UserID;
                }
                else if (existing_user != null)
                {
                    //Update user inputs into User table
                    if (user.UserID == null)
                    {
                        return Ok("User exists");
                        //  return Ok(UserConstants.user_exists.ToString());
                    }
                    else
                    {
                        userTable = new User();
                        UserInput.UserID = user.UserID;
                        UserInput.Password = existing_user.Password;
                        UserInput.CreatedBy = existing_user.CreatedBy;
                        UserInput.CreatedDate = existing_user.CreatedDate;
                        UserInput.ModifiedBy = user.LoggedUser;
                        UserInput.ModifiedDate = createdDate;
                        userTable = MapperService.Map(UserInput);
                        userTable.ID = existing_user.ID;
                        db.Set<User>().AddOrUpdate(userTable);
                        db.SaveChanges();
                        userid = userTable.UserID;
                    }


                }

                //UserTable create and update

                InputUserRoleType.UserTypeID = user.userTypeDetails.UserTypeID;
                InputUserRoleType.UserID = user.EmailID;
                if (user.role.RoleID != 0)
                {
                    InputUserRoleType.RoleID = user.role.RoleID;
                }
                //if (user.RegionDetails.RegionID != 0)
                //{
                //    InputUserRoleType.RegionID = user.RegionDetails.RegionID;
                //}


                InputUserRoleType.IsAdmin = user.IsAdmin;
                InputUserRoleType.GlossaryApprover = user.IsAdmin ? user.GlossaryApprover : false;

                InputUserRoleType.ModifiedDate = createdDate;
                if (user.UserRoleUserType == null)
                {
                    //insert new inputs into UserRoleuserType table
                    urutTable = new UserRoleUserType();
                    InputUserRoleType.CreatedBy = user.LoggedUser;
                    InputUserRoleType.CreatedDate = createdDate;
                    InputUserRoleType.ModifiedBy = user.LoggedUser;
                    InputUserRoleType.ModifiedDate = createdDate;
                    urutTable = MapperService.Map(InputUserRoleType);
                    urutTable.EditNonBUModParam = user.BUAccess;
                    urutTable.DataUpload = user.DataUpload;
                    db.UserRoleUserTypes.Add(urutTable);
                    db.SaveChanges();
                    urutId = urutTable.UserRoleUserTypeID;
                }
                else if (user.UserRoleUserType.UserRoleUserTypeID != 0)
                {
                    var exixsting_UserRoleType = (from URTtable in db.UserRoleUserTypes where (URTtable.UserRoleUserTypeID == user.UserRoleUserType.UserRoleUserTypeID) select URTtable).FirstOrDefault();
                    //Update user inputs into UserRoleuserType table
                    if (exixsting_UserRoleType != null)
                    {
                        urutTable = new UserRoleUserType();
                        InputUserRoleType.UserRoleUserTypeID = user.UserRoleUserType.UserRoleUserTypeID;
                        InputUserRoleType.CreatedBy = exixsting_UserRoleType.CreatedBy;
                        InputUserRoleType.CreatedDate = exixsting_UserRoleType.CreatedDate;
                        InputUserRoleType.ModifiedBy = user.LoggedUser;
                        InputUserRoleType.ModifiedDate = createdDate;
                        urutTable = MapperService.Map(InputUserRoleType);
                        urutTable.EditNonBUModParam = user.BUAccess;
                        urutTable.DataUpload = user.DataUpload;
                        db.Set<UserRoleUserType>().AddOrUpdate(urutTable);
                        db.SaveChanges();
                        urutId = urutTable.UserRoleUserTypeID;
                    }

                }

                //UserBusinessunitTable  create and update

                if (urutId != 0)
                {
                    if (user.BusinessUnitDetails != null)
                    {
                        var listdata = (from URBu in db.UserBusinessUnits where (URBu.UserRoleUserTypeID == urutId) select URBu).ToList();
                        if (listdata.Count != 0)
                        {
                            foreach (var BU in listdata)
                            {
                                BU.ModifiedBy = user.LoggedUser;
                                BU.ModifiedDate = createdDate;
                                db.UserBusinessUnits.Remove(BU);
                                db.SaveChanges();
                            }
                        }
                        foreach (var userBu in user.BusinessUnitDetails)
                        {
                            if (userBu.BusinessUnitID != 0)
                            {
                                InputUserRoleType_BusinessUnit = new UserBusinessUnitViewModel();
                                InputUserRoleType_BusinessUnit.UserRoleUserTypeID = urutId;
                                if (userBu.BusinessUnitID != 0)
                                {
                                    InputUserRoleType_BusinessUnit.BusinessUnitID = userBu.BusinessUnitID;
                                }
                                //insert into UserBusiness Table
                                urBuTable = new UserBusinessUnit();
                                InputUserRoleType_BusinessUnit.CreatedBy = user.LoggedUser;
                                InputUserRoleType_BusinessUnit.CreatedDate = createdDate;
                                InputUserRoleType_BusinessUnit.ModifiedBy = user.LoggedUser;
                                InputUserRoleType_BusinessUnit.ModifiedDate = createdDate;
                                urBuTable = MapperService.Map(InputUserRoleType_BusinessUnit);
                                db.UserBusinessUnits.Add(urBuTable);
                                db.SaveChanges();
                            }
                        }
                    }
                }

                //UserRTM table create and update

                if (urutId != 0)
                {
                    if (user.partnerType != null)
                    {
                        var listdata = (from URtm in db.UserRTMs where (URtm.UserRoleUserTypeID == urutId) select URtm).ToList();
                        if (listdata.Count != 0)
                        {
                            foreach (var pt in listdata)
                            {
                                pt.ModifiedDate = createdDate;
                                pt.ModifiedBy = user.LoggedUser;
                                db.UserRTMs.Remove(pt);
                                db.SaveChanges();
                            }
                        }
                        foreach (var userpt in user.partnerType)
                        {
                            if (userpt.PartnerTypeID != 0)
                            {
                                InputUserRoleType_Rtm = new UserPartnerTypeViewModel();
                                InputUserRoleType_Rtm.UserRoleUserTypeID = urutId;
                                if (userpt.PartnerTypeID != 0)
                                {
                                    InputUserRoleType_Rtm.PartnerTypeID = userpt.PartnerTypeID;
                                }
                                //insert into UserRTM Table
                                urPtypeTable = new UserRTM();
                                InputUserRoleType_Rtm.CreatedBy = user.LoggedUser;
                                InputUserRoleType_Rtm.CreatedDate = createdDate;
                                InputUserRoleType_Rtm.ModifiedBy = user.LoggedUser;
                                InputUserRoleType_Rtm.ModifiedDate = createdDate;
                                urPtypeTable = MapperService.Map(InputUserRoleType_Rtm);
                                db.UserRTMs.Add(urPtypeTable);
                                db.SaveChanges();
                            }
                        }
                    }
                }



                //UserDistrictsTable  create and update

                if (urutId != 0)
                {
                    if (user.districts != null)
                    {
                        var listdist = (from URdist in db.UserDistricts where (URdist.UserRoleUserTypeID == urutId) select URdist).ToList();
                        if (listdist.Count != 0)
                        {
                            foreach (var dist in listdist)
                            {
                                dist.ModifiedDate = createdDate;
                                dist.ModifiedBy = user.LoggedUser;
                                db.UserDistricts.Remove(dist);
                                db.SaveChanges();
                            }
                        }
                        foreach (var userDis in user.districts)
                        {
                            if (userDis.DistrictID != 0)
                            {
                                InputUserRoleType_District = new UserDistrictViewModel();
                                InputUserRoleType_District.UserRoleUserTypeID = urutId;
                                if (userDis.DistrictID != 0)
                                {
                                    InputUserRoleType_District.DistrictID = userDis.DistrictID;
                                }

                                //insert into userDissiness Table
                                urDistTable = new UserDistrict();
                                InputUserRoleType_District.CreatedBy = user.LoggedUser;
                                InputUserRoleType_District.CreatedDate = createdDate;
                                InputUserRoleType_District.ModifiedBy = user.LoggedUser;
                                InputUserRoleType_District.ModifiedDate = createdDate;
                                urDistTable = MapperService.Map(InputUserRoleType_District);
                                db.UserDistricts.Add(urDistTable);
                                db.SaveChanges();
                            }
                        }
                    }
                }


                //UserCountriesTable  create and update

                if (urutId != 0)
                {
                    if (user.countries != null)
                    {
                        var listctry = (from URCtry in db.UserCountries where (URCtry.UserRoleUserTypeID == urutId) select URCtry).ToList();
                        if (listctry.Count != 0)
                        {
                            foreach (var ctry in listctry)
                            {
                                ctry.ModifiedDate = createdDate;
                                ctry.ModifiedBy = user.LoggedUser;
                                db.UserCountries.Remove(ctry);
                                db.SaveChanges();
                            }
                        }
                        foreach (var userCtry in user.countries)
                        {
                            if (userCtry.CountryID != 0)
                            {
                                InputUserRoleType_country = new UserCountryViewModel();
                                InputUserRoleType_country.UserRoleUserTypeID = urutId;
                                if (userCtry.CountryID != 0)
                                {
                                    InputUserRoleType_country.CountryID = userCtry.CountryID;
                                }

                                //insert into userCtrysiness Table
                                urCtryTable = new UserCountry();
                                InputUserRoleType_country.CreatedBy = user.LoggedUser;
                                InputUserRoleType_country.CreatedDate = createdDate;
                                InputUserRoleType_country.ModifiedBy = user.LoggedUser;
                                InputUserRoleType_country.ModifiedDate = createdDate;
                                urCtryTable = MapperService.Map(InputUserRoleType_country);
                                db.UserCountries.Add(urCtryTable);
                                db.SaveChanges();
                            }
                        }
                    }
                }

                if (userid != null && password != null)
                {
                    emaildata data1 = new emaildata();
                    data1.to = userid;// ConfigurationManager.AppSettings["ToEmail"];
                                      //  data1.subject = UserConstants.new_user_created.ToString();
                    data1.subject = "Access to MPOWR " + ConfigurationManager.AppSettings["environment"];
                    data1.body = "Hi " + user.FirstName + ",</br></br>You have been granted access to MPOWR " + ConfigurationManager.AppSettings["environment"] + " system, that have just been launched.</br></br>Please find the access details below:</br><b>" + ConfigurationManager.AppSettings["environment"] + " Link</b> : " + ConfigurationManager.AppSettings["link"] + "</br><b>User Name</b> : " + userid + "</br>  <b>Password</b> : " + password +
                       "</br></br>Some recommendations for a successful log on and appropriate access rights:</br>" +
                        "<i><ul><li>Please try to Use <b><i>Chrome or Internet Explorer V11 Browser</i></b> while using MPOWR tool.</li>" +
                        "<li>Secondly,  please try to clear cache once before running  a new simulation." +
                        "</br><b>To clear cache:</b> Please press on the keyboard: Ctrl+Shift+Delete keys, Select Options related to History and Cookies & website data and click Delete.</li>" +
                        "<li>Copy and paste of password may include blank spaces. Type the password to avoid issues while log in.</li></ul></i>" +
                        "For any issues please contact: <a href='mailto:ContraAppSupport@brillio.com' target='_top'>ContraAppSupport@brillio.com</a>";
                    UserManagement_SendMail(data1);

                    // emaildata data2 = new emaildata();
                    // data2.to = "shritha.mohan@brillio.com";
                    // data2.subject = UserConstants.new_user_created.ToString();
                    // // data2.subject = UserConstants.new_user_created.ToString();
                    // data2.body = "Your have been given access to the MPOWR" + Container + " application. </br>User Name : " + userid + "</br> Password : " + password;
                    //UserManagement_SendMail(data2);

                }

                //Geo changes to store multiple Geo details - 9th Jan, 2018
                if (user.GeoDetails != null)
                {
                    //To remove existing Geo details in case of Edit/Update action.
                    var listSubRegion = (from URGeo in db.UserGeos where (URGeo.UserRoleUserTypeID == urutId) select URGeo).ToList();
                    if (listSubRegion.Count != 0)
                    {
                        foreach (var subreg in listSubRegion)
                        {
                            subreg.ModifiedDate = createdDate;
                            subreg.ModifiedBy = user.LoggedUser;
                            db.UserGeos.Remove(subreg);
                            db.SaveChanges();
                        }
                    }
                    foreach (var userGeo in user.GeoDetails)
                    {
                        if (userGeo.GeoID != 0)
                        {
                            urGeoTable = new UserGeo();
                            urGeoTable.UserRoleUserTypeID = urutId;
                            if (userGeo.GeoID != 0)
                            {
                                urGeoTable.GeoID = userGeo.GeoID;
                            }

                            //insert into UserGeo Table
                            urGeoTable.CreatedBy = user.LoggedUser;
                            urGeoTable.CreatedDate = createdDate;
                            urGeoTable.ModifiedBy = user.LoggedUser;
                            urGeoTable.ModifiedDate = createdDate;
                            db.UserGeos.Add(urGeoTable);
                            db.SaveChanges();
                        }
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


        /// <summary>
        /// This method is to return User details to the view
        /// </summary>
        /// <returns></returns>
        [Route("api/UserManagement/UserManagement_Read")]
        [HttpGet]
        public IHttpActionResult UserManagement_Read()
        {
            try
            {
                var Result = string.Empty;
                DbCommand command = db.Database.Connection.CreateCommand();
                command.CommandText = "usp_User_Mangement_View";
                command.CommandTimeout = 1000;
                command.CommandType = System.Data.CommandType.StoredProcedure;
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
                db.Database.Connection.Close();

                dynamic resultList = JsonConvert.DeserializeObject(Result);

                var buItem = new JArray();
                JObject JBUobj = new JObject();
                JBUobj.Add("UserBusinessUnitID", 0);
                JBUobj.Add("BusinessUnitID", 0);
                JBUobj.Add("DisplayName", "");
                var distItem = new JArray();
                JObject JDistrictobj = new JObject();
                JDistrictobj.Add("UserDistrictID", 0);
                JDistrictobj.Add("DistrictID", 0);
                JDistrictobj.Add("DisplayName", "");
                var countItem = new JArray();
                JObject JCountryobj = new JObject();
                JCountryobj.Add("UserCountryID", 0);
                JCountryobj.Add("CountryID", 0);
                JCountryobj.Add("DisplayName", "");
                var geoItem = new JArray();
                JObject JGeoobj = new JObject();
                JGeoobj.Add("UserGeoID", 0);
                JGeoobj.Add("GeoID", 0);
                JGeoobj.Add("DisplayName", "");
                var partnertype = new JArray();
                JObject Jpartnertypeobj = new JObject();
                Jpartnertypeobj.Add("UserRTMID", 0);
                Jpartnertypeobj.Add("partnerTypeID", 0);
                Jpartnertypeobj.Add("DisplayName", "");

                foreach (var item in resultList.users)
                {
                    if (item.user.districts == null)
                    {
                        distItem = new JArray();
                        distItem.Add(JDistrictobj);
                        item.user.districts = distItem;

                    }

                    if (item.user.countries == null)
                    {
                        countItem = new JArray();
                        countItem.Add(JCountryobj);
                        item.user.countries = countItem;

                    }
                    if (item.user.GeoDetails == null)
                    {
                        geoItem = new JArray();
                        geoItem.Add(JGeoobj);
                        item.user.GeoDetails = geoItem;

                    }

                    if (item.user.BusinessUnitDetails == null)
                    {
                        buItem = new JArray();
                        buItem.Add(JBUobj);
                        item.user.BusinessUnitDetails = buItem;

                    }

                    if (item.user.partnerType == null)
                    {
                        partnertype = new JArray();
                        partnertype.Add(Jpartnertypeobj);
                        item.user.partnerType = partnertype;

                    }

                }


                return Ok(resultList);

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


        /// <summary>
        /// This method is to delete the user details from the table
        /// </summary>
        /// <returns></returns>
        [Route("api/UserManagement/UserManagement_Delete")]
        [HttpPost]
        public IHttpActionResult UserManagement_Delete(UserManagementModel user)
        {
            User userTable = new User();
            UserRoleUserType urutTable = new UserRoleUserType();
            // UserBusinessUnit exist_bu = new UserBusinessUnit();
            UserCountry urCtryTable = new UserCountry();
            UserDistrict urDistTable = new UserDistrict();

            try
            {

                //UserBusinessUnit delete operation

                if (user.BusinessUnitDetails != null)
                {
                    foreach (var userBu in user.BusinessUnitDetails)
                    {
                        if (userBu.BusinessUnitID != 0)
                        {
                            UserBusinessUnit exist_bu = (from URBu in db.UserBusinessUnits where (URBu.UserBusinessUnitID == userBu.UserBusinessUnitID && URBu.UserRoleUserTypeID == user.UserRoleUserType.UserRoleUserTypeID) select URBu).FirstOrDefault();
                            if (exist_bu != null)
                            {
                                db.UserBusinessUnits.Remove(exist_bu);
                                db.SaveChanges();
                            }
                        }
                    }
                }


                //UserRTM delete operation

                if (user.partnerType != null)
                {
                    foreach (var userpt in user.partnerType)
                    {
                        if (userpt.PartnerTypeID != 0)
                        {
                            UserRTM exist_pt = (from URpt in db.UserRTMs where (URpt.PartnerTypeID == userpt.PartnerTypeID && URpt.UserRoleUserTypeID == user.UserRoleUserType.UserRoleUserTypeID) select URpt).FirstOrDefault();
                            if (exist_pt != null)
                            {
                                db.UserRTMs.Remove(exist_pt);
                                db.SaveChanges();
                            }
                        }
                    }
                }

                //UserDistricts delete operation

                if (user.districts != null)
                {
                    foreach (var userdis in user.districts)
                    {
                        if (userdis.DistrictID != 0)
                        {
                            UserDistrict exist_dis = (from URdist in db.UserDistricts where (URdist.UserDistrictID == userdis.UserDistrictID && URdist.UserRoleUserTypeID == user.UserRoleUserType.UserRoleUserTypeID) select URdist).FirstOrDefault();
                            if (exist_dis != null)
                            {
                                db.UserDistricts.Remove(exist_dis);
                                db.SaveChanges();
                            }
                        }
                    }
                }

                //UserCountries delete operation

                if (user.countries != null)
                {
                    foreach (var userCtry in user.countries)
                    {
                        if (userCtry.CountryID != 0)
                        {
                            UserCountry exist_ctry = (from URCtry in db.UserCountries where (URCtry.UserCountryID == userCtry.UserCountryID && URCtry.UserRoleUserTypeID == user.UserRoleUserType.UserRoleUserTypeID) select URCtry).FirstOrDefault();
                            if (exist_ctry != null)
                            {
                                db.UserCountries.Remove(exist_ctry);
                                db.SaveChanges();
                            }
                        }
                    }
                }

                //UserGeo delete operation

                if (user.GeoDetails != null)
                {
                    foreach (var userGeo in user.GeoDetails)
                    {
                        if (userGeo.GeoID != 0)
                        {
                            UserGeo exist_geo = (from URGeo in db.UserGeos where (URGeo.UserGeoID == userGeo.UserGeoID && URGeo.UserRoleUserTypeID == user.UserRoleUserType.UserRoleUserTypeID) select URGeo).FirstOrDefault();
                            if (exist_geo != null)
                            {
                                db.UserGeos.Remove(exist_geo);
                                db.SaveChanges();
                            }
                        }
                    }
                }


                //UserRoleUserType table delete operation

                if (user.UserRoleUserType.UserRoleUserTypeID != 0)
                {
                    List<UserCountry> child_ctry = (from URCtry in db.UserCountries where (URCtry.UserRoleUserTypeID == user.UserRoleUserType.UserRoleUserTypeID) select URCtry).ToList();
                    List<UserDistrict> child_dis = (from URdist in db.UserDistricts where (URdist.UserRoleUserTypeID == user.UserRoleUserType.UserRoleUserTypeID) select URdist).ToList();
                    List<UserBusinessUnit> child_bu = (from URBu in db.UserBusinessUnits where (URBu.UserRoleUserTypeID == user.UserRoleUserType.UserRoleUserTypeID) select URBu).ToList();
                    List<UserRTM> child_pt = (from URpt in db.UserRTMs where (URpt.UserRoleUserTypeID == user.UserRoleUserType.UserRoleUserTypeID) select URpt).ToList();

                    if (child_ctry.Count == 0 && child_dis.Count == 0 && child_bu.Count == 0 && child_pt.Count == 0)
                    {
                        UserRoleUserType exist_urut = (from urut in db.UserRoleUserTypes where (urut.UserRoleUserTypeID == user.UserRoleUserType.UserRoleUserTypeID) select urut).FirstOrDefault();
                        if (exist_urut != null)
                        {
                            db.UserRoleUserTypes.Remove(exist_urut);
                            db.SaveChanges();
                        }
                    }

                    // User table delete operation
                    List<UserRoleUserType> child_urut = (from urut in db.UserRoleUserTypes where (urut.UserID == user.UserID) select urut).ToList();
                    if (child_urut.Count == 0 && user.UserID != null)
                    {
                        User exist_user = (from exuser in db.Users where (exuser.UserID == user.UserID) select exuser).FirstOrDefault();
                        if (exist_user != null)
                        {
                            db.Users.Remove(exist_user);
                            db.SaveChanges();
                        }
                    }
                }
                return Ok("Deleted User Successfully");
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


        /// <summary>
        /// This method is to Reset the user password from the table
        /// </summary>
        /// <returns></returns>
        [Route("api/UserManagement/UserManagement_ResetPassword")]
        public IHttpActionResult UserManagement_ResetPassword(UserManagementModel user)
        {
            Random generator = new Random();
            System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();

            try
            {
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                DateTime createdDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);

                var existing_user = (from usertable in db.Users where (usertable.UserID == user.UserID) select usertable).FirstOrDefault();
                UsersViewModel UserInput = new UsersViewModel();
                if (existing_user != null)
                {
                    string password = generator.Next(1, 10000).ToString("D4");
                    byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
                    existing_user.Password = crypto;
                    existing_user.ModifiedBy = user.LoggedUser;
                    existing_user.ModifiedDate = createdDate;
                    db.Set<User>().AddOrUpdate(existing_user);
                    db.SaveChanges();
                    if (user.UserID != null && password != null)
                    {

                        emaildata data1 = new emaildata();
                        data1.to = user.UserID;// ConfigurationManager.AppSettings["ToEmail"];
                        data1.subject = "Password Reset in MPOWR " + Container;
                        data1.body = "Hi " + user.FirstName + ",</br></br>Your password has been Reset in MPOWR " + ConfigurationManager.AppSettings["environment"] + ".</br></br>Please find the access details below:</br><b>" + ConfigurationManager.AppSettings["environment"] + " Link</b> : " + ConfigurationManager.AppSettings["link"] + "</br><b>User Name</b> : " + user.UserID + "</br>  <b>Password</b> : " + password +
                        "</br></br>Some recommendations for a successful log on and appropriate access rights:</br>" +
                         "<i><ul><li>Please try to Use <b><i>Chrome or Internet Explorer V11 Browser</i></b> while using MPOWR tool.</li>" +
                         "<li>Secondly,  please try to clear cache once before running  a new simulation." +
                         "</br><b>To clear cache:</b> Please press on the keyboard: Ctrl+Shift+Delete keys, Select Options related to History and Cookies & website data and click Delete.</li>" +
                         "<li>Copy and paste of password may include blank spaces. Type the password to avoid issues while log in.</li></ul></i>" +
                         "For any issues please contact: <a href='mailto:ContraAppSupport@brillio.com' target='_top'>ContraAppSupport@brillio.com</a>";
                        UserManagement_SendMail(data1);

                        //emaildata data2 = new emaildata();
                        //data2.to = ConfigurationManager.AppSettings["From"];
                        //data2.subject = data1.subject;
                        //data2.body = "Your password has been Reset. </br>User Name : " + user.UserID + "</br> The new password is " + password;
                        //UserManagement_SendMail(data2);
                    }
                    return Ok();
                }

                return Ok("User Does not exist");
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

        [Route("api/UserManagement/UserManagement_SendMail")]
        [HttpGet]
        public IHttpActionResult UserManagement_SendMail(emaildata data)
        {
            try
            {
                SetMileStoneController setctrl = new SetMileStoneController();
                setctrl.PostEmail(data);
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
        [Route("api/UserManagement/SendMailtoUsers")]
        [HttpGet]
        public IHttpActionResult SendMailtoUsers()
        {
            try
            {
                var users = db.Users.Where(y => y.IsActive == true).ToList();

               // foreach (var item in users)
              //  {
                    MailMessage mail = new MailMessage();
                    mail.IsBodyHtml = true;
                    mail.Subject = "Hi " + "kk" + ",</ br ></ br>"
                        + "To improve availability and response time of <b>PIVOT</b> tool, we have moved the tool to new Infrastructure.</ br>" +
                        "So the old Production link <a href='https://contraweb.azurewebsites.net#' target='_top'>https://contraweb.azurewebsites.net#</a>  will not be working,  please use the new Production link  given below.</ br></ br>" +
                        "Please find the new Partner Incentives Value Optimization Tool(PIVOT) Production link below:</ br>" +
                        "<b>" + ConfigurationManager.AppSettings["environment"] + " Link</b> : " + ConfigurationManager.AppSettings["link"] + "</br><b>User Name</b> : " + "11.44" + "</br></br> <b> Note: Your Password will remain same and it will work with new Production link.</b></br></br>" +
                        "For any issues please contact: < a href = 'mailto:ContraAppSupport@brillio.com' target = '_top' > ContraAppSupport@brillio.com </ a ></ br></ br>With regards</br>ContraAppSupport Team";
                               ;
                    mail.Body = "New PIVOT link";
                    mail.Bcc.Add(new MailAddress(ConfigurationManager.AppSettings["From"]));
                    mail.From = new MailAddress(ConfigurationManager.AppSettings["From"]);
                    mail.To.Add("raveendra.v@brillio.com");
                    System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient();
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
              //  }

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

        [Route("api/UserManagement/UserManagement_IsAdmin")]
        [HttpGet]
        public IHttpActionResult UserManagement_IsAdmin(string userid)
        {

            var isAdmin = (from URTtable in db.UserRoleUserTypes where (URTtable.UserID == userid) select new { URTtable.IsAdmin }).FirstOrDefault();

            try
            {
                if (isAdmin.IsAdmin == true)
                {
                    return Ok(true);
                }
                return Ok(false);

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

        [Route("api/UserManagement/UserManagement_IsBUAccess")]
        [HttpGet]
        public IHttpActionResult UserManagement_IsBUAccess(string userid)
        {

            var BuAccess = (from URTtable in db.UserRoleUserTypes where (URTtable.UserID == userid) select new { URTtable.EditNonBUModParam }).FirstOrDefault();

            try
            {
                if (BuAccess.EditNonBUModParam == true)
                {
                    return Ok(true);
                }
                return Ok(false);

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

        [Route("api/UserManagement/UserManagement_IsDataUploadAccess")]
        [HttpGet]
        public IHttpActionResult UserManagement_IsDataUploadAccess(string userid)
        {

            var DataUploadAccess = (from URTtable in db.UserRoleUserTypes where (URTtable.UserID == userid) select new { URTtable.DataUpload }).FirstOrDefault();

            try
            {
                if (DataUploadAccess.DataUpload == true)
                {
                    return Ok(true);
                }
                return Ok(false);

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

        #endregion

        #region DROPDOWN SELECTION LIST


        /// <summary>
        /// This method is to get dropdown list based on selected access dropDown selection
        /// </summary>
        /// <returns></returns>
        [Route("api/UserManagement/UserManagement_AccessDPClick_Details")]
        [HttpPost]
        public IHttpActionResult UserManagement_AccessDPClick_Details(UsertypeViewModel user)
        {

            List<object> Result = new List<object>();
            string access;


            try
            {
                access = user.DisplayName.Trim().ToUpper();
                var BusinessUnitsList = (from bu in db.BusinessUnits where bu.IsActive == true select new { bu.BusinessUnitID, bu.DisplayName }).ToList();

                if (access == "WORLD WIDE")
                {
                    Result.Add(BusinessUnitsList);
                    return Ok(Result);
                }
                else if (access == "DISTRICT")
                {
                    BusinessUnitsList = (from BUs in db.BusinessUnits where BUs.IsActive == true && BUs.DisplayName != MPOWRConstants.ComputeVolume && BUs.DisplayName != MPOWRConstants.ComputeValue select new { BUs.BusinessUnitID, BUs.DisplayName }).ToList();
                    var DistrictsList = (from dist in db.Districts where dist.IsActive == true && (dist.ShortName != "NM") select new { dist.DistrictID, dist.DisplayName }).ToList();
                    var country = (from ctry in db.Countries where ctry.IsActive == true && (ctry.DisplayName == "UNITED STATES") select new { ctry.CountryID, ctry.DisplayName, ctry.GeoID }).FirstOrDefault();
                    var geo = (from sub in db.Geos where sub.IsActive == true && (sub.GeoID == country.GeoID) select new { sub.GeoID, sub.DisplayName }).FirstOrDefault();
                    BusinessUnitsList = (from bu in db.BusinessUnits where bu.DisplayName != MPOWRConstants.ComputeVolume && bu.DisplayName != MPOWRConstants.ComputeValue select new { bu.BusinessUnitID, bu.DisplayName }).ToList();
                    Result.Add(BusinessUnitsList);
                    Result.Add(geo);
                    Result.Add(country);
                    Result.Add(DistrictsList);
                }
                else
                {
                    var geo = (from sub in db.Geos where sub.IsActive == true select new { sub.GeoID, sub.DisplayName }).ToList();
                    Result.Add(BusinessUnitsList);
                    Result.Add(geo);
                }

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
        }

        /// <summary>
        /// This method is to get dropdown list based on selected Geo dropDown  selection
        /// </summary>
        /// <returns></returns>
        [Route("api/UserManagement/UserManagement_GeoDPClick_Details")]
        [HttpPost]
        public IHttpActionResult UserManagement_GeoDPClick_Details(List<Geo> user)
        {

            List<object> Result = new List<object>();
            try
            {
                if (user != null && user.Count > 0)
                {
                    IEnumerable<object> CountriesList = null;
                    int IsActive = 0;
                    foreach (var geo in user)
                    {
                        if (geo.GeoID != 0)
                        {
                            IEnumerable<object> CountList = null;
                            CountList = (from count in db.Countries where count.IsActive == true && count.GeoID == geo.GeoID select new { count.CountryID, count.DisplayName }).ToList();
                            if (IsActive == 0)
                                CountriesList = CountList;
                            else
                                CountriesList = CountriesList.Concat(CountList);
                            IsActive++;

                        }
                    }
                    List<short?> cntryarr = new List<short?>();
                    foreach (var cntry in user)
                    {
                        cntryarr.Add(cntry.GeoID);
                    }
                    IEnumerable<object> InvalidCountriesList = (from count in db.Countries where count.IsActive == false && cntryarr.Contains(count.GeoID) select new { count.CountryID, count.DisplayName }).ToList();
                    CountriesList = CountriesList.Concat(InvalidCountriesList);
                    Result.Add(CountriesList);

                    var compGeoList = (from geo in db.AppConfig where geo.ShortName == MPOWRConstants.IsCompute select new { geo.Params }).ToList();
                    List<string> ComputeGeoList = compGeoList[0].Params.Split(',').ToList();
                    List<string> GeoList = (from geo in user select geo.GeoID.ToString()).ToList();
                    using (CountryDAL dal = new CountryDAL())
                    {
                        var BUList = dal.GetBUFromCountries(ComputeGeoList.ToList<dynamic>(), GeoList.ToList<dynamic>());
                        Result.Add(BUList);
                    }
                }

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

        }


        /// <summary>
        /// This method is to get dropdown list based on selected Country dropDown  selection
        /// </summary>
        /// <returns></returns>
        [Route("api/UserManagement/UserManagement_CountryDPClick_Details")]
        [HttpPost]
        public IHttpActionResult UserManagement_CountryDPClick_Details(List<Country> user)
        {
            List<object> Result = new List<object>();
            try
            {
                bool IsActive = false;
                foreach (var country in user)
                {
                    if (country.CountryID != 0)
                    {
                        if (country.CountryID == 138)
                        {
                            var DistrictsList = (from dist in db.Districts where dist.IsActive == true && dist.CountryID == country.CountryID && dist.ShortName != "NM" select new { dist.DistrictID, dist.DisplayName }).ToList();
                            Result.Add(DistrictsList);
                            IsActive = true;
                        }
                    }
                }
                if (IsActive == false)
                {
                    var District = new List<object>();
                    Result.Add(District);
                }
                var compCountryList = db.Database.SqlQuery<string>("select dbo.GetComputeCountries()").FirstOrDefault();
                List<string> ComputeCountryList = compCountryList.Split(',').ToList();
                List<string> CountriesList = (from country in user select country.CountryID.ToString()).ToList();
                using (CountryDAL dal = new CountryDAL())
                {
                    var BUList = dal.GetBUFromCountries(ComputeCountryList.ToList<dynamic>(), CountriesList.ToList<dynamic>());
                    Result.Add(BUList);
                }
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

        }


        /// <summary>
        ///  The Edit screen data population API when the user clicks for the very time
        /// </summary>
        /// <returns></returns>
        [Route("api/UserManagement/UserManagement_Edit_User_firstDetails")]
        [HttpPost]
        public IHttpActionResult UserManagement_Edit_User_firstDetails(UserModel user)
        {
            List<object> Result = new List<object>();

            //var list = new string[] { "World Wide", "Regional", "Sub-Regional" };
            var list = new string[] { "World Wide", "Geo", "Country", "District" };
            // var list = new string[] { EnumUserType.WW.ToString(), EnumUserType.Region.ToString(), EnumUserType.Subregion.ToString() };

            try
            {
                var RoleLists = (from role in db.Roles select new { role.RoleID, role.DisplayName, role.IsActive }).ToList();
                RoleLists.RemoveAll(x => x.IsActive == false && x.RoleID != user.user.role.RoleID);
                var RoleList = RoleLists.Select(x => new { x.RoleID, x.DisplayName }).ToList();

                var AccessList = (from usertype in db.UserTypes orderby usertype.CreatedDate select new { usertype.UserTypeID, usertype.DisplayName }).ToList();

                if (user.user.IsAdmin == true)
                {
                    AccessList = (from usertype in db.UserTypes where list.Contains(usertype.DisplayName) orderby usertype.CreatedDate select new { usertype.UserTypeID, usertype.DisplayName }).ToList();
                }
                Result.Add(RoleList);
                Result.Add(AccessList);
                IEnumerable<object> CountriesList = null;
                List<int> cntryarr = new List<int>();
                foreach (var cntry in user.user.countries)
                {
                    cntryarr.Add(cntry.CountryID);
                }
                IEnumerable<object> InvalidCountriesList = (from count in db.Countries where count.IsActive == false && cntryarr.Contains(count.CountryID) select new { count.CountryID, count.DisplayName }).ToList();
                if (user.user.GeoDetails != null && user.user.GeoDetails.Count > 0)
                {
                    int IsActive = 0;
                    foreach (var geo in user.user.GeoDetails)
                    {
                        if (geo.GeoID != 0)
                        {
                            IEnumerable<object> CountList = null;
                            CountList = (from count in db.Countries where count.IsActive == true && count.GeoID == geo.GeoID select new { count.CountryID, count.DisplayName }).ToList();
                            if (IsActive == 0)
                                CountriesList = CountList;
                            else
                                CountriesList = CountriesList.Concat(CountList);
                            IsActive++;

                        }
                    }
                }
                if (InvalidCountriesList.Count() > 0)
                {
                    CountriesList = CountriesList.Concat(InvalidCountriesList);
                }
                Result.Add(CountriesList);
                if (user.user.countries != null)
                {
                    int countryId = user.user.countries[0].CountryID;
                    var DistrictsLists = (from dist in db.Districts where dist.CountryID == countryId && dist.ShortName != "NM" select new { dist.DistrictID, dist.DisplayName, dist.IsActive }).ToList();

                    List<int> Districtarr = new List<int>();
                    foreach (var item in user.user.districts)
                    {
                        Districtarr.Add(item.DistrictID);
                    }
                    DistrictsLists.RemoveAll(x => x.IsActive == false && !Districtarr.Contains(x.DistrictID));
                    var DistrictsList = DistrictsLists.Select(x => new { x.DistrictID, x.DisplayName }).ToList();
                    Result.Add(DistrictsList);
                }
                List<buunit> BusinessUnitsList = new List<buunit>();

                if (user.user.userTypeDetails.UserTypeID == (int)EnumUserType.Geo)
                {
                    var compGeoList = (from geo in db.AppConfig where geo.ShortName == MPOWRConstants.IsCompute select new { geo.Params }).ToList();
                    List<string> ComputeGeoList = compGeoList[0].Params.Split(',').ToList();
                    List<string> GeoList = (from geo in user.user.GeoDetails select geo.GeoID.ToString()).ToList();
                    using (CountryDAL dal = new CountryDAL())
                    {
                        BusinessUnitsList = dal.GetBUFromCountries(ComputeGeoList.ToList<dynamic>(), GeoList.ToList<dynamic>());
                        // Result.Add(BusinessUnitsList);
                    }
                }
                else if (user.user.userTypeDetails.UserTypeID == (int)EnumUserType.Country)
                {
                    var compCountryList = db.Database.SqlQuery<string>("select dbo.GetComputeCountries()").FirstOrDefault();
                    List<string> ComputeCountryList = compCountryList.Split(',').ToList();
                    List<string> CountryList = new List<string>();

                    foreach (var c in CountriesList)
                    {
                        object b = c;
                        var property = b.GetType().GetProperty("CountryID");
                        var value = property.GetValue(b);
                        CountryList.Add(value.ToString());
                    }
                    using (CountryDAL dal = new CountryDAL())
                    {
                        BusinessUnitsList = dal.GetBUFromCountries(ComputeCountryList.ToList<dynamic>(), CountryList.ToList<dynamic>());
                        //       Result.Add(BusinessUnitsList);
                    }
                }
                else if (user.user.userTypeDetails.UserTypeID == (int)EnumUserType.District)
                {
                    BusinessUnitsList = (from BUs in db.BusinessUnits where BUs.IsActive == true && BUs.DisplayName != MPOWRConstants.ComputeVolume && BUs.DisplayName != MPOWRConstants.ComputeValue select new buunit { BusinessUnitID = BUs.BusinessUnitID, DisplayName = BUs.DisplayName }).ToList();
                    //     Result.Add(BusinessUnitsList);
                }
                else
                {
                    BusinessUnitsList = (from bu in db.BusinessUnits where bu.IsActive == true select new buunit { BusinessUnitID = bu.BusinessUnitID, DisplayName = bu.DisplayName }).ToList();
                    //       Result.Add(BusinessUnitsList);
                }
                List<int> BusinessUnitsarr = new List<int>();
                foreach (var item in user.user.BusinessUnitDetails)
                {
                    BusinessUnitsarr.Add(item.BusinessUnitID);
                }
                var InactiveBusinessUnitsList = (from business in db.BusinessUnits where business.IsActive == false && BusinessUnitsarr.Contains(business.BusinessUnitID) select new buunit { BusinessUnitID = business.BusinessUnitID, DisplayName = business.DisplayName }).ToList();
                BusinessUnitsList.AddRange(InactiveBusinessUnitsList.OrderBy(x => x.BusinessUnitID));
                Result.Add(BusinessUnitsList);

                var PartnertypeLists = (from partnertype in db.PartnerTypes select new { partnertype.PartnerTypeID, partnertype.DisplayName, partnertype.IsActive }).ToList();
                List<int> partnerarr = new List<int>();
                foreach (var item in user.user.partnerType)
                {
                    partnerarr.Add(item.PartnerTypeID);
                }
                PartnertypeLists.RemoveAll(x => x.IsActive == false && !partnerarr.Contains(x.PartnerTypeID));
                var PartnertypeList = PartnertypeLists.Select(x => new { x.PartnerTypeID, x.DisplayName }).ToList();
                Result.Add(PartnertypeList.OrderBy(x => x.PartnerTypeID));

                var geoLists = (from geos in db.Geos select new { geos.DisplayName, geos.GeoID, geos.IsActive }).ToList();
                List<int> geoarr = new List<int>();
                foreach (var item in user.user.GeoDetails)
                {
                    geoarr.Add(item.GeoID);
                }

                geoLists.RemoveAll(x => x.IsActive == false && !geoarr.Contains(x.GeoID));
                var geoList = geoLists.Select(x => new { x.GeoID, x.DisplayName }).ToList();
                Result.Add(geoList.OrderBy(x => x.GeoID));
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

        }



        /// <summary>
        ///  The Edit screen data population API when the user clicks for the very time
        /// </summary>
        /// <returns></returns>
        [Route("api/UserManagement/UserManagement_New_User_Dropdowns")]
        [HttpGet]
        public IHttpActionResult UserManagement_New_User_Dropdowns()
        {
            List<object> Result = new List<object>();
            var RoleList = (from role in db.Roles where role.IsActive == true select new { role.RoleID, role.DisplayName }).ToList();
            var AccessList = (from usertype in db.UserTypes orderby usertype.CreatedDate select new { usertype.UserTypeID, usertype.DisplayName }).ToList();
            var BusinessUnitsList = (from bu in db.BusinessUnits where bu.IsActive == true select new { bu.BusinessUnitID, bu.DisplayName }).ToList();
            var PartnertypeList = (from partnertype in db.PartnerTypes where partnertype.IsActive == true select new { partnertype.PartnerTypeID, partnertype.DisplayName }).ToList();
            try
            {
                if (RoleList != null)
                { Result.Add(RoleList); }
                if (AccessList != null)
                { Result.Add(AccessList); }
                if (BusinessUnitsList != null)
                { Result.Add(BusinessUnitsList); }
                if (PartnertypeList != null)
                { Result.Add(PartnertypeList); }
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
        }


        [Route("api/UserManagement/UserManagement_IfUser_IsAdmin")]
        [HttpGet]
        public IHttpActionResult UserManagement_IfUser_IsAdmin(bool isadmin)
        {
            List<object> Result = new List<object>();
            var list = new string[] { "World Wide", "Geo" };
            // var list = new string[] { EnumUserType.WW.ToString(), EnumUserType.Region.ToString(), EnumUserType.Subregion.ToString() };

            var AccessList = (from usertype in db.UserTypes orderby usertype.CreatedDate select new { usertype.UserTypeID, usertype.DisplayName }).ToList();
            try
            {
                if (isadmin == true)
                {
                    AccessList = (from usertype in db.UserTypes where list.Contains(usertype.DisplayName) orderby usertype.CreatedDate select new { usertype.UserTypeID, usertype.DisplayName }).ToList();
                }

                return Ok(AccessList);

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


        #endregion
    }
}
