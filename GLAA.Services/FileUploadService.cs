using Amazon.S3;
using Amazon.S3.Model;
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
        private static string bucketName = Environment.GetEnvironmentVariable("BUCKET_NAME");


        public FileUploadService(IAmazonS3 client)
        {
            this.client = client;
        }

        public async Task UploadFile(FileStream fileStream)
        {
            try
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
                        Console.WriteLine("Check the provided AWS Credentials.");
                        Console.WriteLine("For service sign up go to http://aws.amazon.com/s3");
                    }
                    else
                    {
                        Console.WriteLine(
                            "Error occurred. Message:'{0}' when writing an object"
                            , amazonS3Exception.Message);
                    }
                }
                
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
