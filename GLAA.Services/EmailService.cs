using Microsoft.Extensions.Logging;
using Notify.Client;
using Notify.Exceptions;
using GLAA.ViewModels;
using GLAA.Services.Extensions;

namespace GLAA.Services
{
    public class EmailService : IEmailService
    {
        private readonly NotificationClient client;
        private readonly ILogger<EmailService> logger;

        public EmailService(ILogger<EmailService> logger, string apiKey)
        {
            client = new NotificationClient(apiKey);
            this.logger = logger;            
        }

        public bool Send(NotifyMailMessage msg, string messageTemplate)
        {
            try
            {                
                client.SendEmail(
                    msg.To,
                    messageTemplate,
                    msg.Personalisation,
                    null,
                    null);

                logger.TimedLog(LogLevel.Information, $"Email sent to GOV.Notify : Address : {msg.To} : Success");
                return true;

            }
            catch (NotifyClientException ex)
            {
                logger.TimedLog(LogLevel.Error, $"Email sending to GOV.Notify FAILED : Message: {ex.Message}", ex);
                return false;
            }
        }
    }
}
