using Cimbalino.Toolkit.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MoneyManager.Foundation;

namespace MoneyFox.Core.ViewModels
{
    public class AboutViewModel : ViewModelBase {
        private readonly IEmailComposeService emailComposeService;
        private readonly IAppInformation appInformation;
        private readonly ILauncherService launcherService;
        private readonly IStoreService storeService;

        /// <summary>
        ///     Creates an AboutViewModel Object
        /// </summary>
        /// <param name="appInformation">Instance of a <see cref="IAppInformation" /> implementation.</param>
        /// <param name="emailComposeService">Instance of a <see cref="IEmailComposeService" /> implementation.</param>
        /// <param name="launcherService">Instance of a <see cref="ILauncherService" /> implementation.</param>
        /// <param name="storeService">Instance of a <see cref="IStoreService" /> implementation.</param>
        public AboutViewModel(IAppInformation appInformation,
            IEmailComposeService emailComposeService,
            ILauncherService launcherService,
            IStoreService storeService)
        {
            this.appInformation = appInformation;
            this.emailComposeService = emailComposeService;
            this.launcherService = launcherService;
            this.storeService = storeService;
        }

        /// <summary>
        ///     Opens the webbrowser and loads to the apply solutions
        ///     website
        /// </summary>
        public RelayCommand GoToWebsiteCommand => new RelayCommand(GoToWebsite);

        /// <summary>
        ///     Sends a feedback mail to the apply solutions support
        ///     mail address
        /// </summary>
        public RelayCommand SendMailCommand => new RelayCommand(SendMail);

        /// <summary>
        ///     Opens the store to rate the app.
        /// </summary>
        public RelayCommand RateAppCommand => new RelayCommand(RateApp);

        /// <summary>
        ///     Opens the webbrowser and loads repository page
        ///     on GitHub
        /// </summary>
        public RelayCommand GoToRepositoryCommand => new RelayCommand(GoToRepository);

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
            launcherService.LaunchUriAsync(Constants.WEBSITE_URL);
        }

        private void SendMail()
        {
            emailComposeService.ShowAsync(Constants.SUPPORT_MAIL,
                string.Empty,
                Strings.FeedbackSubject,
                string.Empty,
                string.Empty);
        }

        private void RateApp()
        {
            storeService.ReviewAsync(appInformation.Id);
        }

        private void GoToRepository()
        {
            launcherService.LaunchUriAsync(Constants.GIT_HUB_REPOSITORY_URL);
        }
    }
}