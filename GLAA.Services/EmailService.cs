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
        private readonly ILogger logger;

        public EmailService(ILoggerFactory loggerFactory, string apiKey)
        {
            client = new NotificationClient(apiKey);
            logger = loggerFactory.CreateLogger<EmailService>();            
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

                logger.LogWithTimestamp(LogLevel.Information, $"Email sent to GOV.Notify : Address : {msg.To} : Success");
                return true;

            }
            catch (NotifyClientException ex)
            {
                logger.LogWithTimestamp(LogLevel.Error, $"Email sending to GOV.Notify FAILED : Message: {ex.Message}", ex);
                return false;
            }
        }
    }
}
