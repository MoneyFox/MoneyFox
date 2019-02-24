using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;

namespace MoneyFox.BusinessLogic.Adapters
{
    public interface IEmailAdapter
    {
        Task SendEmail(string subject, string body, List<string> recipients);
    }

    public class EmailAdapter : IEmailAdapter
    {
        public async Task SendEmail(string subject, string body, List<string> recipients)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients
                };
                await Email.ComposeAsync(message).ConfigureAwait(false);
            } 
            catch (FeatureNotSupportedException ex)
            {
                Crashes.TrackError(ex);
            } 
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}
