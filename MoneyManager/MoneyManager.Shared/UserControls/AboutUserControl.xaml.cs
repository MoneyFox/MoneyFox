using MoneyManager.Business.Helper;
using MoneyManager.Foundation;
using System;
using Windows.ApplicationModel.Email;
using Windows.System;
using Windows.UI.Xaml.Input;

namespace MoneyManager.UserControls
{
    public sealed partial class AboutUserControl
    {
        public AboutUserControl()
        {
            InitializeComponent();

            lblVersion.Text = Utilities.GetVersion();
        }

        private async void ComposeMail_OnTap(object sender, TappedRoutedEventArgs e)
        {
            var sendTo = new EmailRecipient
            {
                Address = Translation.GetTranslation("SupportMail")
            };

            var mail = new EmailMessage {Subject = Translation.GetTranslation("Feedback")};
            mail.To.Add(sendTo);
            await EmailManager.ShowComposeNewEmailAsync(mail);
        }

        private async void GoToWebsite_OnTap(object sender, TappedRoutedEventArgs e)
        {
            string url = "http://www.apply-solutions.ch/moneyfoxbeta";
            await Launcher.LaunchUriAsync(new Uri(url));
        }

        private async void GoToTwitter_OnTap(object sender, TappedRoutedEventArgs e)
        {
            string url = "http://twitter.com/npadrutt";
            await Launcher.LaunchUriAsync(new Uri(url));
        }
    }
}