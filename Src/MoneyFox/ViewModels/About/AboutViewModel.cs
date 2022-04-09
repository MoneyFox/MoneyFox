namespace MoneyFox.ViewModels.About
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using Core.Common;
    using Core.Common.Interfaces;
    using Core.Interfaces;
    using Core.Resources;
    using Xamarin.Essentials;

    public class AboutViewModel : ObservableObject
    {
        private const string WEBSITE_URL = "https://www.apply-solutions.ch";
        private const string SUPPORT_MAIL = "mobile.support@apply-solutions.ch";
        private const string GITHUB_PROJECT_URL = "https://github.com/MoneyFox/MoneyFox";
        private const string TRANSLATION_URL = "https://crowdin.com/project/money-fox";
        private const string ICON_DESIGNER_URL = "https://twitter.com/vandert9";
        private const string GITHUB_CONTRIBUTOR_URL = "https://github.com/MoneyFox/MoneyFox/graphs/contributors";

        private readonly IAppInformation appInformation;
        private readonly IBrowserAdapter browserAdapter;
        private readonly IEmailAdapter emailAdapter;
        private readonly IStoreOperations storeFeatures;

        public AboutViewModel(IAppInformation appInformation, IEmailAdapter emailAdapter, IBrowserAdapter browserAdapter, IStoreOperations storeOperations)
        {
            this.appInformation = appInformation;
            this.emailAdapter = emailAdapter;
            this.browserAdapter = browserAdapter;
            storeFeatures = storeOperations;
        }

        public AsyncRelayCommand GoToWebsiteCommand => new AsyncRelayCommand(async () => await GoToWebsiteAsync());

        public AsyncRelayCommand SendMailCommand => new AsyncRelayCommand(async () => await SendMailAsync());

        public RelayCommand RateAppCommand => new RelayCommand(RateApp);

        public AsyncRelayCommand GoToRepositoryCommand => new AsyncRelayCommand(async () => await GoToRepositoryAsync());

        public AsyncRelayCommand GoToTranslationProjectCommand => new AsyncRelayCommand(async () => await GoToTranslationProjectAsync());

        public AsyncRelayCommand GoToDesignerTwitterAccountCommand => new AsyncRelayCommand(async () => await GoToDesignerTwitterAccountAsync());

        public AsyncRelayCommand GoToContributionPageCommand => new AsyncRelayCommand(async () => await GoToContributionPageAsync());

        public string Version => appInformation.GetVersion;

        public string Website => WEBSITE_URL;

        public string SupportMail => SUPPORT_MAIL;

        private async Task GoToWebsiteAsync()
        {
            await browserAdapter.OpenWebsiteAsync(new Uri(WEBSITE_URL));
        }

        private async Task SendMailAsync()
        {
            await emailAdapter.SendEmailAsync(
                subject: Strings.FeedbackSubject,
                body: string.Empty,
                recipients: new List<string> { SUPPORT_MAIL },
                filePaths: new List<string> { Path.Combine(path1: FileSystem.AppDataDirectory, path2: LogConfiguration.FileName) });
        }

        private void RateApp()
        {
            storeFeatures.RateApp();
        }

        private async Task GoToRepositoryAsync()
        {
            await browserAdapter.OpenWebsiteAsync(new Uri(GITHUB_PROJECT_URL));
        }

        private async Task GoToTranslationProjectAsync()
        {
            await browserAdapter.OpenWebsiteAsync(new Uri(TRANSLATION_URL));
        }

        private async Task GoToDesignerTwitterAccountAsync()
        {
            await browserAdapter.OpenWebsiteAsync(new Uri(ICON_DESIGNER_URL));
        }

        private async Task GoToContributionPageAsync()
        {
            await browserAdapter.OpenWebsiteAsync(new Uri(GITHUB_CONTRIBUTOR_URL));
        }
    }
}
