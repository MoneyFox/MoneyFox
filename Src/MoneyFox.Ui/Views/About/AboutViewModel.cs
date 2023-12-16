namespace MoneyFox.Ui.Views.About;

using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Interfaces;
using Core.Interfaces;
using Plugin.StoreReview;
using Resources.Strings;

public class AboutViewModel(IEmailAdapter emailAdapter, IBrowserAdapter adapter, IToastService service) : NavigableViewModel
{
    private const string SUPPORT_MAIL = "mobile.support@apply-solutions.ch";

    public static string Version => AppInfo.VersionString;

    public AsyncRelayCommand SendMailCommand => new(SendMailAsync);
    public AsyncRelayCommand RateAppCommand => new(RateAppAsync);
    public AsyncRelayCommand OpenLogFileCommand => new(OpenLogFile);
    public AsyncRelayCommand<string> OpenUrlCommand => new(url => adapter.OpenWebsiteAsync(new(url ?? string.Empty)));

    public List<LicenseViewModel> Licenses
        => new()
        {
            new(Name: ".net MAUI", ProjectUrl: "https://github.com/dotnet/maui", License: "MIT"),
            new(Name: "MAUI Community ToolkitProjectUrl", ProjectUrl: "https://github.com/dotnet/maui", License: "MIT"),
            new(Name: "LiveChartsCore.SkiaSharpView", ProjectUrl: "https://github.com/beto-rodriguez/LiveCharts2", License: "MIT"),
            new(Name: "Plugin.StoreReviewProjectUrl", ProjectUrl: "https://github.com/jamesmontemagno/StoreReviewPlugin", License: "MIT"),
            new(Name: "Serilog", ProjectUrl: "https://github.com/serilog/serilog", License: "Apache-2.0"),
            new(Name: "Serilog.Exceptions", ProjectUrl: "https://github.com/RehanSaeed/Serilog.Exceptions", License: "MIT"),
            new(Name: "Serilog.Sinks.File", ProjectUrl: "https://github.com/serilog/serilog-sinks-file", License: "Apache-2.0"),
            new(Name: "Microsoft.Extensions.Configuration.Json", ProjectUrl: "https://github.com/dotnet/runtime", License: "MIT"),
            new(Name: "Microsoft.Extensions.Configuration.Binder", ProjectUrl: "https://github.com/dotnet/runtime", License: "MIT"),
            new(Name: "SonarAnalyzer.CSharp", ProjectUrl: "https://github.com/SonarSource/sonar-dotnet", License: "LGPL-3.0"),
            new(Name: "MediatR", ProjectUrl: "https://github.com/jbogard/MediatR", License: "Apache-2.0"),
            new(Name: "SQLitePCLRaw.bundle_e_sqlite3", ProjectUrl: "https://github.com/ericsink/SQLitePCL.raw", License: "Apache-2.0"),
            new(Name: "Microsoft.EntityFrameworkCore", ProjectUrl: "https://github.com/dotnet/efcore", License: "MIT"),
            new(Name: "AutoMapper", ProjectUrl: "https://github.com/AutoMapper/AutoMapper", License: "MIT"),
            new(Name: "JetBrains.Annotations", ProjectUrl: "https://github.com/JetBrains/JetBrains.Annotations", License: "MIT"),
            new(Name: "Newtonsoft.Json", ProjectUrl: "https://github.com/JamesNK/Newtonsoft.Json", License: "MIT"),
            new(Name: "Flurl.Http", ProjectUrl: "https://github.com/tmenier/Flurl", License: "MIT"),
            new(Name: "Microsoft.Identity.Client", ProjectUrl: "https://github.com/AzureAD/microsoft-authentication-library-for-dotnet", License: "MIT"),
            new(Name: "Microsoft.Extensions.DependencyInjection", ProjectUrl: "https://github.com/dotnet/runtime", License: "MIT"),
            new(Name: "Microsoft.NET.Test.Sdk", ProjectUrl: "https://github.com/dotnet/runtime", License: "MIT"),
            new(Name: "xunit", ProjectUrl: "https://github.com/xunit/xunit", License: "Apache-2.0"),
            new(Name: "xunit.runner.visualstudio", ProjectUrl: "https://github.com/xunit/visualstudio.xunit", License: "Apache-2.0"),
            new(Name: "FluentAssertions", ProjectUrl: "https://github.com/fluentassertions/fluentassertions", License: "Apache-2.0"),
            new(Name: "FluentAssertions.Analyzers", ProjectUrl: "https://github.com/fluentassertions/fluentassertions.analyzers", License: "MIT"),
            new(Name: "NSubstitute", ProjectUrl: "https://github.com/nsubstitute/NSubstitute", License: "BSD"),
            new(Name: "NSubstitute.Analyzers.CSharp", ProjectUrl: "https://github.com/nsubstitute/NSubstitute.Analyzers", License: "MIT"),
            new(Name: "Sharpnado.Tabs", ProjectUrl: "https://github.com/roubachof/Sharpnado.Tabs", License: "MIT")
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
            await service.ShowToastAsync(Translations.SendEmailFailedMessage);
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
