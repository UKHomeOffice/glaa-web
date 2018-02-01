using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GLAA.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IAmazonS3 client;
        private readonly ILogger logger;
        private static string bucketName = Environment.GetEnvironmentVariable("BUCKET_NAME");


        public FileUploadService(IAmazonS3 client, ILoggerFactory logger)
        {
            this.client = client;
            this.logger = logger.CreateLogger("File Upload Log");

        }

        public async Task UploadFile(FileStream fileStream)
        {
            try
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = "test_" + DateTime.Now.ToShortTimeString(),
                    InputStream = fileStream
                };

                var response = await client.PutObjectAsync(putRequest);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                    ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    logger.LogError("File upload failed: Check the provided AWS Credentials.");
                }
                else
                {
                    logger.LogError("Error occurred. Message:'{0}' when writing an object", amazonS3Exception.Message);
                }
            }
        }
    }
}
