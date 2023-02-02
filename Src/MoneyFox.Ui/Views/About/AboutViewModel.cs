namespace MoneyFox.Ui.Views.About;

using CommunityToolkit.Mvvm.Input;
using Core.Common.Interfaces;
using Core.Interfaces;
using Plugin.StoreReview;
using Resources.Strings;

// ReSharper disable once PartialTypeWithSinglePart
public partial class AboutViewModel : BasePageViewModel
{
    private const string SUPPORT_MAIL = "mobile.support@apply-solutions.ch";
    private static readonly Uri projectUri = new("https://github.com/MoneyFox/MoneyFox");
    private static readonly Uri translationUri = new("https://crowdin.com/project/money-fox");
    private static readonly Uri iconDesignerUrl = new("https://twitter.com/vandert9");
    private static readonly Uri contributorUrl = new("https://github.com/MoneyFox/MoneyFox/graphs/contributors");
    private static readonly Uri websiteUri = new(uriString: "https://www.apply-solutions.ch", uriKind: UriKind.Absolute);

    private readonly IBrowserAdapter browserAdapter;
    private readonly IEmailAdapter emailAdapter;
    private readonly IToastService toastService;

    public AboutViewModel(IEmailAdapter emailAdapter, IBrowserAdapter browserAdapter, IToastService toastService)
    {
        this.emailAdapter = emailAdapter;
        this.browserAdapter = browserAdapter;
        this.toastService = toastService;
    }

    public static string Version => AppInfo.VersionString;

    [RelayCommand]
    private async Task GoToWebsiteAsync()
    {
        await browserAdapter.OpenWebsiteAsync(websiteUri);
    }

    [RelayCommand]
    private async Task SendMailAsync()
    {
        try
        {
            var latestLogFile = LogFileService.GetLatestLogFileInfo();
            await emailAdapter.SendEmailAsync(
                subject: Translations.FeedbackSubject,
                body: string.Empty,
                recipients: new() { SUPPORT_MAIL },
                filePaths: latestLogFile != null ? new() { latestLogFile.FullName } : new());
        }
        catch (Exception)
        {
            await toastService.ShowToastAsync(Translations.SendEmailFailedMessage);
        }
    }

    [RelayCommand]
    private async Task RateApp()
    {
        await CrossStoreReview.Current.RequestReview(false);
    }

    [RelayCommand]
    private async Task GoToRepositoryAsync()
    {
        await browserAdapter.OpenWebsiteAsync(projectUri);
    }

    [RelayCommand]
    private async Task GoToTranslationProjectAsync()
    {
        await browserAdapter.OpenWebsiteAsync(translationUri);
    }

    [RelayCommand]
    private async Task GoToDesignerTwitterAccountAsync()
    {
        await browserAdapter.OpenWebsiteAsync(iconDesignerUrl);
    }

    [RelayCommand]
    private async Task GoToContributionPageAsync()
    {
        await browserAdapter.OpenWebsiteAsync(contributorUrl);
    }

    [RelayCommand]
    private async Task OpenLogFile()
    {
        var latestLogFile = LogFileService.GetLatestLogFileInfo();
        if (latestLogFile != null)
        {
            await Launcher.OpenAsync(new OpenFileRequest { File = new(latestLogFile.FullName) });
        }
    }
}
