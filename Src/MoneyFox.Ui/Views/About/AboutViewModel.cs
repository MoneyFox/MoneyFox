namespace MoneyFox.Ui.Views.About;

using CommunityToolkit.Mvvm.Input;
using Core.Common;
using Core.Common.Interfaces;
using Core.Interfaces;
using Core.Resources;
using ViewModels;

internal class AboutViewModel : BaseViewModel
{
    private readonly Uri WEBSITE_URI = new("https://www.apply-solutions.ch", UriKind.Absolute);
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
        await emailAdapter.SendEmailAsync(
            subject: Strings.FeedbackSubject,
            body: string.Empty,
            recipients: new() { SUPPORT_MAIL },
            filePaths: new() { Path.Combine(path1: FileSystem.AppDataDirectory, path2: LogConfiguration.FileName) });
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
        var logFilePaths = Directory.GetFiles(path: FileSystem.AppDataDirectory, searchPattern: "moneyfox*").OrderByDescending(x => x);
        var latestLogFile = logFilePaths.Select(logFilePath => new FileInfo(logFilePath)).OrderByDescending(fi => fi.LastWriteTime).FirstOrDefault();
        if (latestLogFile != null)
        {
            await Launcher.OpenAsync(new OpenFileRequest { File = new(latestLogFile.FullName) });
        }
    }
}



