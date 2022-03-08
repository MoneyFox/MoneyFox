namespace MoneyFox.ViewModels.About
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using Core._Pending_.Common.Constants;
    using Core.Common.Interfaces;
    using Core.Interfaces;
    using Core.Resources;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Xamarin.Essentials;

    public class
        AboutViewModel : ObservableObject
    {
        private readonly IAppInformation appInformation;
        private readonly IBrowserAdapter browserAdapter;
        private readonly IEmailAdapter emailAdapter;
        private readonly IStoreOperations storeFeatures;

        public AboutViewModel(
            IAppInformation appInformation,
            IEmailAdapter emailAdapter,
            IBrowserAdapter browserAdapter,
            IStoreOperations storeOperations)
        {
            this.appInformation = appInformation;
            this.emailAdapter = emailAdapter;
            this.browserAdapter = browserAdapter;
            storeFeatures = storeOperations;
        }

        public AsyncRelayCommand GoToWebsiteCommand => new AsyncRelayCommand(async () => await GoToWebsiteAsync());

        public AsyncRelayCommand SendMailCommand => new AsyncRelayCommand(async () => await SendMailAsync());

        public RelayCommand RateAppCommand => new RelayCommand(RateApp);

        public AsyncRelayCommand GoToRepositoryCommand =>
            new AsyncRelayCommand(async () => await GoToRepositoryAsync());

        public AsyncRelayCommand GoToTranslationProjectCommand =>
            new AsyncRelayCommand(async () => await GoToTranslationProjectAsync());

        public AsyncRelayCommand GoToDesignerTwitterAccountCommand =>
            new AsyncRelayCommand(async () => await GoToDesignerTwitterAccountAsync());

        public AsyncRelayCommand GoToContributionPageCommand =>
            new AsyncRelayCommand(async () => await GoToContributionPageAsync());

        public string Version => appInformation.GetVersion;

        public string Website => AppConstants.WebsiteUrl;

        public string SupportMail => AppConstants.SupportMail;

        private async Task GoToWebsiteAsync()
            => await browserAdapter.OpenWebsiteAsync(new Uri(AppConstants.WebsiteUrl));

        private async Task SendMailAsync() =>
            await emailAdapter.SendEmailAsync(
                Strings.FeedbackSubject,
                string.Empty,
                new List<string> { AppConstants.SupportMail },
                new List<string> { Path.Combine(FileSystem.CacheDirectory, AppConstants.LogFileName) });

        private void RateApp()
            => storeFeatures.RateApp();

        private async Task GoToRepositoryAsync()
            => await browserAdapter.OpenWebsiteAsync(new Uri(AppConstants.GitHubRepositoryUrl));

        private async Task GoToTranslationProjectAsync()
            => await browserAdapter.OpenWebsiteAsync(new Uri(AppConstants.TranslationProjectUrl));

        private async Task GoToDesignerTwitterAccountAsync()
            => await browserAdapter.OpenWebsiteAsync(new Uri(AppConstants.IconDesignerTwitterUrl));

        private async Task GoToContributionPageAsync()
            => await browserAdapter.OpenWebsiteAsync(new Uri(AppConstants.GithubContributionUrl));
    }
}