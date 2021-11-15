﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.Constants;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyFox.Uwp.ViewModels.About
{
    public class AboutViewModel : ObservableObject, IAboutViewModel
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
        ///     Opens the web browser and loads to the apply solutions     website
        /// </summary>
        public RelayCommand GoToWebsiteCommand => new RelayCommand(async () => await GoToWebsiteAsync());

        /// <summary>
        ///     Sends a feedback mail to the apply solutions support     mail address
        /// </summary>
        public RelayCommand SendMailCommand => new RelayCommand(async () => await SendMailAsync());

        /// <summary>
        ///     Opens the store to rate the app.
        /// </summary>
        public RelayCommand RateAppCommand => new RelayCommand(RateApp);

        /// <summary>
        ///     Opens the web browser and loads repository page     on GitHub
        /// </summary>
        public RelayCommand GoToRepositoryCommand => new RelayCommand(async () => await GoToRepositoryAsync());

        /// <summary>
        ///     Opens the web browser and loads the project on Crowdin.
        /// </summary>
        public RelayCommand GoToTranslationProjectCommand
            => new RelayCommand(async () => await GoToTranslationProjectAsync());

        /// <summary>
        ///     Opens the webbrowser and loads the project on crowdin.
        /// </summary>
        public RelayCommand GoToDesignerTwitterAccountCommand
            => new RelayCommand(async () => await GoToDesignerTwitterAccountAsync());

        /// <summary>
        ///     Opens the webbrowser loads the contribution page on Github.
        /// </summary>
        public RelayCommand GoToContributionPageCommand
            => new RelayCommand(async () => await GoToContributionPageAsync());

        /// <summary>
        ///     Returns the Version of App
        /// </summary>
        public string Version => appInformation.GetVersion;

        /// <summary>
        ///     Returns the apply solutions webite url from the     resource file
        /// </summary>
        public string Website => AppConstants.WebsiteUrl;

        /// <summary>
        ///     Returns the mailaddress for support cases from the     resource file
        /// </summary>
        public string SupportMail => AppConstants.SupportMail;

        private async Task GoToWebsiteAsync()
            => await browserAdapter.OpenWebsiteAsync(new Uri(AppConstants.WebsiteUrl));

        private async Task SendMailAsync() => await emailAdapter.SendEmailAsync(
            Strings.FeedbackSubject,
            string.Empty,
            new List<string> {AppConstants.SupportMail},
            new List<string> {AppConstants.LogFileName});

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