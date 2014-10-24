using System.Reflection;
using Windows.ApplicationModel;
using MoneyManager.Business;
using MoneyManager.Common;
using MoneyManager.Foundation;
using System;
using Windows.ApplicationModel.Email;
using Windows.System;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace MoneyManager.Views
{
    public sealed partial class About
    {
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly NavigationHelper navigationHelper;

        public About()
        {
            InitializeComponent();

            navigationHelper = new NavigationHelper(this);
            lblVersion.Text = Utilities.GetVersion();
        }

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        public ObservableDictionary DefaultViewModel
        {
            get { return defaultViewModel; }
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