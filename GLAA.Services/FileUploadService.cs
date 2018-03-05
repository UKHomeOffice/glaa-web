using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GLAA.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IAmazonS3 client;
        private readonly ILogger logger;
        private readonly IVirusScanService virusScanService;
        private static string bucketName = Environment.GetEnvironmentVariable("BUCKET_NAME");


        public FileUploadService(IAmazonS3 client, ILoggerFactory logger, IVirusScanService virusScanService)
        {
            this.client = client;
            this.logger = logger.CreateLogger("File Upload Log");
            this.virusScanService = virusScanService;
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

                putRequest.ServerSideEncryptionMethod = ServerSideEncryptionMethod.AWSKMS;

                var file = new FormFile(fileStream, 0, fileStream.Length, "FileUploadControl", "RobotHand.png");                

                var virusScanResult = await virusScanService.ScanFileAsync(file);

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
