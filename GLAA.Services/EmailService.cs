using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Logging;
using Notify.Client;
using Notify.Exceptions;

namespace GLAA.Services
{
    public class EmailService : IEmailService
    {
        private const string APIKEY = "";

        private readonly NotificationClient client;
        private readonly ILogger logger;

        public EmailService(ILoggerFactory loggerFactory)
        {
            client = new NotificationClient(APIKEY);
            logger = loggerFactory.CreateLogger("Email Log");
        }

        public bool Send(MailMessage msg)
        {
            try
            {
                client.SendEmail(
                    msg.To.ToString(),
                    "",
                    new Dictionary<string, dynamic>
                    {
                        { "first_name", "Doug" }
                    },
                    null,
                    null);

                logger.LogInformation($"Email sent to Notify : {DateTime.UtcNow.ToFileTimeUtc()} : {msg.To}");

                return true;

            }
            catch (NotifyClientException ex)
            {
                logger.LogError(ex.Message, ex);
                return false;
            }
        }
    }
}
