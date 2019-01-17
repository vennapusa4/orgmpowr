using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using Newtonsoft.Json;
using MPOWR.Core;
using MPOWR.AdminBL;
using MPOWR.Model;
using MPOWR.Dal.Models;

namespace MPOWR.Api.Controllers
{
     public class FileUploadController : ApiController
    {
        /// <summary>
        ///Get Year & Quarter Informations
        /// </summary>
        /// <param name="ApplicationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/FileUpload/GetConfigYearInfo")]
        public HttpResponseMessage GetConfigYearInfo(int ApplicationId)
        {
            MPOWRResponse response = new MPOWRResponse { Status = MPOWRConstants.Failed, Data = null, Message = "" };
            FileUploadBL GeoBL = new FileUploadBL();
            try
            {
                response.Data = GeoBL.GetConfigYearInfo(ApplicationId);
                response.Status = MPOWRConstants.Success;
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, response);
            }
            catch (MPOWRException MPOWRException)
            {
                response.Message = MPOWRException.Message;
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, response);
            }
            catch
            {
                response.Message = MPOWRMessages.GENERIC_API_EXCEPTION;
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, response);
            }
        }

        //[HttpGet]
        //[Route("api/FileUpload/GetConfigYearInfo")]
        //public HttpResponseMessage GetRegions(int ApplicationID)
        //{
        //    MPOWRResponse response = new MPOWRResponse { Status = MPOWRConstants.Failed, Data = null, Message = "" };
        //    FileUploadBL GeoBL = new FileUploadBL();
        //    try
        //    {
        //        response.Data = GeoBL.GetRegions(ApplicationID);
        //        response.Status = MPOWRConstants.Success;
        //        return Request.CreateResponse(System.Net.HttpStatusCode.OK, response);
        //    }
        //    catch (MPOWRException MPOWRException)
        //    {
        //        response.Message = MPOWRException.Message;
        //        return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, response);
        //    }
        //    catch
        //    {
        //        response.Message = MPOWRMessages.GENERIC_API_EXCEPTION;
        //        return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, response);
        //    }
        //}

        [HttpGet]
        [Route("api/FileUpload/GetUploadedFileDetails")]
        public HttpResponseMessage GetUploadedFileDetails()
        {
            MPOWRResponse response = new MPOWRResponse { Status = MPOWRConstants.Failed, Data = null, Message = "" };
            FileUploadBL GeoBL = new FileUploadBL();
            try
            {
                response.Data = GeoBL.GetUploadedFileDetails();
                response.Status = MPOWRConstants.Success;
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, response);
            }
            catch (MPOWRException MPOWRException)
            {
                response.Message = MPOWRException.Message;
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, response);
            }
            catch
            {
                response.Message = MPOWRMessages.GENERIC_API_EXCEPTION;
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpGet]
        [Route("api/FileUpload/GetGeos")]
        public HttpResponseMessage GetGeos(int regionID)
        {
            MPOWRResponse response = new MPOWRResponse { Status = MPOWRConstants.Failed, Data = null, Message = "" };
            FileUploadBL GeoBL = new FileUploadBL();
            try
            {
                response.Data = GeoBL.GetGeos(regionID);
                response.Status = MPOWRConstants.Success;
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, response);
            }
            catch (MPOWRException MPOWRException)
            {
                response.Message = MPOWRException.Message;
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, response);
            }
            catch
            {
                response.Message = MPOWRMessages.GENERIC_API_EXCEPTION;
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpPost]
        [Route("api/FileUpload/GetCountries")]
        public HttpResponseMessage GetCountries([FromBody] int[] geoIds)
        {
            MPOWRResponse response = new MPOWRResponse { Status = MPOWRConstants.Failed, Data = null, Message = "" };
            FileUploadBL GeoBL = new FileUploadBL();
            try
            {
                response.Data = GeoBL.GetCountries(geoIds);
                response.Status = MPOWRConstants.Success;
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, response);
            }
            catch (MPOWRException MPOWRException)
            {
                response.Message = MPOWRException.Message;
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, response);
            }
            catch
            {
                response.Message = MPOWRMessages.GENERIC_API_EXCEPTION;
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpPost]
        [Route("api/FileUpload/InsertFileUploadMetadata")]
        public HttpResponseMessage InsertFileUploadMetadata([FromBody]  FileUpload fileUpload)
        {
            MPOWRResponse response = new MPOWRResponse { Status = MPOWRConstants.Failed, Data = null, Message = "" };
            FileUploadBL GeoBL = new FileUploadBL();
            try
            {
                response.Data = GeoBL.InsertFileUploadMetadata(fileUpload);
                response.Status = MPOWRConstants.Success;
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, response);
            }
            catch (MPOWRException MPOWRException)
            {
                response.Message = MPOWRException.Message;
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, response);
            }
            catch
            {
                response.Message = MPOWRMessages.GENERIC_API_EXCEPTION;
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpPost]
        [Route("api/FileUpload/DeleteSelectedFiles")]
        public HttpResponseMessage DeleteSelectedFiles([FromBody] object[] fileIDsList)
        {
            List<int> fileIDs = JsonConvert.DeserializeObject<List<int>>(fileIDsList[0].ToString());
            string modifiedBy = Convert.ToString(fileIDsList[1]);
            MPOWRResponse response = new MPOWRResponse { Status = MPOWRConstants.Failed, Data = null, Message = "" };
            FileUploadBL GeoBL = new FileUploadBL();
            try
            {
                response.Data = GeoBL.DeleteSelectedFiles(fileIDs, modifiedBy);
                response.Status = MPOWRConstants.Success;
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, response);
            }
            catch (MPOWRException MPOWRException)
            {
                response.Message = MPOWRException.Message;
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, response);
            }
            catch
            {
                response.Message = MPOWRMessages.GENERIC_API_EXCEPTION;
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpPost]
        [Route("api/FileUpload/MoveFilesToUAT")]
        public HttpResponseMessage MoveFilesToUAT([FromBody] object[] fileIDsList)
        {
            List<int> fileIDs = JsonConvert.DeserializeObject<List<int>>(fileIDsList[0].ToString());
            int statusID = Convert.ToInt32(fileIDsList[1]);
            string modifiedBy = Convert.ToString(fileIDsList[2]);
            MPOWRResponse response = new MPOWRResponse { Status = MPOWRConstants.Failed, Data = null, Message = "" };
            FileUploadBL GeoBL = new FileUploadBL();
            try
            {
                response.Data = GeoBL.MoveFilesToUAT(fileIDs, statusID, modifiedBy);
                response.Status = MPOWRConstants.Success;
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, response);
            }
            catch (MPOWRException MPOWRException)
            {
                response.Message = MPOWRException.Message;
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, response);
            }
            catch
            {
                response.Message = MPOWRMessages.GENERIC_API_EXCEPTION;
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpPost]
        [Route("api/FileUpload/MoveFilesToPROD")]
        public HttpResponseMessage MoveFilesToPROD([FromBody] object[] fileIDsList)
        {
            List<int> fileIDs = JsonConvert.DeserializeObject<List<int>>(fileIDsList[0].ToString());
            int statusID = Convert.ToInt32(fileIDsList[1]);
            string modifiedBy = Convert.ToString(fileIDsList[2]);
            MPOWRResponse response = new MPOWRResponse { Status = MPOWRConstants.Failed, Data = null, Message = "" };
            FileUploadBL GeoBL = new FileUploadBL();
            try
            {
                response.Data = GeoBL.MoveFilesToPROD(fileIDs, statusID, modifiedBy);
                response.Status = MPOWRConstants.Success;
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, response);
            }
            catch (MPOWRException MPOWRException)
            {
                response.Message = MPOWRException.Message;
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, response);
            }
            catch
            {
                response.Message = MPOWRMessages.GENERIC_API_EXCEPTION;
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, response);
            }
        }

        /*[HttpGet]
        [Route(RouteConstants.GetExistingFiles)]
        public HttpResponseMessage GetExistingFiles(string fileName)
        {
            MPOWRResponse response = new MPOWRResponse { Status = MPOWRConstants.Failed, Data = null, Message = "" };
            FileUploadBL GeoBL = new FileUploadBL();
            try
            {
                response.Data = GeoBL.GetExistingFiles(fileName);
                response.Status = MPOWRConstants.Success;
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, response);
            }
            catch (MPOWRException MPOWRException)
            {
                response.Message = MPOWRException.Message;
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, response);
            }
            catch
            {
                response.Message = MPOWRMessages.GENERIC_API_EXCEPTION;
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, response);
            }
        }*/

        [HttpPost]
        [Route("api/FileUpload/CheckExistingCriteria")]
        public HttpResponseMessage CheckExistingCriteria([FromBody] FileUpload fileUpload)
        {
            MPOWRResponse response = new MPOWRResponse { Status = MPOWRConstants.Failed, Data = null, Message = "" };
            FileUploadBL GeoBL = new FileUploadBL();
            try
            {
                response.Data = GeoBL.CheckExistingCriteria(fileUpload);
                response.Status = MPOWRConstants.Success;
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, response);
            }
            catch (MPOWRException MPOWRException)
            {
                response.Message = MPOWRException.Message;
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, response);
            }
            catch
            {
                response.Message = MPOWRMessages.GENERIC_API_EXCEPTION;
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, response);
            }
        }


        [HttpPost]
        [Route("api/FileUpload/UploadFile")]
        public HttpResponseMessage UploadFile()
        {
            MPOWRResponse response = new MPOWRResponse { Status = MPOWRConstants.Failed, Data = null, Message = "" };
            var error = string.Empty;
            var httpRequest = HttpContext.Current.Request;
            string Container = string.Empty;
            try
            {
                Container = ConfigurationManager.AppSettings["uploadcontainer"];
                var name = httpRequest.Form["name"];
                var index = int.Parse(httpRequest.Form["index"]);
                var file = httpRequest.Files[0];
                var id = Convert.ToBase64String(BitConverter.GetBytes(index));
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnString"]);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(Container);
                container.CreateIfNotExists();
                var blob = container.GetBlockBlobReference(name);
                blob.PutBlock(id, file.InputStream, null);
                response.Status = MPOWRConstants.Success;
                response.Message = name;
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, response);
            }
            catch (MPOWRException MPOWRException)
            {
                response.Message = MPOWRException.Message;
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, response);
            }
            catch
            {
                response.Message = MPOWRMessages.GENERIC_API_EXCEPTION;
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, response);
            }
        }
        [HttpPost]
        [Route("api/FileUpload/CommitFile")]
        public HttpResponseMessage CommitFile()
        {
            MPOWRResponse response = new MPOWRResponse { Status = MPOWRConstants.Failed, Data = null, Message = "" };
            var error = string.Empty;
            var httpRequest = HttpContext.Current.Request;
            string Container = string.Empty;
            try
            {
                Container = ConfigurationManager.AppSettings["uploadcontainer"];
                var name = httpRequest.Form["name"];
                var list = httpRequest.Form["list"];
                var ids = list.Split(',').Where(id => !string.IsNullOrWhiteSpace(id))
                    .Select(id => Convert.ToBase64String(BitConverter.GetBytes(int.Parse(id))))
                    .ToArray();
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnString"]);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(Container);
                var blob = container.GetBlockBlobReference(name);
                blob.PutBlockList(ids);
                response.Status = MPOWRConstants.Success;
                response.Message = name;
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (MPOWRException MPOWRException)
            {
                response.Message = MPOWRException.Message;
                return Request.CreateResponse(HttpStatusCode.NotFound, response);
            }
            catch
            {
                response.Message = MPOWRMessages.GENERIC_API_EXCEPTION;
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }

        //[HttpPost]
        //[Route(RouteConstants.UploadFile)]
        //public HttpResponseMessage UploadFile()
        //{
        //    MPOWRResponse response = new MPOWRResponse { Status = MPOWRConstants.Failed, Data = null, Message = "" };
        //    var fileName = string.Empty;
        //    FileUploadBL GeoBL = new FileUploadBL();
        //    try
        //    {
        //        var httpRequest = HttpContext.Current.Request;
        //        var filesPath = HttpContext.Current.Request.FilePath;
        //        string Container = string.Empty;
        //        if (httpRequest.Files.Count > 0)
        //        {
        //            foreach (string file in httpRequest.Files)
        //            {
        //                DateTime dateNow = DateTime.Now;
        //                var postedFile = httpRequest.Files[file];
        //                var text = dateNow.Month.ToString() + dateNow.Day.ToString() + dateNow.Year.ToString() + dateNow.Hour.ToString() + dateNow.Minute.ToString() + dateNow.Second.ToString();
        //                fileName = postedFile.FileName.Replace(".zip", (text + ".zip"));
        //                Container = ConfigurationManager.AppSettings["container"];
        //                string BlobStoragePath = ConfigurationManager.AppSettings["BlobStoragePath"] + fileName;

        //                // Retrieve storage account from connection string.

        //                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnString"]);
        //                // Create the blob client.
        //                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

        //                // Retrieve a reference to a container.
        //                CloudBlobContainer container = blobClient.GetContainerReference(Container);

        //                // Create the container if it doesn't already exist.
        //                container.CreateIfNotExists();

        //                // Retrieve reference to a blob named "myblob".
        //                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

        //                using (var fileStream = postedFile.InputStream)
        //                {
        //                    blockBlob.UploadFromStream(fileStream);
        //                }
        //            }
        //        }                
        //        response.Status = MPOWRConstants.Success;
        //        response.Message = fileName;
        //        return Request.CreateResponse(System.Net.HttpStatusCode.OK, response);
        //    }
        //    catch (MPOWRException MPOWRException)
        //    {
        //        response.Message = MPOWRException.Message;
        //        return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, response);
        //    }
        //    catch
        //    {
        //        response.Message = MPOWRMessages.GENERIC_API_EXCEPTION;
        //        return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, response);
        //    }
        //}
        [HttpGet]

        [Route("api/FileUpload/GetNewFileName")]

        public HttpResponseMessage GetNewFileName(string inputFileName)
        {
            MPOWRResponse response = new MPOWRResponse { Status = MPOWRConstants.Failed, Data = null, Message = "" };
            var error = string.Empty;
            var fileName = string.Empty;
            var httpRequest = HttpContext.Current.Request;

            try
            {
                DateTime dateNow = DateTime.Now;
                var text = "_" + dateNow.Month.ToString() + dateNow.Day.ToString() + dateNow.Year.ToString() + dateNow.Hour.ToString() + dateNow.Minute.ToString() + dateNow.Second.ToString();
                var fileNameSplit = inputFileName.Split('.');
                if(fileNameSplit.Length >= 2)
                {
                    fileNameSplit[fileNameSplit.Length - 2] = fileNameSplit[fileNameSplit.Length - 2] + text;
                }
                
                for(int i=0; i<= fileNameSplit.Length-2; i++)
                {
                    fileName += fileNameSplit[i] + ".";
                }

                fileName += fileNameSplit[fileNameSplit.Length-1];
                response.Status = MPOWRConstants.Success;
                response.Data = fileName;
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (MPOWRException MPOWRException)
            {
                response.Message = MPOWRException.Message;
                return Request.CreateResponse(HttpStatusCode.NotFound, response);
            }
            catch
            {
                response.Message = MPOWRMessages.GENERIC_API_EXCEPTION;
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpGet]

        [Route("api/FileUpload/GetUATStatus")]

        public HttpResponseMessage GetUATStatus()
        {
            MPOWRResponse response = new MPOWRResponse { Status = MPOWRConstants.Failed, Data = null, Message = "" };
            var error = string.Empty;
            var fileName = string.Empty;
            var httpRequest = HttpContext.Current.Request;

            try
            {
                using (MPOWREntities db = new MPOWREntities())
                {

                    var count = (from f in db.ContraWeb_Upload_DataFile
                                 join s in db.ContraWeb_Upload_StatusMaster on f.StatusID equals s.ID
                                 select f).ToList().Count;

                    response.Status = MPOWRConstants.Success;
                    response.Data = count == 0 ?false: true;
                    return Request.CreateResponse(System.Net.HttpStatusCode.OK, response);
                }
            }
            catch (MPOWRException MPOWRException)
            {
                response.Message = MPOWRException.Message;
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, response);
            }
            catch
            {
                response.Message = MPOWRMessages.GENERIC_API_EXCEPTION;
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, response);
            }
        }
    }
}
