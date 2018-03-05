using GLAA.Services.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GLAA.Services
{
    public class VirusScanService : IVirusScanService        
    {
        private const string clamAVUrl = "https://clamav.virus-scan.svc.cluster.local/scan";

        private readonly ILogger<VirusScanService> logger;

        public VirusScanService(ILogger<VirusScanService> logger)
        {
            this.logger = logger;
        }

        public async Task<VirusScanResult> ScanFileAsync(IFormFile file)
        {
            logger.TimedLog(LogLevel.Warning, "Starting virus scan for file: " + file.FileName);

            var result = new VirusScanResult();

            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                }
            };

            using (HttpClient client = new HttpClient(handler))
            {
                var requestContent = new MultipartFormDataContent();
                var fileContent = new StreamContent(file.OpenReadStream());                

                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    Name = "\"file\"",
                    FileName = "\"" + file.Name + "\""
                };

                requestContent.Add(fileContent);

                using (var res = await client.PostAsync(clamAVUrl, requestContent))
                using (var content = res.Content)
                {
                    if(res.IsSuccessStatusCode)
                    {
                        var data = await content.ReadAsStringAsync();
                        logger.TimedLog(LogLevel.Warning, "Virus scan response : " + data);

                        result.Success = true;
                        result.Message = data;

                        //if (data.Contains("ok: false")) { // do things }
                    }
                    else
                    {                        
                        logger.TimedLog(LogLevel.Error, "Failed to access virus scan service : " + res.StatusCode);
                        result.Success = false;
                        result.Message = "Failed to access virus scan service";
                    }
                }

                return result;
            }
        }
    }
}
