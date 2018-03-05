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
            logger.TimedLog(LogLevel.Information, "Starting virus scan for file: " + file.FileName);

            var result = new VirusScanResult();

            using (HttpClient client = new HttpClient())
            {
                var requestContent = new MultipartFormDataContent();
                var fileContent = new StreamContent(file.OpenReadStream());

                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    Name = "\"file\"",
                    FileName = "\"" + file.Name + "\""
                };

                requestContent.Add(fileContent);

                using (HttpResponseMessage res = await client.PostAsync(clamAVUrl, requestContent))
                using (HttpContent content = res.Content)
                {
                    if(res.IsSuccessStatusCode)
                    {
                        string data = await content.ReadAsStringAsync();
                        logger.TimedLog(LogLevel.Information, "Virus scan response : " + data);

                        result.Success = true;
                        result.Message = data;

                        //if (data.Contains("ok: false")) { // do things }
                    }
                    else
                    {                        
                        logger.TimedLog(LogLevel.Information, "Failed to access virus scan service : " + res.StatusCode);
                        result.Success = false;
                        result.Message = "Failed to access virus scan service";
                    }
                }

                return result;
            }
        }
    }
}
