using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyFox.BusinessLogic.Adapters;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.ViewModels;
using MvvmCross.Commands;

namespace MoneyFox.Presentation.ViewModels
{
    public interface IAboutViewModel : IBaseViewModel
    {
        /// <summary>
        ///     Opens the webbrowser and loads to the apply solutions
        ///     website
        /// </summary>
        MvxAsyncCommand GoToWebsiteCommand { get; }

        /// <summary>
        ///     Sends a feedback mail to the apply solutions support
        ///     mail address
        /// </summary>
        MvxAsyncCommand SendMailCommand { get; }

        /// <summary>
        ///     Opens the store to rate the app.
        /// </summary>
        MvxCommand RateAppCommand { get; }

        /// <summary>
        ///     Opens the webbrowser and loads repository page
        ///     on GitHub
        /// </summary>
        MvxAsyncCommand GoToRepositoryCommand { get; }

        /// <summary>
        ///     Opens the webbrowser and loads the project on crowdin.
        /// </summary>
        MvxAsyncCommand GoToTranslationProjectCommand { get; }

        /// <summary>
        ///     Opens the webbrowser and loads the project on crowdin.
        /// </summary>
        MvxAsyncCommand GoToDesignerTwitterAccountCommand { get; }

        /// <summary>
        ///     Opens the webbrowser loads the contribution page on Github.
        /// </summary>
        MvxAsyncCommand GoToContributionPageCommand { get; }

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
        public MvxAsyncCommand GoToWebsiteCommand => new MvxAsyncCommand(GoToWebsite);

        /// <summary>
        ///     Sends a feedback mail to the apply solutions support
        ///     mail address
        /// </summary>
        public MvxAsyncCommand SendMailCommand => new MvxAsyncCommand(SendMail);

        /// <summary>
        ///     Opens the store to rate the app.
        /// </summary>
        public MvxCommand RateAppCommand => new MvxCommand(RateApp);

        /// <summary>
        ///     Opens the webbrowser and loads repository page
        ///     on GitHub
        /// </summary>
        public MvxAsyncCommand GoToRepositoryCommand => new MvxAsyncCommand(GoToRepository);

        /// <summary>
        ///     Opens the webbrowser and loads the project on crowdin.
        /// </summary>
        public MvxAsyncCommand GoToTranslationProjectCommand => new MvxAsyncCommand(GoToTranslationProject);

        /// <summary>
        ///     Opens the webbrowser and loads the project on crowdin.
        /// </summary>
        public MvxAsyncCommand GoToDesignerTwitterAccountCommand => new MvxAsyncCommand(GoToDesignerTwitterAccount);

        /// <summary>
        ///     Opens the webbrowser loads the contribution page on Github.
        /// </summary>
        public MvxAsyncCommand GoToContributionPageCommand => new MvxAsyncCommand(GoToContributionPage);

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

        private async Task GoToWebsite()
        {
            await browserAdapter.OpenWebsite(new Uri(AppConstants.WEBSITE_URL));
        }

        private async Task SendMail()
        {
            await emailAdapter.SendEmail(Strings.FeedbackSubject, string.Empty,
                    new List<string> { AppConstants.SUPPORT_MAIL });
        }

        private void RateApp()
        {
            storeFeatures.RateApp();
        }

        private async Task GoToRepository()
        {
            await browserAdapter.OpenWebsite(new Uri(AppConstants.GIT_HUB_REPOSITORY_URL));
        }

        private async Task GoToTranslationProject()
        {
            await browserAdapter.OpenWebsite(new Uri(AppConstants.TRANSLATION_PROJECT_URL));
        }

        private async Task GoToDesignerTwitterAccount()
        {
            await browserAdapter.OpenWebsite(new Uri(AppConstants.ICON_DESIGNER_TWITTER_URL));
        }

        private async Task GoToContributionPage()
        {
            await browserAdapter.OpenWebsite(new Uri(AppConstants.GITHUB_CONTRIBUTION_URL));
        }
    }
}