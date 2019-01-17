using MPOWR.Api.App_Start;
using MPOWR.Core;
using MPOWR.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MPOWR.Api.Controllers
{
    [AuthorizeUser]
    public class GEOController : ApiController
    {
        private MPOWREntities db = new MPOWREntities();
        private int UserTypeID;
        [Route("api/GEO/GetGeo")]
        [HttpGet]
        public IHttpActionResult GetGeo(string userid)
        {
            try
            {
                List < object > Result = new List<object>();
                UserTypeID = (from ut in db.UserTypes.AsNoTracking()
                                join urut in db.UserRoleUserTypes.AsNoTracking() on ut.UserTypeID equals urut.UserTypeID
                                where urut.UserID == userid
                                select ut.UserTypeID).FirstOrDefault();
                if (UserTypeID == (int)EnumUserType.WorldWide)
                {
                    var geoList = (from geos in db.Geos.AsNoTracking()
                                   where geos.IsActive == true
                                   select new { geos.DisplayName, geos.GeoID }).ToList();
                    Result.Add(geoList);
                }
                else
                {
                    var geoList = (from geos in db.Geos.AsNoTracking()
                                   join ugeos in db.UserGeos.AsNoTracking() on geos.GeoID equals ugeos.GeoID
                                   join urut in db.UserRoleUserTypes.AsNoTracking() on userid equals urut.UserID
                                   where ugeos.UserRoleUserTypeID == urut.UserRoleUserTypeID && geos.IsActive == true
                                   select new { geos.DisplayName, geos.GeoID }).ToList();
                    Result.Add(geoList);
                }
                return Ok(Result);
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }

        [Route("api/GEO/GetUserCountriesByGeo")]
        public IHttpActionResult GetUserCountriesByGeo(string userid, int geoid)
        {
            try
            {
                List<object> Result = new List<object>();
                UserTypeID = (from ut in db.UserTypes.AsNoTracking()
                              join urut in db.UserRoleUserTypes.AsNoTracking() on ut.UserTypeID equals urut.UserTypeID
                              where urut.UserID == userid 
                              select ut.UserTypeID).FirstOrDefault();
                //HPEM-658
                if (UserTypeID == (int)EnumUserType.WorldWide || UserTypeID == (int)EnumUserType.Geo)
                {
                    var countriesList = (from countries in db.Countries.AsNoTracking()
                                         where countries.GeoID == geoid && countries.IsActive == true
                                         select new { countries.DisplayName, countries.CountryID }).ToList();
                    Result.Add(countriesList);
                }
               
                
                else
                {
                    var countriesList = (from countries in db.Countries.AsNoTracking()
                                         join ucountries in db.UserCountries.AsNoTracking() on countries.CountryID equals ucountries.CountryID
                                         join urut in db.UserRoleUserTypes.AsNoTracking() on userid equals urut.UserID
                                         where ucountries.UserRoleUserTypeID == urut.UserRoleUserTypeID && countries.GeoID == geoid && countries.IsActive == true
                                         select new { countries.DisplayName, countries.CountryID }).ToList();
                    Result.Add(countriesList);
                }
                return Ok(Result);
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }

        [Route("api/GEO/GetAllGeo")]
        [HttpGet]
        public IHttpActionResult GetAllGeo(string userid)
        {
            try
            {
                List<object> Result = new List<object>();
                UserTypeID = (from ut in db.UserTypes.AsNoTracking()
                              join urut in db.UserRoleUserTypes.AsNoTracking() on ut.UserTypeID equals urut.UserTypeID
                              where urut.UserID == userid
                              select ut.UserTypeID).FirstOrDefault();
                if (UserTypeID == (int)EnumUserType.WorldWide)
                {
                    var geoList = (from geos in db.Geos.AsNoTracking()
                                   select new { geos.DisplayName, geos.GeoID }).ToList();
                    Result.Add(geoList);
                }
                else
                {
                    var geoList = (from geos in db.Geos.AsNoTracking()
                                   join ugeos in db.UserGeos.AsNoTracking() on geos.GeoID equals ugeos.GeoID
                                   join urut in db.UserRoleUserTypes.AsNoTracking() on userid equals urut.UserID
                                   where ugeos.UserRoleUserTypeID == urut.UserRoleUserTypeID 
                                   select new { geos.DisplayName, geos.GeoID }).ToList();
                    Result.Add(geoList);
                }
                return Ok(Result);
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }

        [Route("api/GEO/GetAllUserCountriesByGeo")]
        public IHttpActionResult GetAllUserCountriesByGeo(string userid, int geoid)
        {
            try
            {
                List<object> Result = new List<object>();
                UserTypeID = (from ut in db.UserTypes.AsNoTracking()
                              join urut in db.UserRoleUserTypes.AsNoTracking() on ut.UserTypeID equals urut.UserTypeID
                              where urut.UserID == userid
                              select ut.UserTypeID).FirstOrDefault();
                //HPEM-658
                if (UserTypeID == (int)EnumUserType.WorldWide || UserTypeID == (int)EnumUserType.Geo)
                {
                    var countriesList = (from countries in db.Countries.AsNoTracking()
                                         where countries.GeoID == geoid //&& countries.IsActive == true
                                         select new { countries.DisplayName, countries.CountryID }).ToList();
                    Result.Add(countriesList);
                }


                else
                {
                    var countriesList = (from countries in db.Countries.AsNoTracking()
                                         join ucountries in db.UserCountries.AsNoTracking() on countries.CountryID equals ucountries.CountryID
                                         join urut in db.UserRoleUserTypes.AsNoTracking() on userid equals urut.UserID
                                         where ucountries.UserRoleUserTypeID == urut.UserRoleUserTypeID && countries.GeoID == geoid //&& countries.IsActive == true
                                         select new { countries.DisplayName, countries.CountryID }).ToList();
                    Result.Add(countriesList);
                }
                return Ok(Result);
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }
    }
}
