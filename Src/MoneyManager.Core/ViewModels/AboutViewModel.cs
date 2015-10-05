using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Localization;
using MvvmCross.Plugins.Email;
using MvvmCross.Plugins.WebBrowser;

namespace MoneyManager.Core.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private readonly IAppInformation appInformation;
        private readonly IMvxComposeEmailTask composeEmailTask;
        private readonly IStoreFeatures storeFeatures;
        private readonly IMvxWebBrowserTask webBrowserTask;

        /// <summary>
        ///     Creates an AboutViewModel Object
        /// </summary>
        /// <param name="appInformation">Instance of a <see cref="IAppInformation" /> implementation.</param>
        /// <param name="composeEmailTask">Instance of a <see cref="IMvxComposeEmailTask" /> implementation.</param>
        /// <param name="webBrowserTask">Instance of a <see cref="IMvxWebBrowserTask" /> implementation.</param>
        /// <param name="storeFeatures">Instance of a <see cref="IStoreFeatures" /> implementation.</param>
        public AboutViewModel(IAppInformation appInformation,
            IMvxComposeEmailTask composeEmailTask,
            IMvxWebBrowserTask webBrowserTask,
            IStoreFeatures storeFeatures)
        {
            this.appInformation = appInformation;
            this.composeEmailTask = composeEmailTask;
            this.webBrowserTask = webBrowserTask;
            this.storeFeatures = storeFeatures;

            GoToWebsiteCommand = new MvxCommand(GoToWebsite);
            SendMailCommand = new MvxCommand(SendMail);
            RateAppCommand = new MvxCommand(RateApp);
            GoToRepositoryCommand = new MvxCommand(GoToRepository);
        }

        /// <summary>
        ///     Opens the webbrowser and loads to the apply solutions
        ///     website
        /// </summary>
        public MvxCommand GoToWebsiteCommand { get; set; }

        /// <summary>
        ///     Sends a feedback mail to the apply solutions support
        ///     mail address
        /// </summary>
        public MvxCommand SendMailCommand { get; set; }

        /// <summary>
        ///     Opens the store to rate the app.
        /// </summary>
        public MvxCommand RateAppCommand { get; set; }

        /// <summary>
        ///     Opens the webbrowser and loads repository page
        ///     on GitHub
        /// </summary>
        public MvxCommand GoToRepositoryCommand { get; set; }

        /// <summary>
        ///     Returns the Version of App
        /// </summary>
        public string Version => appInformation.GetVersion;

        /// <summary>
        ///     Returns the apply solutions webite url from the
        ///     ressource file
        /// </summary>
        public string Website => Strings.WebsiteUrl;

        /// <summary>
        ///     Returns the mailaddress for support cases from the
        ///     ressource file
        /// </summary>
        public string SupportMail => Strings.SupportMail;

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

        private void RateApp()
        {
            storeFeatures.RateApp();
        }

        private void GoToRepository()
        {
            webBrowserTask.ShowWebPage(Strings.GitHubRepositoryUrl);
        }
    }
}