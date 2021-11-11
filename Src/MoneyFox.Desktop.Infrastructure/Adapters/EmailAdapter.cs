using MoneyFox.Application.Common.Adapters;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;

namespace MoneyFox.Desktop.Infrastructure.Adapters
{
    public class EmailAdapter : IEmailAdapter
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        public async Task SendEmailAsync(string subject, string body, List<string> recipients)
            => await SendEmailAsync(subject, body, recipients, new List<string>());


        // Attachments are currntly not supportd on uwp
        public async Task SendEmailAsync(string subject, string body, List<string> recipients, List<string> filePaths)
        {
            var emailMessage = new EmailMessage {Subject = subject, Body = body};

            foreach(EmailRecipient emailRecipient in recipients.Select(s => new EmailRecipient(s)))
            {
                emailMessage.To.Add(emailRecipient);
            }

            await EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }
    }
}