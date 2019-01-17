using MPOWR.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPOWR.Model;
using MPOWR.Dal.Models;
using System.Data;
using System.Data.SqlClient;

namespace MPOWR.Dal
{
    public class CommonDal : ClsDispose
    {
        public dynamic GetAppConfigValue(string appConfigKey)
        {
            try
            {
                using (MPOWREntities db = new MPOWREntities())
                {
                    dynamic value;
                    value = ( from config in db.AppConfig where config.ShortName == appConfigKey select config.Params ).Single();
                    return value;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                throw new MPOWRException(message, ex);
            }
        }
    }
}
