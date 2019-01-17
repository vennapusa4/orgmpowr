using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPOWR.Core;
using static MPOWR.Model.Export;
using MPOWR.Dal.Models;
using System.Data.Common;
using System.Web.Script.Serialization;
using MPOWR.Dal.AdoHelper;
using System.Data;

namespace MPOWR.Dal
{
    public class ExportDAL : ClsDispose
    {
        private MPOWREntities db = new MPOWREntities();
        MPOWREntities objDBEntity = new MPOWREntities();
        public List<Export_SDFC> GetExportExcelSFDCData(int VersionID)
        {
            MPOWREntities objDBEntity = new MPOWREntities();

            List<Export_SDFC> Result = objDBEntity.Database.SqlQuery<Export_SDFC>("usp_Export_SDFC @VersionID",

                   new SqlParameter("@VersionID", VersionID)).ToList();


            return Result;
        }

        public string GetFileName(int versionId, string fileType)
        {
            MPOWREntities objDBEntity = new MPOWREntities();
            var filename = (
                          from partner in objDBEntity.PartnerTypes
                          from financial in objDBEntity.FinancialYears
                          from mdfplanning in objDBEntity.MDFPlannings
                          where partner.PartnerTypeID == mdfplanning.PartnerTypeID && financial.FinancialYearID == mdfplanning.FinancialYearID && mdfplanning.ID == versionId
                           && mdfplanning.Flag == true && financial.IsActive == true //&& partner.IsActive == true
                          select new
                          {
                              ShortName = financial.ShortName,
                              country = mdfplanning.CountryOrGeoOrDistrict,
                              FinancialYearID = mdfplanning.FinancialYearID,
                              parnerTypeId = mdfplanning.PartnerTypeID,
                              allocationType = mdfplanning.AllocationLevel,
                              planname = mdfplanning.VersionName,
                              FileName = mdfplanning.VersionName + "_" + partner.DisplayName + "_" + financial.ShortName + ".xlsx",
                              filenameLength = mdfplanning.VersionName.Length + partner.DisplayName.Length + financial.ShortName.Length
                          }).FirstOrDefault();
            var countrynames = string.Empty;

            if (filename.allocationType == "G")
            {
                int geoId = Int16.Parse(filename.country);
                countrynames = objDBEntity.Geos.Where(x=>x.GeoID == geoId).FirstOrDefault().DisplayName   + "_";
            }
            else  if (filename.allocationType == "C")
            {
                int countryId = 0;
                string[] countries = filename.country.Split(',');
                foreach (var country in countries)
                {
                    countryId = Convert.ToInt16(country);
                    countrynames = countrynames + objDBEntity.Countries.Where(x => x.CountryID == countryId).FirstOrDefault().ShortName + " _ ";
                }
            }
            else if (filename.allocationType == "D")
            {
                int geoId = Int16.Parse(filename.country);
                countrynames = MPOWRConstants.UnitedStates + "_" + objDBEntity.Districts.Where(x => x.DistrictID == geoId).FirstOrDefault().DisplayName + "_";
            }
            if (countrynames.Length > (240 - filename.filenameLength))
                return fileType + countrynames.Substring(0, (240 - filename.filenameLength)) + filename.FileName.ToString();
            else
                return fileType + countrynames + filename.FileName.ToString();
        }

        //HPEM-457 - Export to excel changes start
        /// <summary>
        /// returns the partner budget summary data
        /// </summary>
        /// <param name="countryID"></param>
        /// <param name="partnerTypeID"></param>
        /// <param name="financialYearID"></param>
        /// <param name="districtID"></param>
        /// <param name="businessUnitID"></param>
        /// <param name="versionId"></param>
        /// <returns>List<BudgetAllocationSummary></returns>
        public List<BudgetsummaryWithR1R2> GetSummaryData(int businessUnitID, int versionId)
        {
            var result = string.Empty;
            try
            {
                MPOWREntities db = new MPOWREntities();
                DbCommand command = db.Database.Connection.CreateCommand();
                command.CommandText = "usp_PartnerBudget_Graph";
                command.Parameters.Add(new SqlParameter("@BusinessUnitID", businessUnitID));
                command.Parameters.Add(new SqlParameter("@V_ID", versionId));
                command.CommandType = System.Data.CommandType.StoredProcedure;
                db.Database.Connection.Open();
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (result == string.Empty)
                        {
                            result = reader.GetValue(0).ToString();
                        }
                        else
                        {
                            result = result + reader.GetValue(0).ToString();
                        }
                    }
                }
                db.Database.Connection.Close();
                var serializer = new JavaScriptSerializer();
                serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                dynamic obj = serializer.Deserialize(result, typeof(object));
                return ParseSummaryData(obj, versionId);

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
        /// Parse the json and retuns the sumary objects
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="districtId"></param>
        /// <param name="partnerTypeID"></param>
        /// <param name="countryId"></param>
        /// <returns>List<BudgetAllocationSummary> </returns>
        private List<BudgetsummaryWithR1R2> ParseSummaryData(dynamic obj, int versionId)
        {
            var buId = 0;
            var summaryData = new List<BudgetsummaryWithR1R2>();
            foreach (var rec in obj["Graphs"][0]["AllocatedGraph"])
            {
                buId = rec.Business_Unit;

                summaryData.Add(new BudgetsummaryWithR1R2
                {

                    BusinessUnit = string.IsNullOrEmpty((from bu in objDBEntity.BusinessUnits
                                                         where bu.BusinessUnitID == buId && bu.IsActive == true
                                                         select bu.DisplayName).FirstOrDefault()) ? "Total" :
                                   (from bu in objDBEntity.BusinessUnits
                                    where bu.BusinessUnitID == buId && bu.IsActive == true
                                    select bu.DisplayName).FirstOrDefault(),
                    Round1MDF = rec.Allocated,
                    Round2MDF = rec.Additional_MDF,
                    Remaining = rec.Remaining - rec.Additional_MDF,
                    Allocated = rec.Allocated + rec.Additional_MDF,
                    Total = rec.TotalBaseline
                });

            }


            var planData = (from mdfplanning in objDBEntity.MDFPlannings
                            where mdfplanning.ID == versionId  
                            select new
                            {
                                country = mdfplanning.CountryOrGeoOrDistrict,
                                FinancialYearID = mdfplanning.FinancialYearID,
                                parnerTypeId = mdfplanning.PartnerTypeID,
                                allocationType = mdfplanning.AllocationLevel,
                            }).FirstOrDefault();
            var record = obj["Graphs"][0]["MSAGraph"][0];
            if (planData.allocationType == "D" && planData.parnerTypeId == 2)
            {
                summaryData.Add(new BudgetsummaryWithR1R2
                {

                    BusinessUnit = "MSA",
                    Allocated = obj["Graphs"][0]["MSAGraph"][0].Allocate,
                    Remaining = obj["Graphs"][0]["MSAGraph"][0].Remains,
                    Total = obj["Graphs"][0]["MSAGraph"][0].Total_MSA
                });
                summaryData.Add(new BudgetsummaryWithR1R2
                {

                    BusinessUnit = "ARUBA MSA",
                    Allocated = obj["Graphs"][0]["ARUBAMSAGraph"][0].Allocate,
                    Remaining = obj["Graphs"][0]["ARUBAMSAGraph"][0].Remains,
                    Total = obj["Graphs"][0]["ARUBAMSAGraph"][0].Total_ARUBAMSA
                });

            }
            if (planData.allocationType == "C" && planData.country.IndexOf(MPOWRConstants.CanadaCountryId) != -1 && planData.parnerTypeId == 2)

            {
                summaryData.Add(new BudgetsummaryWithR1R2
                {

                    BusinessUnit = "ARUBA MSA",
                    Allocated = obj["Graphs"][0]["ARUBAMSAGraph"][0].Allocate,
                    Remaining = obj["Graphs"][0]["ARUBAMSAGraph"][0].Remains,
                    Total = obj["Graphs"][0]["ARUBAMSAGraph"][0].Total_ARUBAMSA
                });
            }
            return summaryData;
        }


        public DataTable GetExportData(int VersionId, int partnerTypeId, string prevYear, string currentYear, string allocationlevel, string countryorGeo , string userBuList)
        {
            string sql = "[dbo].[usp_Export_PartnerBudgetcsv]";
            DataSet dsresult = MpowrDatabase.ExecDataSetProc(sql, "@VersionId", VersionId , "@userBus" , userBuList);
            DataTable result;
            if (partnerTypeId == 1)
            {
                for (int i = dsresult.Tables[0].Columns.Count - 1; i >= 0; i--)
                {

                    /* 
                    To be more precise , specify the column name you dont want to get    deleted, 
                    (you can add multilple column names here)*/

                    var strcolname = dsresult.Tables[0].Columns[i].ColumnName.ToString();

                    if (strcolname == "District")
                    {
                        dsresult.Tables[0].Columns.RemoveAt(i);
                    }
                    if (strcolname == currentYear + " M-POWR Projected Sellout")
                    {
                        dsresult.Tables[0].Columns[i].ColumnName = currentYear + " M-POWR Projected Weighted Sellout";
                    }
                }
                result = dsresult.Tables[0];

            }
            else
            {
                for (int i = dsresult.Tables[0].Columns.Count - 1; i >= 0; i--)
                {

                    /* 
                    To be more precise , specify the column name you dont want to get    deleted, 
                    (you can add multilple column names here)*/

                    var strcolname = dsresult.Tables[0].Columns[i].ColumnName.ToString();

                    if (strcolname == prevYear + " Sellout to Silver & Below" || strcolname == prevYear + " Sellout to Gold & Platinum" ||
                        strcolname == prevYear + " Unweighted Sellout")
                    {
                        dsresult.Tables[0].Columns.RemoveAt(i);
                    }
                    if (strcolname == prevYear + " Weighted Sellout ($)")
                    {
                        dsresult.Tables[0].Columns[i].ColumnName = prevYear + " Sellout ($)";
                    }
 
                }
                result = dsresult.Tables[0];
            }
            
            if ((allocationlevel == "C" && countryorGeo.IndexOf(MPOWRConstants.CanadaCountryId) != -1) ||
                (allocationlevel == "C" && countryorGeo.IndexOf(MPOWRConstants.UnitedStatesCountryId) != -1)
                || allocationlevel == "D")
            {
                string carveout = "Allocated " + currentYear + " CarveOut";
                for (int i = result.Columns.Count - 1; i >= 0; i--)
                {
                   
                    var strcolname = result.Columns[i].ColumnName.ToString();
                    //|| strcolname == "M-POWR " + currentYear + " Calculated MDF (Round 2)"
                    if (strcolname.IndexOf(ImportTemplate.Round2MDF) != -1 || (strcolname.IndexOf(carveout) != -1))
                    {
                        dsresult.Tables[0].Columns.RemoveAt(i);
                     }
                 }
            }
                return result;
            
        }
        //HPEM-457 - Export to excel changes end

        public DataTable GetBudgetReportData()
        {

            string sql = "[dbo].[usp_Export_BudgetReport]";
            DataSet dsresult = MpowrDatabase.ExecDataSetProc(sql);
            DataTable result;

            result = dsresult.Tables[0];
            return result;
        }

        public DataTable GetCarveOutReportData()
        {
            string sql = "[dbo].[usp_Export_CarveOutReport]";
            DataSet dsresult = MpowrDatabase.ExecDataSetProc(sql);
            DataTable result;

            result = dsresult.Tables[0];
            return result;
        }

        public DataTable GetBudgetAllocationDateReportData()
        {

            string sql = "[dbo].[usp_Export_BudgetAllocationReport]";
            DataSet dsresult = MpowrDatabase.ExecDataSetProc(sql);
            DataTable result;

            result = dsresult.Tables[0];
            return result;
        }
    }
}