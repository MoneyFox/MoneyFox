using System;
using Windows.ApplicationModel.Email;
using Windows.ApplicationModel.Store;
using Windows.System;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using MoneyManager.Business.Helper;
using MoneyManager.Common;
using MoneyManager.Foundation;

namespace MoneyManager.Views
{
    public sealed partial class About
    {
        public About()
        {
            InitializeComponent();

            NavigationHelper = new NavigationHelper(this);
            lblVersion.Text = Utilities.GetVersion();
        }

        private NavigationHelper NavigationHelper { get; }

        private async void ComposeMail_OnTap(object sender, TappedRoutedEventArgs e) {
            var sendTo = new EmailRecipient {
                Address = Translation.GetTranslation("SupportMail")
            };

            var mail = new EmailMessage { Subject = Translation.GetTranslation("Feedback") };
            mail.To.Add(sendTo);
            await EmailManager.ShowComposeNewEmailAsync(mail);
        }

        private async void GoToWebsite_OnTap(object sender, TappedRoutedEventArgs e) {
            await Launcher.LaunchUriAsync(new Uri(Translation.GetTranslation("InternetAdress")));
        }

        private async void GoToTwitter_OnTap(object sender, TappedRoutedEventArgs e) {
            await Launcher.LaunchUriAsync(new Uri(Translation.GetTranslation("TwitterUrl")));
        }

        private async void RateApp_OnTap(object sender, TappedRoutedEventArgs e) {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
        }

        private async void GoToRepository_Tap(object sender, TappedRoutedEventArgs e) {
            await Launcher.LaunchUriAsync(new Uri(Translation.GetTranslation("GithubRepository")));
        }


        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        #endregion NavigationHelper registration
    }
}