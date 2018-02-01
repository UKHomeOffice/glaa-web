using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using GLAA.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GLAA.ViewModels;

namespace GLAA.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Notify")]
    public class NotifyController : Controller
    {
        private readonly IEmailService emailService;

        public NotifyController(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        [Route("api/Notify/SendEmail")]
        public bool SendEmail(NotifyMailMessage msg, string template)
        {
            return emailService.Send(msg, template);
        }
    }
}