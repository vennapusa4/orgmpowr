using MPOWR.Api.App_Start;
using MPOWR.Bal;
using MPOWR.Core;
using MPOWR.Dal;
using MPOWR.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MPOWR.Api.Controllers
{
    [AuthorizeUser]
    public class SearchController : ApiController
    {
        [HttpPost]
        [Route("api/Search/GetSearchResult")]
        public IHttpActionResult GetSearchResult(SearchCriteria version)
        {
            try
            {
                using (versionBL bal = new versionBL())
                {
                    var searchresultList = bal.GetSearchResult(version);
                    return Ok(searchresultList);
                }
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }

        [HttpGet]
        [Route("api/Search/GetPopup")]
        public IHttpActionResult GetPopup()
        {
            try
            {
                using (versionBL bal = new versionBL())
                {
                    var searchresultList = bal.GetPopup();
                    return Ok(searchresultList);
                }
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }

        [HttpPost]
        [Route("api/Search/SetPopup")]
        public IHttpActionResult SetPopup(ConfigPopup config)
        {
            try
            {
                using (versionBL bal = new versionBL())
                {
                    var searchresultList = bal.SetPopup(config);
                    return Ok(searchresultList);
                }
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
