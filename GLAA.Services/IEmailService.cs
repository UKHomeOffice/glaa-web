using GLAA.ViewModels;

namespace GLAA.Services
{
    public interface IEmailService
    {
        bool Send(NotifyMailMessage msg, string template);
    }
}
