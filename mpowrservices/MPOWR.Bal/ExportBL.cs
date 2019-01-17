using MPOWR.Core;
using System;
using System.Collections.Generic;
using static MPOWR.Model.Export;
using MPOWR.Dal;
using System.ComponentModel;
using System.Data;

namespace MPOWR.Bal
{
    public class ExportBL
    {
        /// <summary>
        /// Returns the SDFC excel report data
        /// </summary>
        /// <param name="VersionID"></param>
        /// <returns>list of rows of SDFC excel</returns>
        public List<Export_SDFC> GetExportExcelSFDCData(int VersionID)
        {
            List<Export_SDFC> listItems = new List<Export_SDFC>();

            try
            {
                using (ExportDAL dal = new ExportDAL())
                {
                    return dal.GetExportExcelSFDCData(VersionID);
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
        /// Returns the file name for excel data
        /// </summary>
        /// <param name="versionid"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public string GetFileName(int versionid, string fileType)
        {
            string fileName = "";
            try
            {
                using (ExportDAL dal = new ExportDAL())
                {
                    fileName = dal.GetFileName(versionid, fileType);
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

            return fileName;
        }

        //HPEM-457 - Export to excel changes start
        /// <summary>
        /// Returns the summery data as json string 
        /// </summary>
        /// <param name="countryID"></param>
        /// <param name="partnerTypeID"></param>
        /// <param name="financialYearID"></param>
        /// <param name="districtID"></param>
        /// <param name="businessUnitID"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public List<BudgetAllocationSummary> GetSummaryData(int businessUnitID, int versionId)
        {


            try
            {
                var summaryData = new List<BudgetAllocationSummary>();
                using (ExportDAL dal = new ExportDAL())
                {
                    var dataWithR1andR2 = dal.GetSummaryData(businessUnitID, versionId);

                    foreach (var rec in dataWithR1andR2)
                    {
                        var record = new BudgetAllocationSummary();
                        record = (BudgetAllocationSummary)CopyProperties(record, rec);
                        summaryData.Add(record);
                    }
                    return summaryData;
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
        //HPEM-457 - Export to excel changes end
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <returns></returns>
        static object CopyProperties(object dest, object src)
        {
            foreach (PropertyDescriptor item in TypeDescriptor.GetProperties(src))
            {
                if (item.Name != MPOWRConstants.Round1MDF && item.Name != MPOWRConstants.Round2MDF)
                    item.SetValue(dest, item.GetValue(src));
            }
            return dest;
        }

        public DataTable GetExportData(int VersionId , int partnerTypeId , string prevYear ,  string current_year , string allocationLevel, string countryorGeo , string userBuList)
        {
            using (ExportDAL dal = new ExportDAL())
            {
              return  dal.GetExportData(VersionId, partnerTypeId , prevYear ,current_year, allocationLevel,  countryorGeo , userBuList);
            }

        }

        public DataTable GetBudgetReportData()
        {
            using (ExportDAL dal = new ExportDAL())
            {
                return dal.GetBudgetReportData();
            }

        }
        public DataTable GetCarveOutReportData()
        {
            using (ExportDAL dal = new ExportDAL())
            {
                return dal.GetCarveOutReportData();
            }

        }
        public DataTable GetBudgetAllocationDateReportData()
        {
            using (ExportDAL dal = new ExportDAL())
            {
                return dal.GetBudgetAllocationDateReportData();
            }

        }
    }
    }
