using DataAccessLayer;
using DoddleNow.API.Models;
using DoddleNow.API.Utility;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Connections.Amazon;
using System.IO;
using System.IO.Compression;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Text;

namespace DoddleNow.API.Controllers
{
    ///<summary>
    ///All document related functions
    ///</summary>
    [RoutePrefix("api/v1/documents")]
    public class DocumentController : ApiController
    {
        private AuthRepository _repo = null;

        ///<summary>
        /// Account related functions
        ///</summary>
        public DocumentController()
        {
            _repo = new AuthRepository();
        }


        ///<summary>
        ///Documents
        ///</summary>
        /// <remarks>Gets all documents </remarks>
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetUserDocuments()
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;
            List<Document> docs = Documents.GetAllDocuments(userId);

            return Ok(docs);
        }

        ///<summary>
        ///Create new user
        ///</summary>
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> AddDocument(Document doc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Document d = Documents.AddDocument(doc);

            return Ok(d.URL);
        }

        ///<summary>
        ///Get document with id = id 
        ///</summary>
        [Route("{documentId}")]
        [HttpGet]
        public IHttpActionResult GetDocument(Guid documentId)
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;
            return Ok(Documents.GetDocument(userId, documentId));
        }


        ///<summary>
        ///Delete document with id = id
        ///</summary>
        [Route("{documentId}")]
        [HttpDelete]
        public IHttpActionResult DeleteDocument(Guid documentId)
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;
            Documents.DeleteDocument(userId, documentId);
            return Ok();
        }


        ///<summary>
        ///Bundles
        ///</summary>
        /// <remarks>Gets all documents </remarks>
        [Route("bundles")]
        [HttpGet]
        public IHttpActionResult GetUserBundles()
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;
            List<DocumentBundle> bundles = Documents.GetBundles(userId);

            return Ok(bundles);
        }

        ///<summary>
        ///Bundles
        ///</summary>
        /// <remarks>Gets all documents </remarks>
        [Route("bundles/{bundleId}")]
        [HttpGet]
        public IHttpActionResult GetBundles(Guid bundleId)
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;
            DocumentBundle bundle = Documents.GetBundleById(userId, bundleId);

            return Ok(bundle);
        }

        ///<summary>
        ///Delete document with id = id
        ///</summary>
        [Route("bundles/{bundleId}")]
        [HttpDelete]
        public IHttpActionResult DeleteBundle(Guid bundleId)
        {
            Documents.DeleteBundle(bundleId);
            return Ok();
        }

        ///<summary>
        ///Create new user
        ///</summary>
        [Route("bundles")]
        [HttpPost]
        public async Task<IHttpActionResult> AddBundle(DocumentBundle bundle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<Models.Document> test = bundle.Bundle;
            string userId = ((ClaimsIdentity)User.Identity).Claims.ToList()[3].Value;
            string url = Documents.AddBundle(userId, bundle.Name, test);

            return Ok(url);
        }



        ///<summary>
        ///
        ///</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }

    #region Helpers

    ///<summary>
    /// Users
    ///</summary>
    public class Documents
    {
        ///<summary>
        ///Get all clients across DoddleNow
        ///</summary>
        public static List<Document> GetAllDocuments(string userId)
        {
            List<Document> docs = new List<Document>();
            DataAccess da = new DataAccess();

            //get all doc id, name, descr from database and add to list
            List<usp_GetDocumentsResult> d = da.GetDocuments(userId);

            for (int i = 0; i < d.Count; ++i)
            {
                docs.Add(new Document { Id = d[i].ID, URL = AWS.GetS3Url(d[i].BUCKET, d[i].KEY), Created = d[i].CREATED, Description = d[i].DESCRIPTION, Name = d[i].NAME, UserId = d[i].USER_ID, AWSFile = new S3File() { BucketName = d[i].BUCKET, Key = d[i].KEY } });
            }

            return docs;
        }

        /// <summary>
        /// Get Bundles
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<DocumentBundle> GetBundles(string userId)
        {
            List<DocumentBundle> bundle = new List<Models.DocumentBundle>();
            DataAccess da = new DataAccess();

            List<usp_GetBundlesResult> b = da.GetBundles(userId);
            
            for (int i = 0; i < b.Count; ++i)
            {
                string url = AWS.GetS3Url("doddle-prod", b[i].KEY);
                bundle.Add(new DocumentBundle { URL=url, Created = b[i].CREATED, Id = b[i].ID, Name = b[i].NAME, Bundle = GetBundleDetails(userId, b[i].ID) });
            }
            return bundle;
        }


        /// <summary>
        /// Get Bundle by Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bundleId"></param>
        /// <returns></returns>
        public static DocumentBundle GetBundleById(string userId, Guid bundleId)
        {

            DataAccess da = new DataAccess();

            usp_GetBundlesResult b = da.GetBundles(userId).Where(v => v.ID == bundleId).FirstOrDefault();
            string url = AWS.GetS3Url("doddle-prod", b.KEY);
            DocumentBundle bundle = new DocumentBundle { URL=url, Created = b.CREATED, Id = b.ID, Name = b.NAME, Bundle = GetBundleDetails(userId, b.ID) };

            return bundle;
        }

        /// <summary>
        /// Get bundle details
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bundleId"></param>
        /// <returns></returns>
        private static List<Document> GetBundleDetails(string userId, Guid bundleId)
        {
            List<Document> bundleDetails = new List<Models.Document>();
            DataAccess da = new DataAccess();
            List<usp_GetBundleDetailsResult> det = da.GetBundleDetails(bundleId);

            for (int i = 0; i < det.Count; ++i)
            {
                Document doc = GetDocument(userId, det[i].DOCUMENT_ID);
                bundleDetails.Add(doc);
            }

            return bundleDetails;
        }

        /// <summary>
        /// Get individual document
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public static Document GetDocument(string userId, Guid documentId)
        {
            Document doc = new Document();
            DataAccess da = new DataAccess();

            //get db info and add to object
            usp_GetDocumentByIdResult d = da.GetDocumentById(userId, documentId);
            //get aws info and add to object
            string url = AWS.GetS3Url(d.BUCKET, d.KEY);

            if (d != null)
            {
                doc = new Document { Id = d.ID, URL = url, Created = d.CREATED, Description = d.DESCRIPTION, Name = d.NAME, UserId = d.USER_ID, AWSFile=new S3File() { BucketName = d.BUCKET, Key = d.KEY } };
            }

            return doc;
        }

        /// <summary>
        /// Add document
        /// </summary>
        /// <returns></returns>
        public static Document AddDocument(Document doc)
        {
            DataAccess da = new DataAccess();

            //string json = JsonConvert.SerializeObject(doc.FileData);

            doc.FileData = doc.FileData.Replace('-', '+');
            doc.FileData = doc.FileData.Replace('_', '/');

            //string base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            byte[] decoded = Convert.FromBase64String(doc.FileData);

            usp_GetUserResult u = Users.GetUser(Guid.Parse(doc.UserId));

            doc.AWSFile = new S3File { BucketName = "doddle-prod", Key = u.ClientId.ToString() + "/" + u.Id.ToString() + "/" + doc.FileName, ContentType = GetMimeType(doc.FileName) };

            AWS.AddS3Object(doc.AWSFile, decoded);
            doc.URL = AWS.GetS3Url(doc.AWSFile.BucketName, doc.AWSFile.Key);

            //add userid, bucket, key, name, descr to db
            da.AddDocument(doc.UserId, doc.Name, doc.Description, doc.AWSFile.BucketName, doc.AWSFile.Key);

            //return document object
            return doc;
        }

        /// <summary>
        /// Add bundle
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="items"></param>
        public static string AddBundle(string userId, string name, List<Models.Document> items)
        {
            DataAccess da = new DataAccess();
            usp_GetUserResult u = Users.GetUser(Guid.Parse(userId));
            Dictionary<string, byte[]> fileList = new Dictionary<string, byte[]>();
            string key = string.Empty;
            string bucket = "doddle-prod";
            foreach (var i in items)
            {
                //download file to byte[]
                fileList.Add(i.AWSFile.Key, AWS.GetS3Bytes(i.AWSFile.BucketName, i.AWSFile.Key));
            }

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in fileList)
                    {
                        string filename = file.Key.Remove(0, file.Key.LastIndexOf("/") + 1);
                        var zip = archive.CreateEntry(filename);

                        using (var entryStream = zip.Open())
                        using (var b = new BinaryWriter(entryStream))
                        {
                            b.Write(file.Value);
                        }
                    }
                }

                key = u.ClientId.ToString() + "/" + u.Id.ToString() + "/bundles/" + name.Replace(" ", "_") + ".zip";
                S3File zipFile = new S3File { BucketName = bucket, Key = key, ContentType = "application/zip" };
                AWS.AddS3Object(zipFile, memoryStream.ToArray());

                
            }



            Guid bundleId = da.AddBundle(userId, name, key);

            for (int i = 0; i < items.Count; ++i)
            {
                da.AddBundleDetail(bundleId, items[i].Id, items[i].SortIdx);
            }

            string url = AWS.GetS3Url(bucket, key);
            return url;
        }


        ///<summary>
        ///Delete user
        ///</summary>
        public static void DeleteDocument(string userId, Guid documentId)
        {
            DataAccess da = new DataAccess();
            //get documentId, bucket, key from db

            usp_GetDocumentByIdResult doc = da.GetDocumentById(userId, documentId);
            var bucket = doc.BUCKET;
            var key = doc.KEY;

            //delete from aws 
            AWS.DeletingAnObject(bucket, key);

            //delete from db
            da.DeleteDocument(userId, documentId);
        }

        /// <summary>
        /// Delete bundle
        /// </summary>
        /// <param name="bundleId"></param>
        public static void DeleteBundle(Guid bundleId)
        {
            DataAccess da = new DataAccess();
            da.DeleteBundle(bundleId);
        }


        private static string GetMimeType(string filename)
        {
            string ext = Path.GetExtension(filename);
            string mime = "text/plain";
            switch (ext.ToLower())
            {
                case ".pdf":
                    mime = "application/pdf";
                    break;
                case ".doc":
                    mime = "application/msword";
                    break;
                case ".docx":
                    mime = "application/msword";
                    break;
                case ".txt":
                    mime = "text/plain";
                    break;
                case ".jpg":
                case ".jpeg":
                    mime = "image/jpeg";
                    break;
                case ".gif":
                    mime = "image/gif";
                    break;
                case ".png":
                    mime = "image/png";
                    break;
                case ".tif":
                case ".tiff":
                    mime = "image/tiff";
                    break;
                default:
                    mime = "text/plain";
                    break;
            }
            return mime;

        }
    }
    #endregion
}
