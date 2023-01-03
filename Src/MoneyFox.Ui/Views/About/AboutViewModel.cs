namespace MoneyFox.Ui.Views.About;

using CommunityToolkit.Mvvm.Input;
using Core.Common.Interfaces;
using Core.Interfaces;
using Resources.Strings;
using ViewModels;

internal class AboutViewModel : BaseViewModel
{
    private const string SUPPORT_MAIL = "mobile.support@apply-solutions.ch";
    private const string GITHUB_PROJECT_URL = "https://github.com/MoneyFox/MoneyFox";
    private const string TRANSLATION_URL = "https://crowdin.com/project/money-fox";
    private const string ICON_DESIGNER_URL = "https://twitter.com/vandert9";
    private const string GITHUB_CONTRIBUTOR_URL = "https://github.com/MoneyFox/MoneyFox/graphs/contributors";

    private readonly IAppInformation appInformation;
    private readonly IBrowserAdapter browserAdapter;
    private readonly IEmailAdapter emailAdapter;
    private readonly IStoreOperations storeFeatures;
    private readonly Uri WEBSITE_URI = new(uriString: "https://www.apply-solutions.ch", uriKind: UriKind.Absolute);

    public AboutViewModel(IAppInformation appInformation, IEmailAdapter emailAdapter, IBrowserAdapter browserAdapter, IStoreOperations storeOperations)
    {
        this.appInformation = appInformation;
        this.emailAdapter = emailAdapter;
        this.browserAdapter = browserAdapter;
        storeFeatures = storeOperations;
    }

    public AsyncRelayCommand GoToWebsiteCommand => new(async () => await GoToWebsiteAsync());

    public AsyncRelayCommand SendMailCommand => new(async () => await SendMailAsync());

    public RelayCommand RateAppCommand => new(RateApp);

    public AsyncRelayCommand GoToRepositoryCommand => new(async () => await GoToRepositoryAsync());

    public AsyncRelayCommand GoToTranslationProjectCommand => new(async () => await GoToTranslationProjectAsync());

    public AsyncRelayCommand GoToDesignerTwitterAccountCommand => new(async () => await GoToDesignerTwitterAccountAsync());

    public AsyncRelayCommand GoToContributionPageCommand => new(async () => await GoToContributionPageAsync());

    public AsyncRelayCommand OpenLogFileCommand => new(async () => await OpenLogFile());

    public string Version => appInformation.GetVersion;

    private async Task GoToWebsiteAsync()
    {
        await browserAdapter.OpenWebsiteAsync(WEBSITE_URI);
    }

    private async Task SendMailAsync()
    {
        var latestLogFile = GetLatestLogFile();
        await emailAdapter.SendEmailAsync(
            subject: Translations.FeedbackSubject,
            body: string.Empty,
            recipients: new() { SUPPORT_MAIL },
            filePaths: latestLogFile != null ? new() { latestLogFile.FullName } : new());
    }

    private void RateApp()
    {
        storeFeatures.RateApp();
    }

    private async Task GoToRepositoryAsync()
    {
        await browserAdapter.OpenWebsiteAsync(new(GITHUB_PROJECT_URL));
    }

    private async Task GoToTranslationProjectAsync()
    {
        await browserAdapter.OpenWebsiteAsync(new(TRANSLATION_URL));
    }

    private async Task GoToDesignerTwitterAccountAsync()
    {
        await browserAdapter.OpenWebsiteAsync(new(ICON_DESIGNER_URL));
    }

    private async Task GoToContributionPageAsync()
    {
        await browserAdapter.OpenWebsiteAsync(new(GITHUB_CONTRIBUTOR_URL));
    }

    private async Task OpenLogFile()
    {
        var latestLogFile = GetLatestLogFile();
        if (latestLogFile != null)
        {
            await Launcher.OpenAsync(new OpenFileRequest { File = new(latestLogFile.FullName) });
        }
    }

    private static FileInfo? GetLatestLogFile()
    {
        var logFilePaths = Directory.GetFiles(path: FileSystem.AppDataDirectory, searchPattern: "moneyfox*").OrderByDescending(x => x);
        var latestLogFile = logFilePaths.Select(logFilePath => new FileInfo(logFilePath)).MaxBy(fi => fi.LastWriteTime);

        return latestLogFile;
    }
}
