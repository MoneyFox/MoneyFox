namespace MoneyFox.Ui.Views.About;

using CommunityToolkit.Mvvm.Input;
using Core.Common.Interfaces;
using Core.Interfaces;
using Plugin.StoreReview;
using Resources.Strings;

public class AboutViewModel : BasePageViewModel
{
    private const string SUPPORT_MAIL = "mobile.support@apply-solutions.ch";

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

    public AsyncRelayCommand SendMailCommand => new(SendMailAsync);
    public AsyncRelayCommand RateAppCommand => new(RateAppAsync);
    public AsyncRelayCommand OpenLogFileCommand => new(OpenLogFile);
    public AsyncRelayCommand<string> OpenUrlCommand => new(url => browserAdapter.OpenWebsiteAsync(new(url ?? string.Empty)));

    public List<LicenseViewModel> Licenses
        => new()
        {
            new(Name: ".net MAUI", ProjectUrl: "https://github.com/dotnet/maui", License: "MIT"),
            new(Name: "MAUI Community ToolkitProjectUrl", ProjectUrl: "https://github.com/dotnet/maui", License: "MIT"),
            new(Name: "LiveChartsCore.SkiaSharpView", ProjectUrl: "https://github.com/beto-rodriguez/LiveCharts2", License: "MIT"),
            new(Name: "Plugin.StoreReviewProjectUrl", ProjectUrl: "https://github.com/jamesmontemagno/StoreReviewPlugin", License: "MIT"),
            new(Name: "Serilog", ProjectUrl: "", License: ""),
            new(Name: "Serilog.Exceptions", ProjectUrl: "", License: ""),
            new(Name: "Serilog.Sinks.File", ProjectUrl: "", License: ""),
            new(Name: "Microsoft.Extensions.Configuration.Json", ProjectUrl: "https://github.com/dotnet/runtime", License: "MIT"),
            new(Name: "Microsoft.Extensions.Configuration.Binder", ProjectUrl: "https://github.com/dotnet/runtime", License: "MIT"),
            new(Name: "SonarAnalyzer.CSharp", ProjectUrl: "https://github.com/SonarSource/sonar-dotnet", License: "LGPL-3.0"),
            new(Name: "MediatR", ProjectUrl: "https://github.com/jbogard/MediatR", License: "Apache-2.0"),
            new(Name: "SQLitePCLRaw.bundle_e_sqlite3", ProjectUrl: "https://github.com/ericsink/SQLitePCL.raw", License: "Apache-2.0"),
            new(Name: "Microsoft.EntityFrameworkCore", ProjectUrl: "https://github.com/dotnet/efcore", License: "MIT"),
            new(Name: "AutoMapper", ProjectUrl: "", License: ""),
            new(Name: "JetBrains.Annotations", ProjectUrl: "", License: ""),
            new(Name: "Newtonsoft.Json", ProjectUrl: "", License: ""),
            new(Name: "Flurl.Http", ProjectUrl: "", License: ""),
            new(Name: "Microsoft.Identity.Client", ProjectUrl: "", License: ""),
            new(Name: "Microsoft.Extensions.DependencyInjection", ProjectUrl: "", License: ""),
        };

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

    private async Task RateAppAsync()
    {
        await CrossStoreReview.Current.RequestReview(false);
    }

    private async Task OpenLogFile()
    {
        var latestLogFile = LogFileService.GetLatestLogFileInfo();
        if (latestLogFile != null)
        {
            await Launcher.OpenAsync(new OpenFileRequest { File = new(latestLogFile.FullName) });
        }
    }
}

public record LicenseViewModel(string Name, string ProjectUrl, string License);
