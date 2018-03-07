using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using GLAA.Repository;
using GLAA.ViewModels.File;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GLAA.Services.File
{
    public class FileService : IFileService
    {
        private readonly IAmazonS3 client;
        private readonly ILogger logger;
        private static readonly string bucketName = Environment.GetEnvironmentVariable("BUCKET_NAME");
        private readonly IEntityFrameworkRepository repository;
        private readonly ILicenceRepository licenceRepository;

        public FileService(IAmazonS3 client, ILoggerFactory logger, IEntityFrameworkRepository repository, ILicenceRepository licenceRepository)
        {
            this.client = client;
            this.logger = logger.CreateLogger("File Upload Log");
            this.repository = repository;
            this.licenceRepository = licenceRepository;
        }

        public async Task<FileUploadedViewModel> UploadFile(FileUploadViewModel fileUploadViewModel)
        {
            var file = UpsertFileToDb(fileUploadViewModel);
            var fileUploadedViewModel = await UploadToS3Bucket(fileUploadViewModel, file.Key.ToString());

            return fileUploadedViewModel;
        }

        private Domain.Models.File UpsertFileToDb(FileUploadViewModel fileUploadViewModel)
        {
            var fileName = fileUploadViewModel.FormFileUpload.FileName;
            var fileType = fileUploadViewModel.FormFileUpload.ContentType;
            //TODO - placeholder licence
            var licence = licenceRepository.GetByApplicationId("TEST-0001");

            var file = repository.Create<Domain.Models.File>();

            file.Licence = licence;
            file.FileName = fileName;
            file.FileType = fileType;
            file.Key = Guid.NewGuid();
            file.Deleted = false;

            repository.Upsert(file);
            return file;
        }

        private async Task<FileUploadedViewModel> UploadToS3Bucket(FileUploadViewModel fileUploadViewModel, string key)
        {
            var stream = new MemoryStream();
            fileUploadViewModel.FormFileUpload.CopyTo(stream);
            var fileUploadedViewModel = new FileUploadedViewModel { Key = key };

            try
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = key,
                    InputStream = stream,
                    ServerSideEncryptionMethod = ServerSideEncryptionMethod.AWSKMS
                };

                await client.PutObjectAsync(putRequest);

                fileUploadedViewModel.Exception = null;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                     ||
                     amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                    logger.LogError("File upload failed: Check the provided AWS Credentials.");
                else
                    logger.LogError("Error occurred. Message:'{0}' when writing an object", amazonS3Exception.Message);

                fileUploadedViewModel.Exception = amazonS3Exception;
            }
            return fileUploadedViewModel;
        }

        public FileSummaryViewModel GetFileSummary(string key)
        {
            var file = repository.GetAll<Domain.Models.File>().Single(x => x.Key == new Guid(key));

            var fileViewModel = new FileSummaryViewModel
            {
                Key = key,
                FileName = file.FileName,
                FileType = file.FileType,
            };

            return fileViewModel;
        }

        public async Task<FileStreamResult> GetFilePreviewImage(string key)
        {
            var s3File = await GetFromS3Bucket(key);
            FileStreamResult fileStreamResult;
            var memoryStream = new MemoryStream();

            using (var stream = s3File.ResponseStream)
            {
                using (var thumbnail = Image.FromStream(stream).GetThumbnailImage(128, 128, null, new IntPtr()))
                    thumbnail.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

                fileStreamResult = new FileStreamResult(memoryStream, "image/png");
            }

            memoryStream.Seek(0, SeekOrigin.Begin);

            return fileStreamResult;
        }

        private async Task<GetObjectResponse> GetFromS3Bucket(string key)
        {
            var getObjectRequest = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = key,
            };

            var response = await client.GetObjectAsync(getObjectRequest);
            return response;
        }
    }
}
