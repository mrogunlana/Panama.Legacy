using dev.Core.Messaging.Interface;
using System.Net.Mail;

namespace dev.Core.Messaging
{
    public class EmailProvider : IEmailProvider
    {
        public void Send(MailMessage mail)
        {
            var client = new SmtpClient();
            client.Send(mail);
        }
    }
}
