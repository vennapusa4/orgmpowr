using MPOWR.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPOWR.Dal;
using MPOWR.Dal.Models;

namespace MPOWR.Bal
{
    public class CommonBL : ClsDispose
    {
        public dynamic GetAppConfigValue(string appConfigKey)
        {
            try
            {
                using (CommonDal dal = new CommonDal())
                {

                    return dal.GetAppConfigValue(appConfigKey);
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
        }
    }
}
