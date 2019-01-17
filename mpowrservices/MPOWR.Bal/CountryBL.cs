
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPOWR.Model;
using MPOWR.Dal;
//using OfficeOpenXml;
//using OfficeOpenXml.Style;
using System.Drawing;
using MPOWR.Core;

namespace MPOWR.Bal
{

    public class CountryBL : ClsDispose
    { 
        public CountryViewModel GetCountries(string userid)
        {
             CountryViewModel model = null;

            try
            {
                
                using (CountryDAL dal = new CountryDAL())
                {
                    model = dal.GetCountries(userid);
                }
            }

            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }

            return model;
        }
    }
}
