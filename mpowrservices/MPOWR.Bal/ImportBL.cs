using MPOWR.Core;
using System;
using System.Collections.Generic;
using static MPOWR.Model.Export;
using MPOWR.Dal;
using MPOWR.Model;
using Microsoft.VisualBasic.FileIO;
using System.Data;
using System.IO;
using MPOWR.Dal.Models;

namespace MPOWR.Bal
{
    public class ImportBL
    {
        private MPOWREntities db = new MPOWREntities();
        /// <summary>
        /// Imports the data from the excel sheet
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public object Import(int versionId , DataTable dt , string bulist )

        {
            try
            {
                using (ImportDal dal = new ImportDal())
                {

                    return dal.Import( versionId , dt,  bulist);
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
        /// <summary>
        /// save the import data
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="versionId"></param>
        /// <param name="countryId"></param>
        /// <param name="partnerTypeId"></param>
        public object SaveImportData(DataTable dt, int versionId, string UserID)

        {
            try
            {
                using (ImportDal dal = new ImportDal())
                {
                  return dal.SaveImportData(dt,  versionId, UserID);
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
        /// <summary>
        /// converts csv data to data table
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>

        public object parseCSV(Stream stream, int versionId , string userBulist)
        {
            try
            {
                using (ImportDal dal = new ImportDal())
                {
                    return dal.parseCSV(stream, versionId , userBulist);
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
