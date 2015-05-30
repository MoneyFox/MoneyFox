using System;
using Windows.ApplicationModel.Email;
using Windows.ApplicationModel.Store;
using Windows.System;
using Windows.UI.Xaml.Input;
using MoneyManager.Business.Helper;
using MoneyManager.Foundation;

namespace MoneyManager.UserControls {
    public sealed partial class AboutUserControl {
        public AboutUserControl() {
            InitializeComponent();

            lblVersion.Text = Utilities.GetVersion();
        }

        private async void ComposeMail_OnTap(object sender, TappedRoutedEventArgs e) {
            var sendTo = new EmailRecipient {
                Address = Translation.GetTranslation("SupportMail")
            };

            var mail = new EmailMessage {Subject = Translation.GetTranslation("Feedback")};
            mail.To.Add(sendTo);
            await EmailManager.ShowComposeNewEmailAsync(mail);
        }

        private async void GoToWebsite_OnTap(object sender, TappedRoutedEventArgs e) {
            const string url = "http://www.apply-solutions.ch";
            await Launcher.LaunchUriAsync(new Uri(url));
        }

        private async void GoToTwitter_OnTap(object sender, TappedRoutedEventArgs e) {
            const string url = "http://twitter.com/npadrutt";
            await Launcher.LaunchUriAsync(new Uri(url));
        }

        private async void RateApp_OnTap(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri( "ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
        }


    }
}