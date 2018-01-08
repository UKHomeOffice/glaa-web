using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace GLAA.Services
{
    public interface IEmailService
    {
        bool Send(MailMessage msg);
    }
}
