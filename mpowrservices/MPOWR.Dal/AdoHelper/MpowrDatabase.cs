using System;
using System.Configuration;
using System.Data;

namespace MPOWR.Dal.AdoHelper
{
    public class MpowrDatabase
    {
       

        private static string connString =  ConfigurationManager.ConnectionStrings["MPOWRDatabase"].ConnectionString;
        public static DataSet ExecDataSet(string sql, params object[] args)
        {
            try
            {
                using (SqlAdoHelper ado = new SqlAdoHelper(connString))
                {
                    return ado.ExecDataSet(sql, args);
                }
            }
            catch (Exception err)
            {
                //logger.ErrorFormat("when execute sql:\r\n {0} \r\nErrorMsg: {1}", sql, err.ToString());
                throw;
            }

        }
        public static DataSet ExecDataSetProc(string sql, params object[] args)
        {
            try
            {
                using (SqlAdoHelper ado = new SqlAdoHelper(connString))
                {
                    return ado.ExecDataSetProc(sql, args);
                }
            }
            catch (Exception err)
            {
              //  logger.ErrorFormat("when execute sql:\r\n {0} \r\nErrorMsg: {1}", sql, err.ToString());
                throw;
            }

        }

        public static bool ExecNonQuery(string sql, params object[] args)
        {
            try
            {
                using (SqlAdoHelper ado = new SqlAdoHelper(connString))
                {
                    int effectRows = ado.ExecNonQuery(sql, args);
                    if (effectRows > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception err)
            {
               // logger.ErrorFormat("when execute sql:\r\n {0} \r\nErrorMsg: {1}", sql, err.ToString());
                throw;
            }
        }
        public static object ExecNonQueryProc(string proc, params object[] args)
        {
            try
            {
                using (SqlAdoHelper ado = new SqlAdoHelper(connString))
                {
                    return ado.ExecNonQueryProc(proc, args);
                }

            }
            catch (Exception err)
            {
                //logger.ErrorFormat("when execute procedure:\r\n {0} \r\nErrorMsg: {1}", proc, err.ToString());
                throw;
            }
        }
        public static object ExecScalar(string sql, params object[] args)
        {
            try
            {
                using (SqlAdoHelper ado = new SqlAdoHelper(connString))
                {
                    return ado.ExecScalar(sql, args);
                }

            }
            catch (Exception err)
            {
              //  logger.ErrorFormat("when execute procedure:\r\n {0} \r\nErrorMsg: {1}", sql, err.ToString());
                throw;
            }
        }
    }
}
