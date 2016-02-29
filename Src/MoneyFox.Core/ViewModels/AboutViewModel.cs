using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Core.ViewModels
{
    public class AboutViewModel : ViewModelBase
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

            GoToWebsiteCommand = new RelayCommand(GoToWebsite);
            SendMailCommand = new RelayCommand(SendMail);
            RateAppCommand = new RelayCommand(RateApp);
            GoToRepositoryCommand = new RelayCommand(GoToRepository);
        }

        /// <summary>
        ///     Opens the webbrowser and loads to the apply solutions
        ///     website
        /// </summary>
        public RelayCommand GoToWebsiteCommand { get; set; }

        /// <summary>
        ///     Sends a feedback mail to the apply solutions support
        ///     mail address
        /// </summary>
        public RelayCommand SendMailCommand { get; set; }

        /// <summary>
        ///     Opens the store to rate the app.
        /// </summary>
        public RelayCommand RateAppCommand { get; set; }

        /// <summary>
        ///     Opens the webbrowser and loads repository page
        ///     on GitHub
        /// </summary>
        public RelayCommand GoToRepositoryCommand { get; set; }

        /// <summary>
        ///     Returns the Version of App
        /// </summary>
        public string Version => appInformation.Version;

        /// <summary>
        ///     Returns the apply solutions webite url from the
        ///     ressource file
        /// </summary>
        public string Website => Constants.WEBSITE_URL;

        /// <summary>
        ///     Returns the mailaddress for support cases from the
        ///     ressource file
        /// </summary>
        public string SupportMail => Constants.SUPPORT_MAIL;

        private void GoToWebsite()
        {
            webBrowserTask.ShowWebPage(Constants.WEBSITE_URL);
        }

        private void SendMail()
        {
            composeEmailTask.ComposeEmail(Constants.SUPPORT_MAIL,
                string.Empty,
                Strings.FeedbackSubject,
                string.Empty,
                true);
        }

        private void RateApp()
        {
            storeFeatures.RateApp();
        }

        private void GoToRepository()
        {
            webBrowserTask.ShowWebPage(Constants.GIT_HUB_REPOSITORY_URL);
        }
    }
}