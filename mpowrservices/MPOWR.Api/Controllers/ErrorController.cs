using MPOWR.Api.App_Start;
using MPOWR.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MPOWR.Api.Controllers
{
    public class ErrorController : ApiController
    {
        [AuthorizeUser]
        [HttpGet,HttpPost,HttpPut,HttpDelete,HttpHead,HttpOptions]
        public  IHttpActionResult NotFounds()
        {
            try
            {


                Elmah.ErrorSignal.FromCurrentContext().Raise(new HttpListenerException(404, "404 Not Found : /"));
                return NotFound();
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
            return NotFound();
        }
    }
}   
