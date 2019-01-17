using MPOWR.Api.App_Start;
using MPOWR.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MPOWR.Bal;
using MPOWR.Model;
using MPOWR.Core;
using MPOWR.Dal;
using Newtonsoft.Json;
using System.Data.Entity.Migrations;
using MPOWR.Api.Service;

namespace MPOWR.Api.Controllers
{
    [AuthorizeUser]
    public class VersionController : ApiController
    {
        /// <summary>
        /// Version data retriving from database
        /// </summary>
        /// <returns> json list</returns>
        [HttpPost]
        [Route("api/Version/GetVersionData")]
        public IHttpActionResult GetVersionData(VersionMDF version)
        {
            versionBL bl = new versionBL();
            
            List<NewVersionAdd> Result = new List<NewVersionAdd>();
            try
            {
                Result = bl.Getversions(version);
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

            return Json(Result);
        }

        [HttpPost]
        [Route("api/Version/GetFinancialYear")]
        public List<GetFinancial> GetFinancialYear(GetFinancial Financial)
        {
            versionBL bl = new versionBL();

            List<GetFinancial> FinancialYears = new List<GetFinancial>();
            
            try
            {
                FinancialYears = bl.GetFinancialYears(Financial);
               
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

            return FinancialYears;
        }
        //[HttpPost]
        //[Route("api/Version/GetFinancialYearForSearch")]
        //public List<GetFinancial> GetFinancialYearForSearch(GetFinancial Financial)
        //{
        //    versionBL bl = new versionBL();

        //    List<GetFinancial> FinancialYears = new List<GetFinancial>();

        //    try
        //    {
        //        FinancialYears = bl.GetFinancialYears(Financial, true);

        //    }

        //    catch (MPOWRException ex)
        //    {
        //        MPOWRLogManager.LogMessage(ex.Message.ToString());
        //        MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        MPOWRLogManager.LogMessage(ex.Message.ToString());
        //        MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
        //    }

        //    return FinancialYears;
        //}


        [HttpPost]
        [Route("api/Version/CreateVersion")]
        public IHttpActionResult CreateVersion(VersionMDF VersionDetails)
        {
            try
            {
                NewVersionAdd Result = new NewVersionAdd();
                versionBL bl = new versionBL();
                Result= bl.CreateVersion(VersionDetails);
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

        [HttpPost]
        [Route("api/Version/CopyVersion")]
        public IHttpActionResult CopyVersion(CopyVersion VersionDetails)
        {
            try
            {
                NewVersionAdd Result = new NewVersionAdd();
                versionBL bl = new versionBL();
                Result = bl.CopyVersion(VersionDetails);
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

        [HttpPost]
        [Route("api/Version/DeleteVersion")]
        public IHttpActionResult DeleteVersion(VersionMDF VersionDetails)
        {
            try
            {
                versionBL bl = new versionBL();
                bl.DelVersion(VersionDetails);
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
        /// To update IsFinal option for make the version is final.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        [Route("api/Version/UpdateIsFinal")]
        [HttpPost]
        public IHttpActionResult UpdateIsFinal(VersionMDF version)
        {
            try
            {
                if (!string.IsNullOrEmpty(version.CountryOrGeoOrDistrict) && !string.IsNullOrEmpty(version.AllocationLevel) &&
                    version.FinancialYearID > 0 && version.PartnerTypeID > 0)
                {
                    using (versionBL bl = new versionBL())
                    {
                        bl.UpdateIsFinal(version);
                    }
                    return Ok();
                }
                else
                {
                    return Ok("Version parameters are missing.");
                }
            }
            catch(Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// To update the VersioName.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        [Route("api/Version/UpdateVersionName")]
        [HttpPost]
        public IHttpActionResult UpdateVersionName(VersionMDF version)
        {
            try
            {
                using (versionBL bl = new versionBL())
                {
                    bl.UpdateVersionName(version);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// To check any Geo/Country/district/BU isActive flag set to 0
        /// </summary>
        /// <param name="VersionID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Version/CheckIsActiveFlag")]
        public int CheckIsActiveFlag(int VersionID)
        {
            try
            {
                using (versionBL bl = new versionBL())
                {
                   return bl.CheckIsActiveFlag(VersionID);
                }
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return 1;
            }
        }

        /// <summary>
        /// To check any Geo/Country/district/BU isActive flag set to 0
        /// </summary>
        /// <param name="VersionID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Version/CheckIsActiveFlagByVersion")]
        public bool CheckIsActiveFlagByVersion(VersionMDF version)
        {
            try
            {
                using (versionBL bl = new versionBL())
                {
                    return bl.CheckIsActiveFlagByVersion(version);
                }
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return false;
            }
        }
    }
}
