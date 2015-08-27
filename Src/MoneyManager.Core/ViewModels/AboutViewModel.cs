using Cirrious.MvvmCross.Plugins.Email;
using Cirrious.MvvmCross.Plugins.WebBrowser;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private readonly IAppInformation appInformation;
        private readonly IMvxComposeEmailTask composeEmailTask;
        private readonly IMvxWebBrowserTask webBrowserTask;

        public AboutViewModel(IAppInformation appInformation, IMvxComposeEmailTask composeEmailTask, IMvxWebBrowserTask webBrowserTask)
        {
            this.appInformation = appInformation;
            this.composeEmailTask = composeEmailTask;
            this.webBrowserTask = webBrowserTask;

            GoToTwitterCommand = new MvxCommand(GoToTwitter);
            GoToWebsiteCommand = new MvxCommand(GoToWebsite);
            SendMailCommand = new MvxCommand(SendMail);
        }

        public MvxCommand GoToTwitterCommand { get; set; }
        public MvxCommand GoToWebsiteCommand { get; set; }
        public MvxCommand SendMailCommand { get; set; }

        public string Version => appInformation.GetVersion;

        private void GoToTwitter()
        {
            const string url = "http://twitter.com/npadrutt";
            webBrowserTask.ShowWebPage(url);
        }

        private void GoToWebsite()
        {
            webBrowserTask.ShowWebPage(Strings.WebsiteUrl);
        }

        private void SendMail()
        {
            composeEmailTask.ComposeEmail(Strings.SupportMail,
                string.Empty,
                Strings.FeedbackSubject,
                string.Empty, true);
        }
    }
}