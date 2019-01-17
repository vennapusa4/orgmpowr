using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using MPOWR.Core;
using static MPOWR.Model.Export;
using MPOWR.Dal.Models;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Data.OleDb;
using MPOWR.Model;
using MPOWR.Dal.AdoHelper;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace MPOWR.Dal
{
   public class ImportDal: ClsDispose
    {
        private MPOWREntities db = new MPOWREntities();
        private ExportDAL exportDAl = new ExportDAL();
        public object Import( int versionId , DataTable dtData, string userBuList)
        {
            using (SqlCommand conn = new SqlCommand())
            {
               
                var planningInfo = (from mdfplanning in db.MDFPlannings
                                where mdfplanning.ID == versionId && mdfplanning.Flag == true
                                select new {
                                    financialyearId =mdfplanning.FinancialYearID,
                                allocationLevel = mdfplanning.AllocationLevel,
                                CountryOrGeoOrDistrict = mdfplanning.CountryOrGeoOrDistrict
                                }
                                ).FirstOrDefault();
                var current_year = db.FinancialYears.Where(x => x.FinancialYearID == planningInfo.financialyearId && x.IsActive == true).Select(x => x.ShortName).FirstOrDefault();
                var q_current_year = current_year.Substring(5, 2) + current_year.Substring(2, 2);
                var IscanaraOrUSA = ((planningInfo.allocationLevel == "C" && planningInfo.CountryOrGeoOrDistrict.IndexOf(MPOWRConstants.CanadaCountryId) != -1)||
                     (planningInfo.allocationLevel == "C" && planningInfo.CountryOrGeoOrDistrict.IndexOf(MPOWRConstants.UnitedStatesCountryId) != -1) || planningInfo.allocationLevel == "D");
                var MDF1lcolmName = "allocated " + q_current_year + " proposed budget (round 1)"; 
                var MDF2lcolmName = "allocated " + q_current_year + " proposed budget (round 2)";
                var CarveoutcolmName = "Allocated " + q_current_year + " CarveOut";
                string[] columnlist = { "ARUBA MSA", q_current_year + " allocated MSA", "Country", "partner id", "Budget", "Business Unit", MDF1lcolmName, MDF2lcolmName, CarveoutcolmName };

                if (IscanaraOrUSA)
                    columnlist= columnlist.Where(w => w != MDF2lcolmName && w != CarveoutcolmName).ToArray();
                // Dictionary<string, int> colIndexes = new Dictionary<string, int>();
                try
                {
                    StringBuilder processResult = new StringBuilder();
                    var importMDF = new MDFImport();
                    string[] columnNames = dtData.Columns.Cast<DataColumn>()
                                   .Select(x => x.ColumnName.ToLower())
                                   .ToArray();
                   if(!( columnNames.Intersect(columnlist.Select(x => x.ToLower()).ToArray()).Count() == columnlist.Count()))
                        return new { invalidExcel = "Required columns are missing"};

                    DataView view = new DataView(dtData);
                    DataTable validdt;
                    if (IscanaraOrUSA)
                        validdt = view.ToTable("validdt", false, "partner id", "country", "Business Unit", "budget", MDF1lcolmName, q_current_year + " allocated MSA", "ARUBA MSA");
                    else
                     validdt = view.ToTable("validdt", false, "partner id", "country", "Business Unit", "budget", MDF1lcolmName,  q_current_year + " allocated MSA", "ARUBA MSA", MDF2lcolmName, CarveoutcolmName);

                    validdt.Columns[q_current_year + " allocated MSA"].ColumnName = "msa";
                    validdt.Columns[MDF1lcolmName].ColumnName = "MDF1";

                    if (IscanaraOrUSA)
                    {
                        validdt.Columns.Add("MDF2", typeof(decimal));
                        validdt.Columns.Add(CarveoutcolmName, typeof(decimal));
                    }
                    else
                        validdt.Columns[MDF2lcolmName].ColumnName = "MDF2";

                    validdt.Columns["Business Unit"].ColumnName = "Business_unit";
                    validdt.Columns["aruba msa"].ColumnName = "arubamsa";
                    validdt.AcceptChanges();
                    string sql = "[dbo].[usp_ImportMDF_Helper]";
                    DataSet dsresult = MpowrDatabase.ExecDataSetProc(sql, "@table", validdt, "@financialyearId",planningInfo.financialyearId, "@versionId", versionId  , "@bu" , userBuList);
                    if (dsresult.Tables[1].Rows.Count > 0)
                        return new { invalid = dsresult.Tables[1], valid = dsresult.Tables[0] , total = dtData.Rows.Count, success= dsresult.Tables[0].Rows.Count};
                    else
                            return new { total = dtData.Rows.Count , success = dsresult.Tables[0].Rows.Count , valid = dsresult.Tables[0] };
                   
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
      /// <summary>
      /// Checks if the allocated budget exceeds the actual 
      /// </summary>
      /// <param name="countryId"></param>
      /// <param name="partnerTypeId"></param>
      /// <param name="financialyearId"></param>
      /// <param name="districtId"></param>
      /// <param name="versionId"></param>
      /// <param name="dtData"></param>
      /// <returns></returns>
        private  string ValidateAllocatedBudget ( int versionId , DataTable dtData)
        {

            try
            {
                var planData = (from mdfplanning in db.MDFPlannings
                                where mdfplanning.ID == versionId && mdfplanning.Flag == true
                                select new
                                {
                                    country = mdfplanning.CountryOrGeoOrDistrict,
                                    FinancialYearID = mdfplanning.FinancialYearID,
                                    parnerTypeId = mdfplanning.PartnerTypeID,
                                    allocationType = mdfplanning.AllocationLevel,
                                }).FirstOrDefault();
                var allocationData = (from b in dtData.AsEnumerable()
                                                              group b by b.Field<string>("Business_Unit") into g
                                                              select new BudgetsummaryWithR1R2
                                                              {
                                                                  BusinessUnit = g.Key,
                                                                  Round1MDF = g.Sum(x => Convert.ToDecimal(x.Field<long?>("mdf1"))),
                                                                  Round2MDF = g.Sum(x => Convert.ToDecimal(x.Field<long?>("mdf2")))
                                                              }).ToList();

                SqlParameter parameter = new SqlParameter();

                //The parameter for the SP must be of SqlDbType.Structured 
                parameter.ParameterName = "@dt";
                parameter.SqlDbType = System.Data.SqlDbType.Structured;
                parameter.Value = dtData;
                parameter.TypeName = "dbo.mdfinput";
                db.Database.CommandTimeout = 100000000;
                var allocatedData = db.Database.SqlQuery<BudgetsummaryWithR1R2>("usp_Get_AllocatedBudget @dt,@versionId",

                    parameter, new SqlParameter("@versionId", versionId)).ToList();
                var totalpartnerBudget = (from a in allocationData
                                          join b in allocatedData on a.BusinessUnit equals b.BusinessUnit into res
                                          from result in res.DefaultIfEmpty()
                                          select new BudgetsummaryWithR1R2
                                          { BusinessUnit = a.BusinessUnit, Round1MDF = a.Round1MDF + (result?.Round1MDF ?? 0), Round2MDF = a.Round2MDF + (result?.Round2MDF ?? 0) }).ToList();
                var summary = exportDAl.GetSummaryData(0, versionId);

                var differenceCount = (from predata in summary.Where(x => x.BusinessUnit != "MSA ARUBA")
                                       join postdata in totalpartnerBudget on predata.BusinessUnit equals postdata.BusinessUnit
                                       where predata.Total <( postdata.Round1MDF +postdata.Round2MDF)
                                       select new BudgetAllocationSummary { BusinessUnit = predata.BusinessUnit }).ToList();
                string validationRes = null;
                
                    var buList = string.Join(",", differenceCount.Select(x => x.BusinessUnit));
                    if (planData.allocationType == "D" && planData.parnerTypeId == 2)
                    {
                    var allocatedMSA = Convert.ToDecimal(dtData.AsEnumerable().Sum(x => Convert.ToDecimal( x.Field<long?>("msa")))) +
                    (allocatedData.Select(x => x.Round1MDF).FirstOrDefault() ?? 0); //Where(x => x.BusinessUnit == "")

                        if (summary.Where(x => x.BusinessUnit == "MSA").FirstOrDefault().Total < allocatedMSA)
                            validationRes = ImportTemplate.MSABudgetError;
                        
                    }

                    if ((planData.allocationType == "C" && planData.country.IndexOf("26") !=-1 && planData.parnerTypeId == 2) || (planData.allocationType == "D" && planData.parnerTypeId == 2))
                    {
                        var allocatedArubaMSA = Convert.ToDecimal(dtData.AsEnumerable().Where(x => x.Field<string>("Business_Unit") == "Aruba Products").Sum(x => Convert.ToDecimal(x.Field<long?>("arubamsa")))) + 
                        (allocatedData.Where(x => x.BusinessUnit == "Aruba Products").Select(x => x.Round1MDF).FirstOrDefault()??0);

                        if (summary.Where(x => x.BusinessUnit == "ARUBA MSA").FirstOrDefault().Total < allocatedArubaMSA)
                            validationRes = validationRes+ ImportTemplate.ARUbaMSABudgetError;
                    }
                //if (differenceCount.Count > 0)
                //{
                //    return validationRes + ImportTemplate.BUBudgetError + buList;
                //}
                return validationRes;
            }
            catch (Exception ex)
                {
                    throw ex;
                }
}

      
        public object SaveImportData (DataTable dt , int versionId,string UserID)
        {
            
            var validationResult = ValidateAllocatedBudget(versionId, dt);
            if (validationResult == null)
            {
                string sql = "[dbo].[usp_Save_PartnerBudget]";
                DataSet dsresult = MpowrDatabase.ExecDataSetProc(sql, "@dt", dt, "@versionId", versionId, "@UserID", UserID);
                return new { message = string.Empty, count =dt.Rows.Count };
            }
            return new { message = validationResult, count = 0 };

        }
        public object parseCSV(Stream stream, int versionId  , string userBuList)
        {
            long iRow = 0;
            var rowno = 2;
            var parser = new TextFieldParser(stream) { TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited };
            parser.SetDelimiters(",");
            var planningInfo = (from mdfplanning in db.MDFPlannings
                                   where mdfplanning.ID == versionId && mdfplanning.Flag == true
                                select new 
                                   {
                                       financialyearId = mdfplanning.FinancialYearID,
                                       allocationLevel = mdfplanning.AllocationLevel,
                                       CountryOrGeoOrDistrict = mdfplanning.CountryOrGeoOrDistrict
                                   }).FirstOrDefault();

            var IscanaraOrUSA = ((planningInfo.allocationLevel == "C" && planningInfo.CountryOrGeoOrDistrict.IndexOf(MPOWRConstants.CanadaCountryId) != -1) ||
                    (planningInfo.allocationLevel == "C" && planningInfo.CountryOrGeoOrDistrict.IndexOf(MPOWRConstants.UnitedStatesCountryId) != -1) || planningInfo.allocationLevel == "D");
            var current_year = db.FinancialYears.Where(x => x.FinancialYearID == planningInfo.financialyearId).Select(x => x.ShortName).FirstOrDefault();
            var q_current_year = current_year.Substring(5, 2) + current_year.Substring(2, 2);
            var MDF1lcolmName = "allocated " + q_current_year + " proposed budget (round 1)";
            var MDF2lcolmName = "allocated " + q_current_year + " proposed budget (round 2)";
            var CarveoutcolmName = "Allocated " + q_current_year + " CarveOut";
            string[] columnlist = { "ARUBA MSA", q_current_year + " allocated MSA", "Country", "partner id", "Budget", "Business Unit", MDF1lcolmName, MDF2lcolmName, CarveoutcolmName };
            if (IscanaraOrUSA)
                columnlist = columnlist.Where(w => w != MDF2lcolmName).ToArray();
            Dictionary<string, int> colIndexes = new Dictionary<string, int>();
            DataTable validdt = new DataTable();
            validdt.Columns.Add(new DataColumn(ImportTemplate.partnerId, typeof(string)));
            validdt.Columns.Add(new DataColumn(ImportTemplate.country, typeof(string)));
            validdt.Columns.Add(new DataColumn(ImportTemplate.business_unit, typeof(string)));
            validdt.Columns.Add(new DataColumn(ImportTemplate.budget, typeof(string)));
            validdt.Columns.Add(new DataColumn(ImportTemplate.MFD1, typeof(decimal)));
            validdt.Columns.Add(new DataColumn(ImportTemplate.MFD2, typeof(decimal)));
            validdt.Columns.Add(new DataColumn(ImportTemplate.msa, typeof(decimal)));
            validdt.Columns.Add(new DataColumn(ImportTemplate.arubamsa, typeof(decimal)));
            validdt.Columns.Add(new DataColumn(ImportTemplate.Carveout, typeof(decimal)));
            try
            {
                while (!parser.EndOfData)
                {
                    var fields = parser.ReadFields();
                    iRow = parser.LineNumber;
                   
                    if (iRow <= 2 && iRow != -1)
                    {
                        //validation for column mis match
                        if (!(fields.Intersect(columnlist , StringComparer.OrdinalIgnoreCase).Count() == columnlist.Count()))
                              return new { invalidExcel = ImportTemplate.InvalidColumnError };

                        fields = fields.Select(x => x.ToLower()).ToArray();
                        for (var i =0;i< columnlist.Length;i++)
                        {
                            colIndexes.Add(columnlist[i], Array.IndexOf(fields, columnlist[i].ToLower()));
                        }
                    }
                    else
                    {
                        DataRow validdr = validdt.NewRow();
                        validdr[ImportTemplate.partnerId] = fields[colIndexes["partner id"]].Trim();
                        validdr[ImportTemplate.country] = fields[colIndexes["Country"]].Trim();
                        validdr[ImportTemplate.business_unit] = fields[colIndexes["Business Unit"]].Trim();
                        validdr[ImportTemplate.budget] = fields[colIndexes["Budget"]].Trim();
                        validdr[ImportTemplate.MFD1] = fields[colIndexes[MDF1lcolmName]] == "" ? Convert.ToDecimal(0) : Convert.ToDecimal( fields[colIndexes[MDF1lcolmName]].Trim());
                        if (!IscanaraOrUSA)
                            validdr[ImportTemplate.MFD2] = fields[colIndexes[MDF2lcolmName]] == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(fields[colIndexes[MDF2lcolmName]].Trim());
                        else
                            validdr[ImportTemplate.MFD2] = Convert.ToDecimal(0);
                        validdr[ImportTemplate.msa] = Convert.ToDecimal(fields[colIndexes[q_current_year + " allocated MSA"]]);
                        validdr[ImportTemplate.arubamsa] = Convert.ToDecimal(fields[colIndexes["ARUBA MSA"]]);
                        validdr[ImportTemplate.Carveout] = Convert.ToDecimal(fields[colIndexes[CarveoutcolmName]].Trim());
                        validdt.Rows.Add(validdr);
                        rowno++;
                    }

                }
                validdt.AcceptChanges();
                string sql = "[dbo].[usp_ImportMDF_Helper]";
                DataSet dsresult = MpowrDatabase.ExecDataSetProc(sql, "@table", validdt, "@financialyearId", planningInfo.financialyearId, "@versionId", versionId,"@bu", userBuList);
                if (dsresult.Tables[1].Rows.Count > 0)
                    return new { invalid = dsresult.Tables[1], valid = dsresult.Tables[0], total = validdt.Rows.Count, success = dsresult.Tables[0].Rows.Count };
                else
                    return new { total = validdt.Rows.Count, success = dsresult.Tables[0].Rows.Count, valid = dsresult.Tables[0] };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (parser != null) parser.Close();
            }


        }

    }
}
