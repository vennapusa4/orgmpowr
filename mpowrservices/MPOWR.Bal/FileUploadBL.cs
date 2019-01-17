
using MPOWR.Core;
using MPOWR.Dal;
using MPOWR.Model;
using System;
using System.Collections.Generic;

namespace MPOWR.AdminBL
{
   public class FileUploadBL
    {
        public string GetConfigYearInfo(int ApplicationID)
        {
            try
            {
                FileUploadDAL dal = new FileUploadDAL();
                return dal.GetConfigYearInfo(ApplicationID);
            }
            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
        }

        //public List<Regions> GetRegions(int ApplicationID)
        //{
        //    try
        //    {
        //        FileUploadDAL dal = new FileUploadDAL();
        //        return dal.GetRegions(ApplicationID);
        //    }
        //    catch (ContraException ex)
        //    {
        //        ContraLogManager.LogException(ContraConstants.MessageSeparator + ex.ToString());
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        ContraLogManager.LogException(ContraConstants.MessageSeparator + ex.ToString());
        //        throw;
        //    }
        //}

        public List<Geos> GetGeos(int regionID)
        {
            try
            {
                FileUploadDAL dal = new FileUploadDAL();
                return dal.GetGeos(regionID);
            }
            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
        }

        public List<Countries> GetCountries(int[] geoIds)
        {
            try
            {
                FileUploadDAL dal = new FileUploadDAL();
                return dal.GetCountries(geoIds);
            }
            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
        }

        //public bool GetExistingFiles(string fileName)
        //{
        //    try
        //    {
        //        FileUploadDAL dal = new FileUploadDAL();
        //        return dal.GetExistingFiles(fileName);
        //    }
        //    catch (ContraException ex)
        //    {
        //        ContraLogManager.LogException(ContraConstants.MessageSeparator + ex.ToString());
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        ContraLogManager.LogException(ContraConstants.MessageSeparator + ex.ToString());
        //        throw;
        //    }
        //}

        public List<FileUpload> CheckExistingCriteria(FileUpload fileUpload)
        {
            try
            {
                FileUploadDAL dal = new FileUploadDAL();
                return dal.CheckExistingCriteria(fileUpload);
            }
            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
        }

        public string InsertFileUploadMetadata(FileUpload fileUpload)
        {
            try
            {
                FileUploadDAL dal = new FileUploadDAL();
                return dal.InsertFileUploadMetadata(fileUpload);
            }
            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
        }

        public string DeleteSelectedFiles(List<int> fileIDs, string modifiedBy)
        {
            try
            {
                FileUploadDAL dal = new FileUploadDAL();
                return dal.DeleteSelectedFiles(fileIDs, modifiedBy);
            }
            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
        }

        public string MoveFilesToUAT(List<int> fileIDs, int statusID, string modifiedBy)
        {
            try
            {   
                FileUploadDAL dal = new FileUploadDAL();
                return dal.MoveFilesToUAT(fileIDs, statusID, modifiedBy);
            }
            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
        }

        public string MoveFilesToPROD(List<int> fileIDs, int statusID, string modifiedBy)
        {
            try
            {
                FileUploadDAL dal = new FileUploadDAL();
                return dal.MoveFilesToPROD(fileIDs, statusID, modifiedBy);
            }
            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
        }

        /*public string GetUploadSatus(FileUpload model)
        {
            try
            {
                FileUploadDAL dal = new FileUploadDAL();
                return dal.GetUploadSatus(model);
            }
            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
        }*/

        public List<FileUpload> GetUploadedFileDetails()
        {
            try
            {
                FileUploadDAL dal = new FileUploadDAL();
                return dal.GetUploadedFileDetails();
            }
            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
        }

    }
}
