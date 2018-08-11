using System.Net.Mail;

namespace dev.Core.Messaging.Interface
{
    public interface IEmailProvider
    {
        void Send(MailMessage mail);
    }
}
