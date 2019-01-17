using MPOWR.Api.App_Start;
using MPOWR.Api.ViewModel;
using MPOWR.Bal;
using MPOWR.Core;
using MPOWR.Dal.Models;
using MPOWR.Model;
using System;
using System.Web.Http;


namespace MPOWR.Api.Controllers
{
    [AuthorizeUser]
    public class CountriesController : ApiController
    {


        [HttpGet]
        public CountryViewModel GetCountries(string userid)
        {
            CountryBL bl = new CountryBL();
            CountryViewModel vewModel = new CountryViewModel();

            try
            {
                vewModel = bl.GetCountries(userid);
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

            return vewModel;
        }
    }

}
