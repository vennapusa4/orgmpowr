using Cache;
using Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.ApplicationServer.Caching;
using System.Configuration;
using Newtonsoft.Json;
using MPOWR.Core;
using MPOWR.Model;

namespace MPOWR.Api.Controllers
{
     public class FileChunkUploadController : ApiController
    {
        private DataCache cache;
        public FileChunkUploadController()
        {
            DataCacheFactory factory = new DataCacheFactory();
            cache = factory.GetDefaultCache();
        }   
        public string Get()
        {
            return Guid.NewGuid().ToString();
        }

        [HttpPost]
        [Route("api/FileChunkUpload/UploadFileChunks")]
        public async Task<HttpResponseMessage> UploadFileChunks()
        {
            MPOWRResponse response = new MPOWRResponse { Status = MPOWRConstants.Failed, Data = null, Message = "" };
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            try
            {
                var httpRequest = HttpContext.Current.Request;
                var filesPath = HttpContext.Current.Request.FilePath;
                // Temp storage location for File Chunks
                MultipartMemoryStreamProvider provider = new MultipartMemoryStreamProvider();
                FileChunk chunk = null;
                string Container = ConfigurationManager.AppSettings["container"];
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnString"]);
                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                // Retrieve a reference to a container.
                CloudBlobContainer container = blobClient.GetContainerReference(Container);
                // Create the container if it doesn't already exist.
                container.CreateIfNotExists();

                // Read all contents of multipart message into MultipartMemoryStreamProvider.                 
                await Request.Content.ReadAsMultipartAsync(provider);
                using (Stream fileChunkStream = await provider.Contents[0].ReadAsStreamAsync())
                {

                    //Check for not null or empty
                    if (fileChunkStream == null)
                        throw new HttpResponseException(HttpStatusCode.NotFound);

                    // Read file chunk detail
                    chunk = provider.Contents[0].Headers.GetMetaData();
                    chunk.ChunkData = fileChunkStream.ReadFully();
                    // Retrieve reference to a blob named "myblob".
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference("fileName");
                    // Upload Chunk to blob storage and store the reference in Azure Cache
                    using (MemoryStream stream = new MemoryStream(chunk.ChunkData))
                    {
                        blockBlob.PutBlock(chunk.ChunkId, stream, null);
                        SmallChunk smallChunk = new SmallChunk() { ChunkId = chunk.ChunkId, OriginalChunkId = chunk.OriginalChunkId };
                        cache.CreateRegion(chunk.fileName);
                        cache.Add(Guid.NewGuid().ToString(), smallChunk, chunk.fileName);
                    }

                    // check for last chunk, if so, then do a PubBlockList
                    // Remove all keys of that FileID from Dictionary
                    if (chunk.IsCompleted)
                    {
                        List<CacheItem> cacheItems = GetItems(chunk.fileName);
                        Dictionary<string, string> blockIds = cacheItems.Select(p => (SmallChunk)p.Item)
                                                                        .Select(p => new { p.OriginalChunkId, p.ChunkId })
                                                                        .ToDictionary(d => d.OriginalChunkId, d => d.ChunkId);

                        blockIds = blockIds.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);
                        List<string> committedBlockIDs = blockIds.Select(p => p.Value).ToList();

                        await blockBlob.PutBlockListAsync(committedBlockIDs);
                        cache.RemoveRegion(chunk.fileName);
                    }   
                }

                // Send OK Response along with saved file names to the client.                 
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

        

        private List<CacheItem> GetItems(string fileId)
        {
            List<CacheItem> items = new List<CacheItem>();
            var cacheItems = cache.GetObjectsInRegion(fileId);
            foreach (var item in cacheItems)
            {
                items.Add(item.Value as CacheItem);
            }
            return items;
        }
    }
}
