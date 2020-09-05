using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.Constants;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MoneyFox.Ui.Shared.ViewModels.About
{
    public class AboutViewModel : ViewModelBase, IAboutViewModel
    {
        private readonly IAppInformation appInformation;
        private readonly IBrowserAdapter browserAdapter;
        private readonly IEmailAdapter emailAdapter;
        private readonly IStoreOperations storeFeatures;

        /// <summary>
        /// Creates an AboutViewModel Object
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
        /// Opens the webbrowser and loads to the apply solutions     website
        /// </summary>
        public RelayCommand GoToWebsiteCommand => new RelayCommand(async () => await GoToWebsite());

        /// <summary>
        /// Sends a feedback mail to the apply solutions support     mail address
        /// </summary>
        public RelayCommand SendMailCommand => new RelayCommand(async () => await SendMail());

        /// <summary>
        /// Opens the store to rate the app.
        /// </summary>
        public RelayCommand RateAppCommand => new RelayCommand(RateApp);

        /// <summary>
        /// Opens the webbrowser and loads repository page     on GitHub
        /// </summary>
        public RelayCommand GoToRepositoryCommand => new RelayCommand(async () => await GoToRepository());

        /// <summary>
        /// Opens the webbrowser and loads the project on crowdin.
        /// </summary>
        public RelayCommand GoToTranslationProjectCommand => new RelayCommand(async () => await GoToTranslationProject());

        /// <summary>
        /// Opens the webbrowser and loads the project on crowdin.
        /// </summary>
        public RelayCommand GoToDesignerTwitterAccountCommand => new RelayCommand(async () => await GoToDesignerTwitterAccount());

        /// <summary>
        /// Opens the webbrowser loads the contribution page on Github.
        /// </summary>
        public RelayCommand GoToContributionPageCommand => new RelayCommand(async () => await GoToContributionPage());

        /// <summary>
        /// Returns the Version of App
        /// </summary>
        public string Version => appInformation.GetVersion;

        /// <summary>
        /// Returns the apply solutions webite url from the     resource file
        /// </summary>
        public string Website => AppConstants.WebsiteUrl;

        /// <summary>
        /// Returns the mailaddress for support cases from the     resource file
        /// </summary>
        public string SupportMail => AppConstants.SupportMail;

        private async Task GoToWebsite()
            => await browserAdapter.OpenWebsiteAsync(new Uri(AppConstants.WebsiteUrl));

        private async Task SendMail()
        {
            await emailAdapter.SendEmailAsync(Strings.FeedbackSubject,
                                              string.Empty,
                                              new List<string>
                                              { AppConstants.SupportMail },
                                              new List<string>
                                              { Path.Combine(FileSystem.CacheDirectory, AppConstants.LogFileName) });
        }

        private void RateApp()
            => storeFeatures.RateApp();

        private async Task GoToRepository()
            => await browserAdapter.OpenWebsiteAsync(new Uri(AppConstants.GitHubRepositoryUrl));

        private async Task GoToTranslationProject()
            => await browserAdapter.OpenWebsiteAsync(new Uri(AppConstants.TranslationProjectUrl));

        private async Task GoToDesignerTwitterAccount()
            => await browserAdapter.OpenWebsiteAsync(new Uri(AppConstants.IconDesignerTwitterUrl));

        private async Task GoToContributionPage()
            => await browserAdapter.OpenWebsiteAsync(new Uri(AppConstants.GithubContributionUrl));
    }
}
