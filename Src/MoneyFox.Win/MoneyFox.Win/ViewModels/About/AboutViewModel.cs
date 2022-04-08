namespace MoneyFox.Win.ViewModels.About;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Interfaces;
using Core.Interfaces;
using Core.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Common;

public class AboutViewModel : ObservableObject, IAboutViewModel
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

    public RelayCommand GoToWebsiteCommand => new(async () => await GoToWebsiteAsync());

    public RelayCommand SendMailCommand => new(async () => await SendMailAsync());

    public RelayCommand RateAppCommand => new(RateApp);

    public RelayCommand GoToRepositoryCommand => new(async () => await GoToRepositoryAsync());

    public RelayCommand GoToTranslationProjectCommand
        => new(async () => await GoToTranslationProjectAsync());

    public RelayCommand GoToDesignerTwitterAccountCommand
        => new(async () => await GoToDesignerTwitterAccountAsync());

    public RelayCommand GoToContributionPageCommand
        => new(async () => await GoToContributionPageAsync());

    public string Version => appInformation.GetVersion;

    public string Website => WEBSITE_URL;

    public string SupportMail => SUPPORT_MAIL;

    private async Task GoToWebsiteAsync()
        => await browserAdapter.OpenWebsiteAsync(new Uri(WEBSITE_URL));

    private async Task SendMailAsync() => await emailAdapter.SendEmailAsync(
        Strings.FeedbackSubject,
        string.Empty,
        new List<string> { SUPPORT_MAIL },
        new List<string> { LogConfiguration.FileName });

    private void RateApp()
        => storeFeatures.RateApp();

    private async Task GoToRepositoryAsync()
        => await browserAdapter.OpenWebsiteAsync(new Uri(GITHUB_PROJECT_URL));

    private async Task GoToTranslationProjectAsync()
        => await browserAdapter.OpenWebsiteAsync(new Uri(TRANSLATION_URL));

    private async Task GoToDesignerTwitterAccountAsync()
        => await browserAdapter.OpenWebsiteAsync(new Uri(ICON_DESIGNER_URL));

    private async Task GoToContributionPageAsync()
        => await browserAdapter.OpenWebsiteAsync(new Uri(GITHUB_CONTRIBUTOR_URL));
}