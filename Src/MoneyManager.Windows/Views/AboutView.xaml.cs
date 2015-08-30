using System;
using Windows.ApplicationModel.Email;
using Windows.ApplicationModel.Store;
using Windows.System;
using Windows.UI.Xaml.Input;
using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;

namespace MoneyManager.Windows.Views
{
    public sealed partial class AboutView
    {
        public AboutView()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<AboutViewModel>();

            //TODO: move to VM
            //lblVersion.Text = Utilities.GetVersion();
        }


        private async void ComposeMail_OnTap(object sender, TappedRoutedEventArgs e)
        {
            var sendTo = new EmailRecipient
            {
                Address = Strings.SupportMail
            };

            var mail = new EmailMessage {Subject = Strings.FeedbackLabel};
            mail.To.Add(sendTo);
            await EmailManager.ShowComposeNewEmailAsync(mail);
        }

        private async void GoToWebsite_OnTap(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(Strings.Homepage));
        }

        private async void RateApp_OnTap(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
        }

        private async void GoToRepository_Tap(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(Strings.GitHubRepositoryUrl));
        }
    }
}