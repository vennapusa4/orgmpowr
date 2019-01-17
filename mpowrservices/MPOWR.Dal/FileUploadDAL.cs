using MPOWR.Dal.Models;
using MPOWR.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPOWR.Dal
{
    public class FileUploadDAL
    {
        public string GetConfigYearInfo(int ApplicationID)
        {
            var Result = string.Empty;
            using (MPOWREntities db = new MPOWREntities())
            {
                try
                {
                    DbCommand command = db.Database.Connection.CreateCommand();
                    command.CommandText = "USP_GetConfigYearInfo";
                    command.Parameters.Add(new SqlParameter("@ApplicationID", ApplicationID));
                    command.CommandType = CommandType.StoredProcedure;
                    db.Database.Connection.Open();
                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (Result == string.Empty)
                            {
                                Result = reader.GetValue(0).ToString();
                            }
                            else
                            {
                                Result = Result + reader.GetValue(0).ToString();
                            }
                        }
                    }
                    reader.Close();
                    Object json;
                    if (Result == null || Result == "")
                    {
                        json = Result;
                    }
                    else { json = JObject.Parse(Result); }


                    db.Database.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return Result;
        }

        //public List<Regions> GetRegions(int ApplicationID)
        //{
        //    List<Regions> list = new List<Regions>();
        //    using (ContraAdminEntities db = new ContraAdminEntities())
        //    {
        //        try
        //        {

        //            list = (from data in db.ContraWeb_Admin_Region
        //                    where data.ApplicationId == ApplicationID && data.IsActive == true
        //                    select new Regions { RegionId = data.RegionId, RegionsName = data.Region }).ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    return list;
        //}

        public List<Geos> GetGeos(int regionID)
        {
            List<Geos> list = new List<Geos>();
            using (MPOWREntities db = new MPOWREntities())
            {
                try
                {
                         list = (from data in db.Geos
                                where data.IsActive == true
                                select new Geos { GeoId = data.GeoID, Geo = data.DisplayName }).ToList();
                 }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return list;
        }

        public List<Countries> GetCountries(int[] geoIds)
        {
            List<Countries> list = new List<Countries>();
            using (MPOWREntities db = new MPOWREntities())
            {
                try
                {

                    list = (from geo in db.Geos
                            join cntry in db.Countries on geo.GeoID equals cntry.GeoID
                            where geoIds.Contains(geo.GeoID) && geo.IsActive == true  && cntry.IsActive == true
                            select new Countries { CountryId = cntry.CountryID, Country = cntry.DisplayName }).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return list;
        }

        /*public bool GetExistingFiles(string fileName)
        {
            bool fileWithThisNameExists = false;
            using (ContraAdminEntities db = new ContraAdminEntities())
            {
                try
                {
                    List<Contra.Entities.FileUpload> list = (from files in db.ContraWeb_Upload_DataFile
                                                             where files.FileName == fileName
                                                             select new Contra.Entities.FileUpload { ID = files.ID, FileName = files.FileName }).ToList();
                    if (list.Count > 0)
                    {
                        fileWithThisNameExists = true;
                    }
                    else
                    {
                        fileWithThisNameExists = false;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return fileWithThisNameExists;
        }*/

        public List<FileUpload> CheckExistingCriteria(FileUpload fileUpload)
        {
            List<FileUpload> list = new List<FileUpload>();
            using (MPOWREntities db = new MPOWREntities())
            {
                try
                {
                    list = (from files in db.ContraWeb_Upload_DataFile
                            join status in db.ContraWeb_Upload_StatusMaster on files.ContraWeb_Upload_StatusMaster.ID equals status.ID
                            where (files.SelectedCriteria == fileUpload.SelectedCriteria) && files.IsDeleted == false
                            select new FileUpload
                            {
                                ID = files.ID,
                                FileName = files.FileName,
                                SelectedCriteria = files.SelectedCriteria,
                                Status = status.Status,
                                StatusID = status.ID
                            }).ToList();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return list;
        }

        public string InsertFileUploadMetadata(FileUpload fileUpload)
        {
            var Result = string.Empty;
            using (MPOWREntities db = new MPOWREntities())
            {
                try
                {
                    DbCommand command = db.Database.Connection.CreateCommand();
                    command.CommandText = "USP_InsertFileUploadData";
                    command.Parameters.Add(new SqlParameter("@ID", fileUpload.ID));
                    command.Parameters.Add(new SqlParameter("@ApplicationID", fileUpload.ApplicationID));
                    command.Parameters.Add(new SqlParameter("@FileName", fileUpload.FileName));
                    command.Parameters.Add(new SqlParameter("@FileNameForBlob", fileUpload.FileNameForBlob));
                    command.Parameters.Add(new SqlParameter("@ContainerName", fileUpload.ContainerName));
                    command.Parameters.Add(new SqlParameter("@FilePath", fileUpload.FilePath));
                    command.Parameters.Add(new SqlParameter("@DataLevel", fileUpload.DataLevel));
                    command.Parameters.Add(new SqlParameter("@RegionID", fileUpload.RegionID));
                    command.Parameters.Add(new SqlParameter("@StatusID", fileUpload.StatusID));
                    command.Parameters.Add(new SqlParameter("@IsDeleted", fileUpload.IsDeleted));
                    command.Parameters.Add(new SqlParameter("@UploadedBy", fileUpload.UploadedBy));
                    command.Parameters.Add(new SqlParameter("@UploadedDate", DateTime.Now));
                    command.Parameters.Add(new SqlParameter("@ModifiedBy", fileUpload.ModifiedBy));
                    command.Parameters.Add(new SqlParameter("@ModifiedDate", DateTime.Now));
                    command.Parameters.Add(new SqlParameter("@SelectedCriteria", fileUpload.SelectedCriteria));
                    command.Parameters.Add(new SqlParameter("@GeoIds", fileUpload.GeoIds));
                    command.Parameters.Add(new SqlParameter("@CountryIds", fileUpload.CountryIds));
                    command.Parameters.Add(new SqlParameter("@FinancialYearAndQuarter", fileUpload.FinancialYearAndQuarter));
                    command.CommandType = CommandType.StoredProcedure;
                    db.Database.Connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    Result = "Records Inserted Successfully";
                    db.Database.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return Result;
        }

        public string DeleteSelectedFiles(List<int> fileIDs, string modifiedBy)
        {
            string deletionStatus = string.Empty;
            using (MPOWREntities db = new MPOWREntities())
            {
                try
                {
                    db.ContraWeb_Upload_DataFile.Where(x => fileIDs.Contains(x.ID)).ToList().ForEach(r =>
                    {
                        r.IsDeleted = true;
                        r.ModifiedDate = DateTime.Now;
                        r.ModifiedBy = modifiedBy;
                    });
                    db.SaveChanges();
                    deletionStatus = "Deleted Successfully";

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return deletionStatus;
        }

        public string MoveFilesToUAT(List<int> fileIDs, int statusID, string modifiedBy)
        {
            string updateStatus = string.Empty;
            using (MPOWREntities db = new MPOWREntities())
            {
                try
                {
                    db.ContraWeb_Upload_DataFile.Where(x => fileIDs.Contains(x.ID)).ToList().ForEach(r =>
                    {
                        r.StatusID = statusID;
                        r.ModifiedDate = DateTime.Now;
                        r.ModifiedBy = modifiedBy;
                    });
                    db.SaveChanges();
                    updateStatus = "Status updated Successfully";

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return updateStatus;
        }

        public string MoveFilesToPROD(List<int> fileIDs, int statusID, string modifiedBy)
        {
            string updateStatus = string.Empty;
            using (MPOWREntities db = new MPOWREntities())
            {
                try
                {
                    db.ContraWeb_Upload_DataFile.Where(x => fileIDs.Contains(x.ID)).ToList().ForEach(r =>
                    {
                        r.StatusID = statusID;
                        r.ModifiedDate = DateTime.Now;
                        r.ModifiedBy = modifiedBy;
                    });
                    db.SaveChanges();
                    updateStatus = "Status updated Successfully";

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return updateStatus;
        }

        /*public string GetUploadSatus(FileUpload model)
        {
            var Result = string.Empty;
            using (ContraAdminEntities db = new ContraAdminEntities())
            {
                try
                {
                    DbCommand command = db.Database.Connection.CreateCommand();
                    command.CommandText = "USP_GetUploadSatus";
                    command.Parameters.Add(new SqlParameter("@ApplicationID", model.ApplicationID));
                    command.CommandType = CommandType.StoredProcedure;
                    db.Database.Connection.Open();
                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (Result == string.Empty)
                            {
                                Result = reader.GetValue(0).ToString();
                            }
                            else
                            {
                                Result = Result + reader.GetValue(0).ToString();
                            }
                        }
                    }
                    reader.Close();
                    Object json;
                    if (Result == null || Result == "")
                    {
                        json = Result;
                    }
                    else { json = JObject.Parse(Result); }


                    db.Database.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return Result;
        }*/

        public List<FileUpload> GetUploadedFileDetails()
        {
            List<FileUpload> list = new List<FileUpload>();
            using (MPOWREntities db = new MPOWREntities())
            {
                try
                {

                    list = (from data in db.ContraWeb_Upload_DataFile
                            join status in db.ContraWeb_Upload_StatusMaster on data.StatusID equals status.ID
                            where data.IsDeleted == false
                            orderby data.ModifiedDate descending
                            select new FileUpload
                            {
                                ID = data.ID,
                                FileName = data.FileName,
                                SelectedCriteria = data.SelectedCriteria,
                                ModifiedBy = data.ModifiedBy,
                                ModifiedDate = data.ModifiedDate,
                                UploadedBy = data.UploadedBy,
                                UploadedDate = data.UploadedDate,
                                StatusID = data.StatusID,
                                Status = status.Status
                            }).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return list;
        }

        /*public string GetUploadedFileDetails()
        {
            var Result = string.Empty;
            using (ContraAdminEntities db = new ContraAdminEntities())
            {
                try
                {
                    DbCommand command = db.Database.Connection.CreateCommand();
                    command.CommandText = "USP_GetUploadedFileDetails";
                    command.CommandType = CommandType.StoredProcedure;
                    db.Database.Connection.Open();
                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (Result == string.Empty)
                            {
                                Result = reader.GetValue(0).ToString();
                            }
                            else
                            {
                                Result = Result + reader.GetValue(0).ToString();
                            }
                        }
                    }
                    reader.Close();
                    Object json;
                    if (Result == null || Result == "")
                    {
                        json = Result;
                    }
                    else { json = JObject.Parse(Result); }


                    db.Database.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return Result;
        }*/

    }
}
