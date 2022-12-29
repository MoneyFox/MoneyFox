namespace MoneyFox.Ui.Infrastructure.Adapters;

using Core.Interfaces;
using Serilog;

public class EmailAdapter : IEmailAdapter
{
    public async Task SendEmailAsync(string subject, string body, List<string> recipients)
    {
        try
        {
            var message = new EmailMessage { Subject = subject, Body = body, To = recipients };
            await Email.ComposeAsync(message);
        }
        catch (FeatureNotSupportedException ex)
        {
            Log.Warning(exception: ex, messageTemplate: "Error during sending email");
        }
    }

    public async Task SendEmailAsync(string subject, string body, List<string> recipients, List<string> filePaths)
    {
        try
        {
            var message = new EmailMessage
            {
                Subject = subject,
                Body = body,
                To = recipients,
                Attachments = new()
            };

            foreach (var path in filePaths)
            {
                message.Attachments.Add(new(fullPath: path, contentType: "txt"));
            }

            await Email.ComposeAsync(message);
        }
        catch (FeatureNotSupportedException ex)
        {
            Log.Warning(exception: ex, messageTemplate: "Error during sending email");
        }
    }
}
