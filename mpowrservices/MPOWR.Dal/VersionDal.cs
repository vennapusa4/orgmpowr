using MPOWR.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPOWR.Model;
using MPOWR.Dal.Models;
using System.Data.Common;
using System.Data.Entity.Migrations;
using System.Data.SqlTypes;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Data.Entity.Infrastructure;

namespace MPOWR.Dal
{
    public class VersionDal : ClsDispose
    {
        /// <summary>
        /// Gets the list of versions available in the system 
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public List<NewVersionAdd> GetVersions(VersionMDF version)
        {
            try
            {
                using (MPOWREntities db = new MPOWREntities())
                {
                    var computeList = db.AppConfig.Where(x => x.ShortName == MPOWRConstants.IsCompute).Select(d => d.Params).FirstOrDefault().ToString().Split(',');
                    bool iscompute = computeList.Contains(version.GeoID.ToString());
                    var verMDF1 = (from dat in db.MDFPlannings
                                   join finan in db.FinancialYears on dat.FinancialYearID equals finan.FinancialYearID
                                   join budgt in db.BUBudgets on dat.ID equals budgt.VersionID into group1
                                   from g1 in group1.DefaultIfEmpty()
                                   where dat.AllocationLevel == version.AllocationLevel && dat.CountryOrGeoOrDistrict == version.CountryOrGeoOrDistrict && dat.PartnerTypeID == version.PartnerTypeID && dat.MembershipGroupID == version.MembershipGroupID
                                    && finan.IsActive == true
                                   select new NewVersionAdd
                                   {
                                       VersionID = dat.ID,
                                       VersionNo = dat.VersionNo,
                                       VersionName = dat.VersionName,
                                       FinancialYearID = finan.FinancialYearID,
                                       FinancialPeriod = finan.ShortName,
                                       TotalMDF = g1.TotalMDF == null ? 0 : g1.TotalMDF,
                                       BaselineMDF = g1.BaselineMDF == null ? 0 : g1.BaselineMDF,
                                       ProgramMDFCaveOuts = g1.ProgramMDF == null ? 0 : g1.ProgramMDF,
                                       CountryReverseMDF = g1.CountryReserveMDF == null ? 0 : g1.CountryReserveMDF,
                                       BusinessUnit = g1.BusinessUnit.DisplayName,
                                       BusinessUnitID = g1.BusinessUnitID.ToString(),
                                       IsFinal = dat.IsFinal,
                                       CreatedBy = dat.CreatedBy,
                                       ModifiedBy = dat.ModifiedBy,
                                       isEnable = dat.FinancialYearID >= version.FinancialYearID,
                                       MembershipGroupID = dat.MembershipGroupID,
                                      
                                   }).ToList();


                 var   verMDF = verMDF1.GroupBy(x => new { x.VersionID, x.VersionNo, x.VersionName, x.FinancialPeriod, x.FinancialYearID, x.IsFinal, x.CreatedBy  })
                        .Select(l => new NewVersionAdd
                        {
                            VersionID = l.First().VersionID,
                            VersionNo = l.First().VersionNo,
                            VersionName = l.First().VersionName,
                            FinancialPeriod = l.First().FinancialPeriod,
                            FinancialYearID = l.First().FinancialYearID,
                            TotalMDF = l.Sum(s => s.TotalMDF == null ? 0 : s.TotalMDF),
                            BaselineMDF = l.Sum(s => s.BaselineMDF == null ? 0 : s.BaselineMDF),
                            ProgramMDFCaveOuts = l.Sum(s => s.ProgramMDFCaveOuts == null ? 0 : s.ProgramMDFCaveOuts),
                            CountryReverseMDF = l.Sum(s => s.CountryReverseMDF == null ? 0 : s.CountryReverseMDF),
                            IsFinal = l.First().IsFinal,
                            CreatedBy = l.First().CreatedBy,
                            ModifiedBy = l.First().ModifiedBy,
                            isEnable = l.First().FinancialYearID >= version.FinancialYearID,
                            MembershipGroupID = l.First().MembershipGroupID,
                            disabled = iscompute ? l.Any(x=>x.BusinessUnit == MPOWRConstants.ComputeValue || x.BusinessUnit == MPOWRConstants.ComputeVolume) : false
                        }).OrderBy(x => x.VersionID).ToList();

                    List<NewVersionAdd> versions = new List<NewVersionAdd>();
                    if (version.IsAllFinancialYear == false)
                    {
                        var versionMDFAll = verMDF.ToList();

                        for (int fID = 1; fID < version.FinancialYearID; fID++)
                        {
                            List<NewVersionAdd> ver = versionMDFAll.Where(x => x.FinancialYearID == fID).ToList();
                            if (ver.Count > 0)
                            {
                                var v = ver.Where(x => x.IsFinal == true).ToList();
                                if (v.Count == 0)
                                    versions.Add(ver.OrderByDescending(x => x.VersionNo).First());
                                else
                                    versions.Add(v.First());
                            }
                        }
                        verMDF = verMDF.Where(x => x.FinancialYearID >= version.FinancialYearID && x.FinancialYearID <= version.FinancialYearID + 1).ToList();
                        versions.AddRange(verMDF);
                    }
                    else
                    {
                        versions = verMDF.ToList();
                    }

                    var businessUnit = db.BusinessUnits.Where(x => x.IsActive == false).Select(x => x.BusinessUnitID.ToString()).ToList();
                    var intbusinessUnit = businessUnit.Select(int.Parse).ToList();
                    foreach (var item in versions)
                    {
                        var bulist = verMDF1.Where(x => x.VersionID == item.VersionID).Select(x => x.BusinessUnitID).Distinct().ToList();
                        if (bulist[0] != "")
                        {
                            var intbulist = bulist.Select(int.Parse).ToList();
                            var compare = intbulist.Except(intbusinessUnit).ToList();
                            if (compare.Count != bulist.Count)
                            {
                                item.buflag = true;
                            }
                        }
                    }

                     return versions.ToList();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                throw new MPOWRException(message, ex);
            }
        }
        /// <summary>
        /// Gets the list of financial years
        /// </summary>
        /// <param name="Financial"></param>
        /// <param name="isSearch"></param>
        /// <returns></returns>
        public List<GetFinancial> GetFinancialyear(GetFinancial Financial, bool isSearch = false)
        {
            try
            {
                using (MPOWREntities db = new MPOWREntities())
                {
                    var FinacialYear = new List<GetFinancial>();
                    int FinancialYearID = db.Database.SqlQuery<int>("select dbo.GetPlanningFinancialPeriodId()").FirstOrDefault();
                    var Year = new List<int>();
                    if (isSearch)
                    {
                        Year.Add(FinancialYearID - 1);
                    }
                    Year.Add(FinancialYearID);
                    Year.Add(FinancialYearID + 1);

                    //return (from dat in db.FinancialYears
                    //                where Year.Contains(dat.FinancialYearID)
                    //                select new GetFinancial
                    //                {
                    //                    FinancialyearID = dat.FinancialYearID,
                    //                    Financialyear = dat.ShortName,
                    //                    Version = (from ver in db.MDFPlannings
                    //                                where ver.FinancialYearID == dat.FinancialYearID && ver.CountryOrGeoOrDistrict == Financial.CountryOrGeoOrDistrict
                    //                                && ver.AllocationLevel == Financial.AllocationLevel
                    //                                && ver.PartnerTypeID == Financial.PartnerTypeID && ver.IsActive == true
                    //                                select new Versions
                    //                                {
                    //                                    VersionID = ver.ID,
                    //                                    VersionNo = ver.VersionNo,
                    //                                    VersionName = ver.VersionName
                    //                                }).OrderBy(x => x.VersionID)
                    //                }).OrderBy(x => x.FinancialyearID).ToList();
                    if (Financial.AllocationLevel == null || Financial.AllocationLevel == "")
                    {
                        FinacialYear = (from dat in db.FinancialYears
                                        where Year.Contains(dat.FinancialYearID) && dat.IsActive == true
                                        select new GetFinancial
                                        {
                                            FinancialyearID = dat.FinancialYearID,
                                            Financialyear = dat.ShortName
                                        }).OrderBy(x => x.FinancialyearID).ToList();
                    }

                    else
                    {
                        FinacialYear = (from dat in db.FinancialYears
                                        where Year.Contains(dat.FinancialYearID) && dat.IsActive == true
                                        select new GetFinancial
                                        {
                                            FinancialyearID = dat.FinancialYearID,
                                            Financialyear = dat.ShortName,
                                            Version = (from ver in db.MDFPlannings
                                                       where ver.FinancialYearID == dat.FinancialYearID && ver.CountryOrGeoOrDistrict == Financial.CountryOrGeoOrDistrict
                                                       && ver.AllocationLevel == Financial.AllocationLevel
                                                       && ver.PartnerTypeID == Financial.PartnerTypeID
                                                       && ver.Flag == true
                                                       select new Versions
                                                       {
                                                           VersionID = ver.ID,
                                                           VersionNo = ver.VersionNo,
                                                           VersionName = ver.VersionName,
                                                           IsFinal = ver.IsFinal
                                                       }).OrderBy(x => x.VersionID)
                                        }).OrderBy(x => x.FinancialyearID).ToList();
                    }

                    return FinacialYear;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                throw new MPOWRException(message, ex);
            }
        }
        /// <summary>
        /// creates a new version in database
        /// </summary>
        /// <param name="VersionDetails"></param>
        /// <returns></returns>
        public NewVersionAdd CreateVersion(VersionMDF VersionDetails)
        {
            try
            {
                using (MPOWREntities db = new MPOWREntities())
                {
                    int VersnNo;
                    int newVerNo;
                    var version = db.MDFPlannings.Where(x => x.AllocationLevel == VersionDetails.AllocationLevel &&
                    x.CountryOrGeoOrDistrict == VersionDetails.CountryOrGeoOrDistrict && x.FinancialYearID ==
                    VersionDetails.FinancialYearID && x.PartnerTypeID == VersionDetails.PartnerTypeID && x.MembershipGroupID == VersionDetails.MembershipGroupID ).AsQueryable();

                    if (!version.Any())
                    {
                        newVerNo = 0;
                    }
                    else
                    {
                          newVerNo = version.Max(x => x.VersionNo);
                    }
                    VersnNo = newVerNo + 1;

                    var VersionTable = new MDFPlanning()
                    {
                        CountryOrGeoOrDistrict = VersionDetails.CountryOrGeoOrDistrict,
                        VersionNo = VersnNo,
                        FinancialYearID = VersionDetails.FinancialYearID,
                        AllocationLevel = VersionDetails.AllocationLevel,
                        PartnerTypeID = VersionDetails.PartnerTypeID,
                        VersionName = VersionDetails.VersionName,
                        MembershipGroupID = VersionDetails.MembershipGroupID,
                        CreatedBy = VersionDetails.UserID,
                        CreatedDate = DateTime.Now,
                        ModifiedBy = VersionDetails.UserID,
                        ModifiedDate = DateTime.Now,
                        Flag = true,
                        IsFinal = false
                    };
                    db.MDFPlannings.Add(VersionTable);
                    db.SaveChanges();
                    db.Database.Connection.Close();
                    return GetVersions(VersionDetails).Last();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                throw new MPOWRException(message, ex);
            }
        }
        /// <summary>
        /// Creates a new version by copying the datafrom existing data
        /// </summary>
        /// <param name="VersionDetails"></param>
        /// <returns></returns>
        public NewVersionAdd CopyVersion(CopyVersion VersionDetails)
        {
                     int? version_id = 0;

                    var ver = new VersionMDF()
                    {
                        PartnerTypeID = VersionDetails.PartnerTypeID,
                        FinancialYearID = VersionDetails.NewFinancialyearID,
                        VersionName = VersionDetails.VersionName,
                        UserID = VersionDetails.UserID,
                        CountryOrGeoOrDistrict = VersionDetails.CountryOrGeoOrDistrict,
                        IsFinal = VersionDetails.IsFinal,
                        AllocationLevel = VersionDetails.AllocationLevel,
                        MembershipGroupID = VersionDetails.MembershipGroupID
                    };


               using (MPOWREntities db = new MPOWREntities())
                {
                         try
                        {
 
                      //  db.Database.CommandTimeout = 1000000000;
                        var result = CreateVersion(ver);

                        version_id = result.VersionID;

                        DbCommand command = db.Database.Connection.CreateCommand();
                        command.CommandText = "usp_copyBuBudget";
                        command.Parameters.Add(new SqlParameter("@OldVersionID", VersionDetails.VersionID));
                        command.Parameters.Add(new SqlParameter("@NewVersionID", result.VersionID));
                        command.Parameters.Add(new SqlParameter("@NewFinancialYearID", result.FinancialYearID));
                        command.Parameters.Add(new SqlParameter("@UserID", VersionDetails.UserID));
                        command.CommandType = CommandType.StoredProcedure;

                        if (db.Database.Connection.State == System.Data.ConnectionState.Closed)
                        {
                            db.Database.Connection.Open();
                        }
                        command.ExecuteReader();
 
                         db.Database.Connection.Close();
                        return result;
                    }
                    catch (Exception ex)
                    {
                    // Delete Create Version if copy not successfull
                        ver.VersionID = version_id;
                        DelFinancialVersion(ver);

                        string message = ex.Message;
                        throw new MPOWRException(message, ex);
                    }
             }
           
        }
        /// <summary>
        /// Deletes the final version data from database
        /// </summary>
        /// <param name="VersionDetails"></param>
        public void DelFinancialVersion(VersionMDF VersionDetails)
        {
            try
            {
                using (MPOWREntities db = new MPOWREntities())
                {
                    DbCommand command = db.Database.Connection.CreateCommand();
                    command.CommandText = "usp_DelFinancialVersion";
                    command.Parameters.Add(new SqlParameter("@V_ID", VersionDetails.VersionID));

                    command.CommandType = CommandType.StoredProcedure;

                    db.Database.Connection.Open();
                    var reader = command.ExecuteReader();

                    var result = ((IObjectContextAdapter)db).ObjectContext.Translate<dynamic>
                        (reader);

                    db.Database.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                throw new MPOWRException(message, ex);
            }
        }
        /// <summary>
        /// Sets the final version IsActive for all the versions 
        /// </summary>
        /// <param name="version"></param>
        public void UpdateIsFinal(VersionMDF version)
        {
            try
            {
                using (MPOWREntities db = new MPOWREntities())
                {
                    //Update IsFinal to false for all the versions.
                    db.MDFPlannings.Where(x => x.FinancialYearID == version.FinancialYearID &&
                    x.PartnerTypeID == version.PartnerTypeID &&
                    x.AllocationLevel == version.AllocationLevel  &&
                    x.CountryOrGeoOrDistrict == version.CountryOrGeoOrDistrict).ToList().ForEach(x =>
                    {
                        x.IsFinal = false;
                        x.ModifiedBy = version.UserID;
                        x.ModifiedDate = DateTime.Now;
                    });
                    db.SaveChanges();

                    //Update IsFinal to true for selected version.
                    var versions = db.MDFPlannings.Where(x => x.ID == version.VersionID ).FirstOrDefault();
                    versions.IsFinal = true;
                    versions.ModifiedBy = version.UserID;
                    versions.ModifiedDate = DateTime.Now;
                    db.SaveChanges();
                }
                    
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
            }
        }
        /// <summary>
        /// Get the search results based on criteria 
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public List<SearchResult> GetSearchResult(SearchCriteria version)
        {
            try
            {
                var searchresultList = new List<SearchResult>();
                using (MPOWREntities db = new MPOWREntities())
                {
                    var UserRoleUserType = (from urut in db.UserRoleUserTypes
                                    where urut.UserID == version.UserID
                                    select new { urut.UserRoleUserTypeID, urut.UserTypeID }).FirstOrDefault();
                    string AccessLevel = (from ut in db.UserTypes
                                          where ut.UserTypeID == UserRoleUserType.UserTypeID
                                          select ut.DisplayName).FirstOrDefault();
                    var Result = string.Empty;
                    DbCommand command = db.Database.Connection.CreateCommand();
                    command.CommandText = "usp_GetSearchResult";
                    command.Parameters.Add(new SqlParameter("@GEO_ID", version.GeoID));
                    command.Parameters.Add(new SqlParameter("@COUNTRY_IDs", version.CountryID));
                    command.Parameters.Add(new SqlParameter("@DISTRICT_IDs", version.DistrictID));
                    command.Parameters.Add(new SqlParameter("@PARTNER_TYPE", version.PartnerTypeID));
                    command.Parameters.Add(new SqlParameter("@FINANCIAL_ID", version.FinancialYearID));
                    command.Parameters.Add(new SqlParameter("@ACCESS_LEVEL", AccessLevel.ToUpper()));
                    command.Parameters.Add(new SqlParameter("@USERROLE_USERTYPE_ID", UserRoleUserType.UserRoleUserTypeID));
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    db.Database.Connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        object[] row = null;
                        while (reader.Read())
                        {
                            if (row == null)
                            {
                                row = new object[reader.FieldCount];
                            }
                            reader.GetValues(row);
                            var SearchResult = new SearchResult() {
                            FinancialYearID = Convert.ToInt16(row[0]),
                            FinancialYear = row[1].ToString(),
                            PartnerType = JsonConvert.DeserializeObject<List<PartnerType>>(row[2].ToString()),
                            AllocationLevel = row[3].ToString(),
                            CountryOrGeoOrDistrict = row[4].ToString(),
                            Geo = JsonConvert.DeserializeObject<List<Geo>>(row[5].ToString()),
                            Country = JsonConvert.DeserializeObject<List<Country>>(row[6].ToString()),
                            District = JsonConvert.DeserializeObject<List<District>>(row[7].ToString()),
                            Membership = JsonConvert.DeserializeObject<List<Membership>>(row[8].ToString())
                            };
                            searchresultList.Add(SearchResult);
                        }
                    }
                }
                return searchresultList;
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// Updates the name of a version 
        /// </summary>
        /// <param name="version"></param>
        public void UpdateVersionName(VersionMDF version)
        {
            try
            {
                using (MPOWREntities db = new MPOWREntities())
                {
                    db.MDFPlannings.Where(x => x.FinancialYearID == version.FinancialYearID &&
                   x.ID == version.VersionID   && 
                   x.VersionNo == version.VersionNo).ToList().ForEach(x =>
                   {
                       x.VersionName = version.VersionName;
                       x.ModifiedBy = version.UserID;
                       x.ModifiedDate = DateTime.Now;

                   });
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
            }
        }

        /// <summary>
        /// Checks the version IsActive from database
        /// </summary>
        /// <param name="">VersionID</param>
        public int CheckIsActiveFlag(int VersionID)
        {
            try
            {
                int isActive = 1;
                using (MPOWREntities db = new MPOWREntities())
                {
                    DbCommand command = db.Database.Connection.CreateCommand();
                    command.CommandText = "usp_CheckVersionIsActiveFlag";
                    command.Parameters.Add(new SqlParameter("@V_ID", VersionID));

                    command.CommandType = CommandType.StoredProcedure;

                    db.Database.Connection.Open();
                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        object[] row = null;
                        while (reader.Read())
                        {
                            if (row == null)
                            {
                                row = new object[reader.FieldCount];
                            }
                            reader.GetValues(row);
                            isActive = Convert.ToInt16(row[0]);
                        }
                    }

                    db.Database.Connection.Close();
                    return isActive;
                }
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Checks the version IsActive from database
        /// </summary>
        /// <param name = "version" ></ param >
        public bool CheckIsActiveFlagByVersion(VersionMDF version)
        {
            try
            {
                bool isActive = true;
                using (MPOWREntities db = new MPOWREntities())
                {
                    DbCommand command = db.Database.Connection.CreateCommand();
                    command.CommandText = "usp_CheckIsActiveFlagByVersion";
                    command.Parameters.Add(new SqlParameter("@FinancialYearID", version.FinancialYearID));
                    command.Parameters.Add(new SqlParameter("@PartnerTypeID", version.PartnerTypeID));
                    command.Parameters.Add(new SqlParameter("@CountryOrGeoOrDistrict", version.CountryOrGeoOrDistrict));
                    command.Parameters.Add(new SqlParameter("@AllocationLevel", version.AllocationLevel));
                    //command.Parameters.Add(new SqlParameter("@DistrictID", version.DistrictID));

                    command.CommandType = CommandType.StoredProcedure;

                    db.Database.Connection.Open();
                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        object[] row = null;
                        while (reader.Read())
                        {
                            if (row == null)
                            {
                                row = new object[reader.FieldCount];
                            }
                            reader.GetValues(row);
                            isActive = Convert.ToBoolean(row[0]);
                            
                        }
                    }

                    db.Database.Connection.Close();
                    return isActive;
                }
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return false;
            }
        }

        public List<DataLoadRefresh> GetPopup()
        {
            var data = new List<DataLoadRefresh>();
            try
            {
                using (MPOWREntities db = new MPOWREntities())
                {
                    data = db.DataLoadRefreshes.Where(x => x.IsActive == true).ToList();
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {

                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

            }
            return data;
        }



        public List<DataLoadRefresh> ConfigPopup(ConfigPopup version)
        {
            var data = new List<DataLoadRefresh>();
            try
            {
                using (MPOWREntities db = new MPOWREntities())
                {
                    data = db.DataLoadRefreshes.Where(x => x.IsActive == true).ToList();

                    foreach (var item in data)
                    {
                        var load = new DataLoadRefresh();
                        load = item;
                        load.IsPopup = version.IsPopup;
                        db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    }
                    try
                    {
                        db.SaveChanges();
                        data = db.DataLoadRefreshes.Where(x => x.IsActive == true).ToList();
                    }

                    catch (DbUpdateConcurrencyException ex)
                    {

                        MPOWRLogManager.LogMessage(ex.Message.ToString());
                        MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
            }
            return data;
        }
    }
}
