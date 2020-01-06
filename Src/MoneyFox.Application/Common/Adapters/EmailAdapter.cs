using MoneyFox.Application.Common.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MoneyFox.Application.Common.Adapters
{
    public interface IEmailAdapter
    {
        Task SendEmailAsync(string subject, string body, List<string> recipients);
        Task SendEmailAsync(string subject, string body, List<string> recipients, List<string> filePaths);
    }

    public class EmailAdapter : IEmailAdapter
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        IDialogService dialogService;

        public EmailAdapter( dialogService)
        {
            this.dialogService = dialogService;
        }

        public async Task SendEmailAsync(string subject, string body, List<string> recipients)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients
                };

                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                logManager.Warn(ex);
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
                    To = recipients
                };

                foreach(var path in filePaths)
                {
                    message.Attachments.Add(new EmailAttachment(path));
                }

                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                logManager.Warn(ex);
                await dialogService.ShowMessage("Warning", ex.ToString());
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.ToString());
            }
        }
    }
}
