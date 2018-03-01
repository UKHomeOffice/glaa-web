using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using GLAA.ViewModels.File;
using Microsoft.Extensions.Logging;

namespace GLAA.Services.File
{
    public class FileService : IFileService
    {
        private readonly IAmazonS3 client;
        private readonly ILogger logger;
        private static readonly string bucketName = Environment.GetEnvironmentVariable("BUCKET_NAME");

        public FileService(IAmazonS3 client, ILoggerFactory logger)
        {
            this.client = client;
            this.logger = logger.CreateLogger("File Upload Log");
        }

        public async Task<FileUploadedViewModel> UploadFile(FileUploadViewModel fileUploadViewModel)
        {
            var stream = new MemoryStream();
            fileUploadViewModel.FormFileUpload.CopyTo(stream);

            var filename = fileUploadViewModel.FormFileUpload.FileName;
            var key = GetKey(fileUploadViewModel);
            var fileUploadedViewModel = new FileUploadedViewModel { Key = key, FileName = filename };

            try
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = key,
                    InputStream = stream,
                    ServerSideEncryptionMethod = ServerSideEncryptionMethod.AWSKMS
                };

                var response = await client.PutObjectAsync(putRequest);

                fileUploadedViewModel.Exception = null;
                fileUploadedViewModel.ETag = response.ETag;
                fileUploadedViewModel.VersionId = response.ETag;
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

                fileUploadedViewModel.Exception = amazonS3Exception;
            }

            return fileUploadedViewModel;
        }

        public async Task<FileSummaryViewModel> GetFileSummary(string key)
        {
            var getObjectRequest = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = key,
            };

            var response = await client.GetObjectAsync(getObjectRequest);

            var fileViewModel = new FileSummaryViewModel
            {
                Key = key,
                FileName = GetFileName(key),
                Preview = GetFilePreview(response.ResponseStream)
            };

            return fileViewModel;
        }

        private byte[] GetFilePreview(Stream stream)
        {
            return new byte[] { };
        }
        private string GetKey(FileUploadViewModel fileUploadViewModel)
        {
            var builder = new StringBuilder();
            builder.Append(fileUploadViewModel.FormFileUpload.FileName);
            builder.Append("_");
            builder.Append(Guid.NewGuid());
            builder.Append("_");
            builder.Append(DateTime.Now.Ticks.ToString());

            return builder.ToString();
        }

        private string GetFileName(string key)
        {
            var keySections = key.Split("_");

            return keySections.Length > 0 ? keySections[0] : string.Empty;
        }
    }
}
