using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Command;
using MoneyFox.BusinessLogic.Adapters;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Interfaces;

namespace MoneyFox.Presentation.ViewModels
{
    public interface IAboutViewModel : IBaseViewModel
    {
        /// <summary>
        ///     Opens the webbrowser and loads to the apply solutions
        ///     website
        /// </summary>
        RelayCommand GoToWebsiteCommand { get; }

        /// <summary>
        ///     Sends a feedback mail to the apply solutions support
        ///     mail address
        /// </summary>
        RelayCommand SendMailCommand { get; }

        /// <summary>
        ///     Opens the store to rate the app.
        /// </summary>
        RelayCommand RateAppCommand { get; }

        /// <summary>
        ///     Opens the webbrowser and loads repository page
        ///     on GitHub
        /// </summary>
        RelayCommand GoToRepositoryCommand { get; }

        /// <summary>
        ///     Opens the webbrowser and loads the project on crowdin.
        /// </summary>
        RelayCommand GoToTranslationProjectCommand { get; }

        /// <summary>
        ///     Opens the webbrowser and loads the project on crowdin.
        /// </summary>
        RelayCommand GoToDesignerTwitterAccountCommand { get; }

        /// <summary>
        ///     Opens the webbrowser loads the contribution page on Github.
        /// </summary>
        RelayCommand GoToContributionPageCommand { get; }

        /// <summary>
        ///     Returns the Version of App
        /// </summary>
        string Version { get; }

        /// <summary>
        ///     Returns the apply solutions webite url from the
        ///     resource file
        /// </summary>
        string Website { get; }

        /// <summary>
        ///     Returns the mailaddress for support cases from the
        ///     resource file
        /// </summary>
        string SupportMail { get; }
    }

    public class AboutViewModel : BaseViewModel, IAboutViewModel
    {
        private readonly IAppInformation appInformation;
        private readonly IBrowserAdapter browserAdapter;
        private readonly IEmailAdapter emailAdapter;
        private readonly IStoreOperations storeFeatures;

        /// <summary>
        ///     Creates an AboutViewModel Object
        /// </summary>
        public AboutViewModel(IAppInformation appInformation,
            IEmailAdapter emailAdapter,
            IBrowserAdapter browserAdapter,
            IStoreOperations storeOperations)
        {
            this.appInformation = appInformation;
            this.emailAdapter = emailAdapter;
            this.browserAdapter = browserAdapter;
            storeFeatures = storeOperations;
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
        ///     Opens the webbrowser and loads the project on crowdin.
        /// </summary>
        public RelayCommand GoToTranslationProjectCommand => new RelayCommand(GoToTranslationProject);

        /// <summary>
        ///     Opens the webbrowser and loads the project on crowdin.
        /// </summary>
        public RelayCommand GoToDesignerTwitterAccountCommand => new RelayCommand(GoToDesignerTwitterAccount);

        /// <summary>
        ///     Opens the webbrowser loads the contribution page on Github.
        /// </summary>
        public RelayCommand GoToContributionPageCommand => new RelayCommand(GoToContributionPage);

        /// <summary>
        ///     Returns the Version of App
        /// </summary>
        public string Version => appInformation.GetVersion();

        /// <summary>
        ///     Returns the apply solutions webite url from the
        ///     resource file
        /// </summary>
        public string Website => AppConstants.WEBSITE_URL;

        /// <summary>
        ///     Returns the mailaddress for support cases from the
        ///     resource file
        /// </summary>
        public string SupportMail => AppConstants.SUPPORT_MAIL;

        private async void GoToWebsite()
        {
            await browserAdapter.OpenWebsite(new Uri(AppConstants.WEBSITE_URL));
        }

        private async void SendMail()
        {
            await emailAdapter.SendEmail(Strings.FeedbackSubject, string.Empty,
                    new List<string> { AppConstants.SUPPORT_MAIL });
        }

        private void RateApp()
        {
            storeFeatures.RateApp();
        }

        private async void GoToRepository()
        {
            await browserAdapter.OpenWebsite(new Uri(AppConstants.GIT_HUB_REPOSITORY_URL));
        }

        private async void GoToTranslationProject()
        {
            await browserAdapter.OpenWebsite(new Uri(AppConstants.TRANSLATION_PROJECT_URL));
        }

        private async void GoToDesignerTwitterAccount()
        {
            await browserAdapter.OpenWebsite(new Uri(AppConstants.ICON_DESIGNER_TWITTER_URL));
        }

        private async void GoToContributionPage()
        {
            await browserAdapter.OpenWebsite(new Uri(AppConstants.GITHUB_CONTRIBUTION_URL));
        }
    }
}