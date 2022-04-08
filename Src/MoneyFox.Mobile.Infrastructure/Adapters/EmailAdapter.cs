namespace MoneyFox.Mobile.Infrastructure.Adapters
{

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Interfaces;
    using Serilog;
    using Xamarin.Essentials;

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
                var message = new EmailMessage { Subject = subject, Body = body, To = recipients };
                foreach (var path in filePaths)
                {
                    message.Attachments.Add(new EmailAttachment(fullPath: path, contentType: "txt"));
                }

                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                Log.Warning(exception: ex, messageTemplate: "Error during sending email");
            }
        }
    }

}
