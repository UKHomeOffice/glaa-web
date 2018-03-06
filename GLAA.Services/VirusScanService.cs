using GLAA.Services.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
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

        private X509Certificate2 GetCert()
        {
            var cert = new X509Certificate2(@"certificates/ca-bundle.crt");

            logger.TimedLog(LogLevel.Information, "Getting SSL certificate...");
            logger.TimedLog(LogLevel.Information, "Certificate issuer: " + cert.Issuer);
            logger.TimedLog(LogLevel.Information, "Effective date: " + cert.GetEffectiveDateString());
            logger.TimedLog(LogLevel.Information, "Name info: " + cert.GetNameInfo(X509NameType.SimpleName, true));
            logger.TimedLog(LogLevel.Information, "Has private key?: " + cert.HasPrivateKey);

            return cert;
        }

        private HttpClientHandler GetSSLHandler()
        {
            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                //SslProtocols = SslProtocols.Tls12,
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            //handler.ClientCertificates.Add(GetCert());

            return handler;
        }

        public async Task<VirusScanResult> ScanFileAsync(IFormFile file)
        {
            logger.TimedLog(LogLevel.Information, "Starting virus scan for file: " + file.FileName);

            var result = new VirusScanResult();

            using (var client = new HttpClient(GetSSLHandler()))
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
                    var data = await content.ReadAsStringAsync();

                    if (res.IsSuccessStatusCode)
                    {                        
                        logger.TimedLog(LogLevel.Warning, "Virus scan response: " + data);

                        result.Success = true;
                        result.Message = data;

                        //if (data.Contains("ok: false")) { // do things }
                    }
                    else
                    {                           
                        logger.TimedLog(LogLevel.Error, "Failed to access virus scan service: " + res.StatusCode + " : " + data);
                        result.Success = false;
                        result.Message = "Failed to access virus scan service";
                    }
                }

                return result;
            }
        }
    }
}
