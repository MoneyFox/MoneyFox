using JetBrains.Annotations;
using MoneyFox.Application.Common.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;

namespace MoneyFox.Desktop.Infrastructure.Adapters
{
    [UsedImplicitly]
    public class EmailAdapter : IEmailAdapter
    {
        public async Task SendEmailAsync(string subject, string body, List<string> recipients)
            => await SendEmailAsync(subject, body, recipients, new List<string>());

        // Attachments are currently not supportd on uwp
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