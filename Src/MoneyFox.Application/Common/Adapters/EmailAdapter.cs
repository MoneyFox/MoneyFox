using NLog;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MoneyFox.Application.Common.Adapters
{
    public interface IEmailAdapter
    {
        Task SendEmailAsync(string subject, string body, List<string> recipients);
    }

    public class EmailAdapter : IEmailAdapter
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

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
    }
}
