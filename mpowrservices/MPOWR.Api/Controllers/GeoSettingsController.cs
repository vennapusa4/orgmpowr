using MPOWR.Api.ViewModel;
using MPOWR.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MPOWR.Api.Controllers
{
    public class GeoSettingsController : ApiController
    {
        private MPOWREntities db = new MPOWREntities();
        [HttpGet]
        [Route("api/GeoSettings/GetGeoLevelData")]
        public IHttpActionResult GetGeoLevelData()
        {
            try
            {
                var Result = (from g in db.GeoConfigDetails
                              join geo in db.Geos on g.GeoID equals geo.GeoID
                              where geo.IsActive == true
                              select new GeoConfigViewModel
                              {
                                  ID = g.ID,
                                  GeoName = geo.ShortName,
                                  IsCountryLevel = g.IsCountryLevel,
                                  IsOverAllocation=g.IsOverAllocation
                              }).ToList();

                return Ok(Result);
            }
            catch (Exception e)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("api/GeoSettings/GetBuLevelData")]
        public IHttpActionResult GetBuLevelData()
        {
            try
            {
                var Result = (from g in db.BusinessUnits where g.IsActive == true
                              select new GeoConfigViewModel
                              {
                                  ID = g.BusinessUnitID,
                                  GeoName = g.DisplayName,
                                  IsApplicable = false,
                                  IsOverAllocation = false
                              }).ToList();

                return Ok(Result);
            }
            catch (Exception e)
            {

                throw;
            }

        }
        [HttpPost]
        [Route("api/GeoSettings/UpdateGeodata")]
        public IHttpActionResult UpdateGeodata(string user, List<GeoConfigDetails> data)
        {
            try
            {
                using (MPOWREntities db = new MPOWREntities())
                {
                    foreach (var item in data)
                    {
                        var Geo= db.GeoConfigDetails.Find(item.ID);
                        if (Geo != null)
                        {
                            Geo.IsCountryLevel = item.IsCountryLevel;
                            Geo.IsOverAllocation = item.IsOverAllocation;
                            Geo.ModifiedBy = user;
                            Geo.ModifiedDate = DateTime.Now;
                            db.SaveChanges();
                        }
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
