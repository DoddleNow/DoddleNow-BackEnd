using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;


namespace Connections.Amazon
{
    public class S3File
    {
        /// <summary>
        /// IAM User Profile Name
        /// </summary>
        public string AWSProfileName { get; set; }
        /// <summary>
        /// Base bucket name
        /// </summary>
        public string BucketName { get; set; }
        /// <summary>
        /// API Secret Key for this Profile
        /// </summary>
        public string SecretKey{ get; set; }
        /// <summary>
        /// Unique key for file.  Include folderesque name to index on top of the bucketname.
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Full file path, including file and extension
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// Mime Type of file
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// Any metadata as key/value pair.  Initialize as "new Dictionary<String, String>()"
        /// </summary>
        public List<KeyValuePair<String, String>> Metadata {get; set;}
            
    }

    public class AWS
    {
       
        public static string AddS3Object(S3File s3File)
        {
            string tag = string.Empty;

            try
            {
                if (checkRequiredFields())
                {
                    NameValueCollection appConfig = ConfigurationManager.AppSettings;
                    string region = appConfig["AWSRegion"];
                    string accessKey = appConfig["AWSAccessKey"];
                    string secretKey = appConfig["AWSSecretKey"];
                    var credentials = new BasicAWSCredentials(accessKey, secretKey);
                    var s3Client = new AmazonS3Client(credentials, RegionEndpoint.USWest1);

                    using (s3Client)
                    {
                        PutObjectRequest request = new PutObjectRequest()
                        {
                            BucketName = s3File.BucketName,
                            Key = s3File.Key,
                            FilePath = s3File.FilePath,
                            ContentType = s3File.ContentType,
                            ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256,
                            StorageClass = S3StorageClass.Standard
                        };

                        if (s3File.Metadata != null)
                        {
                            foreach (KeyValuePair<string, string> element in s3File.Metadata)
                            {
                                request.Metadata.Add(element.Key, element.Value);
                            }
                        }

                        PutObjectResponse response = s3Client.PutObject(request);

                        tag = response.ETag;
                    }
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    throw new Exception("Please check the provided AWS Credentials.");
                }
                else
                {
                    throw new Exception(string.Format("An error occurred with the message '{0}' when writing an object", amazonS3Exception.Message));
                }
            }

            return tag;
        }

        public static void AddS3Object(S3File s3File, byte[] fileData)
        {
            try
            {
                if (checkRequiredFields())
                {
                    NameValueCollection appConfig = ConfigurationManager.AppSettings;
                    string region = appConfig["AWSRegion"];
                    string accessKey = appConfig["AWSAccessKey"];
                    string secretKey = appConfig["AWSSecretKey"];
                    var credentials = new BasicAWSCredentials(accessKey, secretKey);
                    var s3Client = new AmazonS3Client(credentials, RegionEndpoint.USWest1);

                    //convert to memory stream
                    MemoryStream stream = new MemoryStream();
                    stream.Write(fileData, 0, fileData.Length);

                    using (s3Client)
                    {
                        PutObjectRequest request = new PutObjectRequest()
                        {
                            BucketName = s3File.BucketName,
                            Key = s3File.Key,
                            InputStream = stream,
                            ContentType = s3File.ContentType,
                            ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256,
                            StorageClass = S3StorageClass.Standard
                        };

                        if (s3File.Metadata != null)
                        {
                            foreach (KeyValuePair<string, string> element in s3File.Metadata)
                            {
                                request.Metadata.Add(element.Key, element.Value);
                            }
                        }

                        PutObjectResponse response = s3Client.PutObject(request);
                    }
                }

            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    throw new Exception("Please check the provided AWS Credentials.");
                }
                else
                {
                    throw new Exception(string.Format("An error occurred with the message '{0}' when writing an object", amazonS3Exception.Message));
                }
            }
        }


        public static byte[] GetS3Bytes(string bucketName, string key)
        {
            NameValueCollection appConfig = ConfigurationManager.AppSettings;
            string region = appConfig["AWSRegion"];
            string accessKey = appConfig["AWSAccessKey"];
            string secretKey = appConfig["AWSSecretKey"];
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var s3Client = new AmazonS3Client(credentials, RegionEndpoint.USWest1);

            MemoryStream rs = null;
            GetObjectRequest getObjectRequest = new GetObjectRequest();
            getObjectRequest.BucketName = bucketName;
            getObjectRequest.Key = key;
            using (var getObjectResponse = s3Client.GetObject(getObjectRequest))
            {
                getObjectResponse.ResponseStream.CopyTo(rs);
            }
            return rs.ToArray();
        }



        public static string GetS3Object(string bucketName, string key)
        {
            try
            {
                //GetObjectRequest request = new GetObjectRequest()
                //{
                //    BucketName = bucketName,
                //    Key = key
                //};
                GetPreSignedUrlRequest request = new GetPreSignedUrlRequest()
                {
                    BucketName = bucketName,
                    Key = key,
                    Expires = DateTime.Now.AddDays(1)
                };

                NameValueCollection appConfig = ConfigurationManager.AppSettings;
                string region = appConfig["AWSRegion"];
                string accessKey = appConfig["AWSAccessKey"];
                string secretKey = appConfig["AWSSecretKey"];
                var credentials = new BasicAWSCredentials(accessKey, secretKey);
                var s3Client = new AmazonS3Client(credentials, RegionEndpoint.USWest1);

                string url = s3Client.GetPreSignedURL(request);
               
                return url;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    throw new Exception("Please check the provided AWS Credentials.");
                }
                else
                {
                    throw new Exception(string.Format("An error occurred with the message '{0}' when reading an object", amazonS3Exception.Message));
                }
            }
        }

        public static string GetUnexpiringS3Object(string bucketName, string key)
        {
            try
            {
                //GetObjectRequest request = new GetObjectRequest()
                //{
                //    BucketName = bucketName,
                //    Key = key
                //};
                GetPreSignedUrlRequest request = new GetPreSignedUrlRequest()
                {
                    BucketName = bucketName,
                    Key = key,
                    Expires = DateTime.Now.AddYears(20)
                };

                NameValueCollection appConfig = ConfigurationManager.AppSettings;
                string region = appConfig["AWSRegion"];
                string accessKey = appConfig["AWSAccessKey"];
                string secretKey = appConfig["AWSSecretKey"];
                var credentials = new BasicAWSCredentials(accessKey, secretKey);
                var s3Client = new AmazonS3Client(credentials, RegionEndpoint.USWest1);

                string url = s3Client.GetPreSignedURL(request);

                return url;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    throw new Exception("Please check the provided AWS Credentials.");
                }
                else
                {
                    throw new Exception(string.Format("An error occurred with the message '{0}' when reading an object", amazonS3Exception.Message));
                }
            }
        }

        public static void DeletingAnObject(string bucketName, string key)
        {
            try
            {
                DeleteObjectRequest request = new DeleteObjectRequest()
                {
                    BucketName = bucketName,
                    Key = key
                };

                NameValueCollection appConfig = ConfigurationManager.AppSettings;
                string region = appConfig["AWSRegion"];
                string accessKey = appConfig["AWSAccessKey"];
                string secretKey = appConfig["AWSSecretKey"];
                var credentials = new BasicAWSCredentials(accessKey, secretKey);
                var s3Client = new AmazonS3Client(credentials, RegionEndpoint.USWest1);

                using (s3Client)
                {
                    s3Client.DeleteObject(request);
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    throw new Exception("Please check the provided AWS Credentials.");
                }
                else
                {
                    throw new Exception(string.Format("An error occurred with the message '{0}' when deleting an object", amazonS3Exception.Message));
                }
            }
        }

        private static bool checkRequiredFields()
        {
            NameValueCollection appConfig = ConfigurationManager.AppSettings;

            if (string.IsNullOrEmpty(appConfig["AWSRegion"]))
            {
                throw new Exception("AWSRegion was not set in the App.config file.");
            }
            return true;
        }
    }

}
