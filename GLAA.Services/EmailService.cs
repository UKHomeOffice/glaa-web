using System;
using System.Collections.Generic;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Notify.Client;
using Notify.Exceptions;
using GLAA.ViewModels;

namespace GLAA.Services
{
    public class EmailService : IEmailService
    {
        private readonly NotificationClient client;
        private readonly ILogger logger;

        public EmailService(ILoggerFactory loggerFactory, string apiKey)
        {
            client = new NotificationClient(apiKey);
            logger = loggerFactory.CreateLogger("Email Log");            
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
