using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GLAA.ViewModels;
using Microsoft.Extensions.Configuration;
using GLAA.Services;
using Microsoft.Extensions.Logging;
using GLAA.Services.Extensions;

namespace GLAA.Scheduler.Tasks
{
    public class SendTestEmailTask : IScheduledTask
    {
        private IEmailService emailService;
        private IConfiguration configuration;
        private ILogger logger;
        public SendTestEmailTask(IEmailService emailService, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            this.emailService = emailService;
            this.configuration = configuration;
            logger = loggerFactory.CreateLogger<SendTestEmailTask>();
        }

        public string Schedule => "* * * * *";

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            logger.LogWithTimestamp(LogLevel.Information, $"Task Started: Send Test Email");
            var msg = new NotifyMailMessage("dmcdonald@bmtdsl.co.uk", new Dictionary<string, dynamic>
            {
                {"full_name", "Doug"},
                {"confirm_email_link", "na"}
            });

            var template = configuration.GetSection("GOVNotify:EmailTemplates")["ConfirmEmail"];
            var success = emailService.Send(msg, template);
            var successMessage = success ? "SUCCESS" : "FAILED";
            logger.LogWithTimestamp(LogLevel.Information, $"Task Completed: Send Test Email : {successMessage}");
        }
    }
}
