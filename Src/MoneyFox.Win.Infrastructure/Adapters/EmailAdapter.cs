namespace MoneyFox.Win.Infrastructure.Adapters;

using Core.Interfaces;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

[UsedImplicitly]
public class EmailAdapter : IEmailAdapter
{
    public async Task SendEmailAsync(string subject, string body, List<string> recipients)
        => await SendEmailAsync(subject, body, recipients, new List<string>());

    // Attachments are currently not supportd on uwp
    public async Task SendEmailAsync(string subject, string body, List<string> recipients, List<string> filePaths) =>
        await Task.CompletedTask;
    // TODO: Reimplement this
    //var emailMessage = new EmailMessage { Subject = subject, Body = body };
    //foreach(EmailRecipient emailRecipient in recipients.Select(s => new EmailRecipient(s)))
    //{
    //    emailMessage.To.Add(emailRecipient);
    //}
    //await EmailManager.ShowComposeNewEmailAsync(emailMessage);
}