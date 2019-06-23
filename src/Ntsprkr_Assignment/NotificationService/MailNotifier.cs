using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Ntsprkr_Assignment.NotificationService
{
    public class MailNotifier : INotifier
    {
        public NotifyTypes Type => NotifyTypes.Email;
        private MailNotifierOptions _options;
        public MailNotifierOptions Options { get { return _options; } }

        public MailNotifier(MailNotifierOptions options)
        {
            _options = options;
        }
        

        public async Task NotifyAsync(string notification)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(Options.SmtpServer);

            mail.From = new MailAddress(Options.From);
            foreach (var to in Options.To)
            {
                mail.To.Add(to);
            }
            mail.Subject = Options.Subject;
            mail.Body = Options.BodyTemplate.Replace("{{notification}}",notification);

            SmtpServer.Port = Options.Port;
            SmtpServer.Credentials = new System.Net.NetworkCredential(Options.UserName, Options.Password);
            SmtpServer.EnableSsl = true;

            await SmtpServer.SendMailAsync(mail);
        }
    }
    public class MailNotifierOptions
    {
        public string SmtpServer { get; set; }
        public string From { get; set; }
        public List<string> To { get; set; }
        public string Subject { get; set; }
        public string BodyTemplate { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
