using MoneyManager.Common;
using MoneyManager.Src;
using System;
using Windows.ApplicationModel.Email;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

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

            var mail = new EmailMessage { Subject = Utilities.GetTranslation("Feedback") };
            mail.To.Add(sendTo);
            await EmailManager.ShowComposeNewEmailAsync(mail);
        }

        private async void GoToWebsite_OnTap(object sender, TappedRoutedEventArgs e)
        {
            var url = "http://www.apply-solutions.ch/moneyfoxbeta";
            await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
        }

        private async void GoToTwitter_OnTap(object sender, TappedRoutedEventArgs e)
        {
            var url = "http://twitter.com/npadrutt";
            await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
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

        #endregion NavigationHelper registration
    }
}