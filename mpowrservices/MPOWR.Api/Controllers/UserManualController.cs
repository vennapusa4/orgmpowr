using MPOWR.Api.ViewModel;
using MPOWR.Core;
using MPOWR.Dal.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
using MPOWR.Api.App_Start;

namespace MPOWR.Api.Controllers
{
    [AuthorizeUser]
    public class UserManualController : ApiController
    {
        private MPOWREntities db = new MPOWREntities();
        [HttpGet]
        [Route("api/UserManualController/GetDetails")]
        public IHttpActionResult GetDetails()
        {
           
            try
            {
                List<UserManualViewModel> Details = new List<UserManualViewModel>();
                          Details = (
                          from UserManual in db.UserManuals
                          select new UserManualViewModel
                          {
                              id = UserManual.Id,
                              type = UserManual.Type,
                              displayname = UserManual.DisplayName,
                              shortname = UserManual.ShortName
                          }).ToList();
                List<UserManualViewModel> DetailsToSend = new List<UserManualViewModel>();

                 string url = ConfigurationManager.AppSettings["blobURL"];
                //string url = ConfigurationManager.AppSettings["URL"];

                foreach (var item in Details)
                {
                    UserManualViewModel DetailsTo = new UserManualViewModel();
                    DetailsTo.id = item.id;
                    DetailsTo.type = item.type;
                    DetailsTo.displayname = item.displayname;
                    DetailsTo.shortname = item.shortname;
                    DetailsTo.url = url + item.shortname;

                    DetailsToSend.Add(DetailsTo);
                }
                return Ok(DetailsToSend);

            }
            catch (MPOWRException ex)
            {
                db.Database.Connection.Close();
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

                return null;

            }
            catch (Exception ex)
            {
                db.Database.Connection.Close();
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }
        [HttpGet]
        [Route("api/UserManualController/DeleteFile")]
        public IHttpActionResult DeleteFile(int fileID)
        {
            string Container = ConfigurationManager.AppSettings["container"];
            if (fileID == 0) return Ok("Not a valid record");
            try
            {
                  UserManual exist_file = (from files in db.UserManuals where (files.Id == fileID) select files).FirstOrDefault();
                    if (exist_file != null)
                    {
                       //delete the blob
                         CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnString"));
                         CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                         CloudBlobContainer container = blobClient.GetContainerReference(Container); 
                         if (container.Exists()) {
                            CloudBlockBlob blockBlob = container.GetBlockBlobReference(exist_file.DisplayName);
                            blockBlob.Delete(); 
                        }
                        
                        //delete the entry in db
                        db.UserManuals.Remove(exist_file);
                        db.SaveChanges();
                    } 
                return Ok();
            }
            
            catch (MPOWRException ex)
            {
                db.Database.Connection.Close();
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

                return null;

            }
            catch (Exception ex)
            {
                db.Database.Connection.Close();
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }
        [HttpPost]
        //Comment below line if you are deploying on cloud and uncomment if it is azure
         [Route("api/UserManualController/FileUpload")]
        public IHttpActionResult FileUploadAzure()
        {

            try
            {
                string Container = ConfigurationManager.AppSettings["container"];
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count < 1)
                {
                    return Ok("No File Selected");
                }
                else
                {
                    // Retrieve storage account from connection string.
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnString"));
                    // Create the blob client.
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    // Retrieve a reference to a container.
                    CloudBlobContainer container = blobClient.GetContainerReference(Container);
                    // Create the container if it doesn't already exist.
                    container.CreateIfNotExists();
                    container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });


                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];

                        // Retrieve reference to a blob by name and upload.
                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(file);

                        int Record = (from item in db.UserManuals where item.DisplayName.Trim() == file.Trim() select item.Id).FirstOrDefault();
                        if (!blockBlob.Exists() && (Record == 0 || Record == null))
                        {
                            //Upload data to stream
                            blockBlob.Properties.ContentType = postedFile.ContentType;
                            blockBlob.UploadFromStream(postedFile.InputStream);

                            //Insert a record into table
                            UserManual insertfile = new UserManual();
                            insertfile.Type = postedFile.ContentType;
                            insertfile.DisplayName = file;
                            insertfile.ShortName = file;
                            db.UserManuals.Add(insertfile);
                            db.SaveChanges();

                        }
                        else
                        {
                            return Ok("File Already Existed");
                        }
                    }
                }

                return Ok("Success");

            }
            catch (MPOWRException ex)
            {
                db.Database.Connection.Close();
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

                return null;

            }

            catch (Exception ex)
            {
                if (ex.Message == "Maximum request length exceeded")
                {
                    db.Database.Connection.Close();
                    MPOWRLogManager.LogMessage(ex.Message.ToString());
                    MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                    return Ok("maxSizeExceeded");
                }
                else
                    return null;
            }
        }
        [HttpPost]
        //Comment below line if you are deploying on azure and uncomment if it is cloud
        // [Route("api/UserManualController/FileUpload")]
        public IHttpActionResult FileUploadLocal()
        {
            string url = ConfigurationManager.AppSettings["URL"];


            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count < 1)
                {
                    return Ok("No File Selected");
                }
                else
                {
                    foreach (string file in httpRequest.Files)
                    {
                        UserManual insertfile = new UserManual();
                        var postedFile = httpRequest.Files[file];
                        insertfile.Type = postedFile.ContentType;
                        insertfile.DisplayName = postedFile.FileName;
                        insertfile.ShortName = postedFile.FileName;
                        string filename = System.IO.Path.GetFileName(insertfile.ShortName);
                        var fileexits = Directory.GetFiles(url, filename, SearchOption.AllDirectories).FirstOrDefault();
                        if (fileexits == null)
                        {
                            db.UserManuals.Add(insertfile);
                            db.SaveChanges();
                            var filePath = Path.Combine(url, postedFile.FileName);
                            postedFile.SaveAs(filePath);

                        }
                        else
                        {
                            return Ok("File Already Existed");
                        }

                    }
                }

                return Ok("Success");

            }
            catch (MPOWRException ex)
            {
                db.Database.Connection.Close();
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

                return null;

            }

            catch (Exception ex)
            {
                if (ex.Message == "Maximum request length exceeded.")
                {
                    db.Database.Connection.Close();
                    MPOWRLogManager.LogMessage(ex.Message.ToString());
                    MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                    return Ok("maxSizeExceeded");
                }
                else
                    return null;
            }
        }
    }
}
