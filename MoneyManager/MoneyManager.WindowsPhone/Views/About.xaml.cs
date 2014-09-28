using Windows.ApplicationModel.Email;
using Microsoft.VisualBasic.CompilerServices;
using MoneyManager.Common;
using System;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using MoneyManager.Src;

namespace MoneyManager.Views
{
    public sealed partial class About
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public About()
        {
            InitializeComponent();

            navigationHelper = new NavigationHelper(this);
            lblVersion.Text = Utilities.GetVersion();
        }

        private async void ComposeMail_OnTap(object sender, TappedRoutedEventArgs e)
        {
            var sendTo = new EmailRecipient()
            {
                Address = Utilities.GetTranslation("SupportMail")
            };

            var mail = new EmailMessage {Subject = Utilities.GetTranslation("Feedback") };
            mail.To.Add(sendTo);
            await EmailManager.ShowComposeNewEmailAsync(mail);
        }

        private async void GoToWebsite_OnTap(object sender, TappedRoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(lblWebsite.Text));
        }

        private async void GoToTwitter_OnTap(object sender, TappedRoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(lblTwitter.Text));
        }

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        public ObservableDictionary DefaultViewModel
        {
            get { return defaultViewModel; }
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
