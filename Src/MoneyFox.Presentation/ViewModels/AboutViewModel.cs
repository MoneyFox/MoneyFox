using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Application.Constants;
using MoneyFox.Application.Resources;
using MoneyFox.BusinessLogic.Adapters;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.Interfaces;

namespace MoneyFox.Presentation.ViewModels
{
    public interface IAboutViewModel : IBaseViewModel
    {
        /// <summary>
        ///     Opens the webbrowser and loads to the apply solutions
        ///     website
        /// </summary>
        AsyncCommand GoToWebsiteCommand { get; }

        /// <summary>
        ///     Sends a feedback mail to the apply solutions support
        ///     mail address
        /// </summary>
        AsyncCommand SendMailCommand { get; }

        /// <summary>
        ///     Opens the store to rate the app.
        /// </summary>
        RelayCommand RateAppCommand { get; }

        /// <summary>
        ///     Opens the webbrowser and loads repository page
        ///     on GitHub
        /// </summary>
        AsyncCommand GoToRepositoryCommand { get; }

        /// <summary>
        ///     Opens the webbrowser and loads the project on crowdin.
        /// </summary>
        AsyncCommand GoToTranslationProjectCommand { get; }

        /// <summary>
        ///     Opens the webbrowser and loads the project on crowdin.
        /// </summary>
        AsyncCommand GoToDesignerTwitterAccountCommand { get; }

        /// <summary>
        ///     Opens the webbrowser loads the contribution page on Github.
        /// </summary>
        AsyncCommand GoToContributionPageCommand { get; }

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
        public AsyncCommand GoToWebsiteCommand => new AsyncCommand(GoToWebsite);

        /// <summary>
        ///     Sends a feedback mail to the apply solutions support
        ///     mail address
        /// </summary>
        public AsyncCommand SendMailCommand => new AsyncCommand(SendMail);

        /// <summary>
        ///     Opens the store to rate the app.
        /// </summary>
        public RelayCommand RateAppCommand => new RelayCommand(RateApp);

        /// <summary>
        ///     Opens the webbrowser and loads repository page
        ///     on GitHub
        /// </summary>
        public AsyncCommand GoToRepositoryCommand => new AsyncCommand(GoToRepository);

        /// <summary>
        ///     Opens the webbrowser and loads the project on crowdin.
        /// </summary>
        public AsyncCommand GoToTranslationProjectCommand => new AsyncCommand(GoToTranslationProject);

        /// <summary>
        ///     Opens the webbrowser and loads the project on crowdin.
        /// </summary>
        public AsyncCommand GoToDesignerTwitterAccountCommand => new AsyncCommand(GoToDesignerTwitterAccount);

        /// <summary>
        ///     Opens the webbrowser loads the contribution page on Github.
        /// </summary>
        public AsyncCommand GoToContributionPageCommand => new AsyncCommand(GoToContributionPage);

        /// <summary>
        ///     Returns the Version of App
        /// </summary>
        public string Version => appInformation.GetVersion();

        /// <summary>
        ///     Returns the apply solutions webite url from the
        ///     resource file
        /// </summary>
        public string Website => AppConstants.WebsiteUrl;

        /// <summary>
        ///     Returns the mailaddress for support cases from the
        ///     resource file
        /// </summary>
        public string SupportMail => AppConstants.SupportMail;

        private async Task GoToWebsite()
        {
            await browserAdapter.OpenWebsiteAsync(new Uri(AppConstants.WebsiteUrl));
        }

        private async Task SendMail()
        {
            await emailAdapter.SendEmail(Strings.FeedbackSubject, string.Empty,
                                         new List<string> {AppConstants.SupportMail});
        }

        private void RateApp()
        {
            storeFeatures.RateApp();
        }

        private async Task GoToRepository()
        {
            await browserAdapter.OpenWebsiteAsync(new Uri(AppConstants.GitHubRepositoryUrl));
        }

        private async Task GoToTranslationProject()
        {
            await browserAdapter.OpenWebsiteAsync(new Uri(AppConstants.TranslationProjectUrl));
        }

        private async Task GoToDesignerTwitterAccount()
        {
            await browserAdapter.OpenWebsiteAsync(new Uri(AppConstants.IconDesignerTwitterUrl));
        }

        private async Task GoToContributionPage()
        {
            await browserAdapter.OpenWebsiteAsync(new Uri(AppConstants.GithubContributionUrl));
        }
    }
}
