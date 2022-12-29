namespace MoneyFox.Ui;

using System.Reflection;
using CommunityToolkit.Maui;
using Controls;
using Core.Common;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Handlers;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using SkiaSharp.Views.Maui.Controls.Hosting;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MoneyFox.Ui.appsettings.json");
        var configuration = new ConfigurationBuilder().AddJsonStream(stream).Build();
        InitAppCenter(configuration);
        InitLogger();
        var builder = MauiApp.CreateBuilder();
        builder.Configuration.AddConfiguration(configuration);
        builder.UseMauiApp<App>()
            .ConfigureFonts(
                fonts =>
                {
                    fonts.AddFont(filename: "OpenSans-Regular.ttf", alias: "OpenSansRegular");
                    fonts.AddFont(filename: "OpenSans-Semibold.ttf", alias: "OpenSansSemibold");
                    fonts.AddFont(filename: "ProductSans-Regular.ttf", alias: "Product");
                    fonts.AddFont(filename: "materialdesignicons.ttf", alias: "MaterialIcons");
                })
            .ConfigureMauiHandlers(
                handlers =>
                {
#if IOS
                    handlers.AddHandler(viewType: typeof(Shell), handlerType: typeof(Platforms.iOS.Renderer.CustomShellRenderer));
#endif
                })
            .UseSkiaSharp(true)
            .UseMauiCommunityToolkit();

        EntryHandler.Mapper.AppendToMapping(
            key: "Borderless",
            method: (handler, view) =>
            {
                if (view is BorderlessEntry)
                {
#if ANDROID
                handler.PlatformView.Background = null;
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif IOS
                    handler.PlatformView.Layer.BorderWidth = 0;
                    handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
                }
            });

        return builder.Build();
    }

    private static void InitAppCenter(IConfiguration configuration)
    {
        var appCenter = configuration.GetRequiredSection("AppCenter").Get<AppCenter>()!;
        Microsoft.AppCenter.AppCenter.Start(
            appSecret: $"android={appCenter.AndroidSecret};" + $"uwp={appCenter.WindowsSecret};" + $"ios={appCenter.IosSecret};",
            typeof(Analytics),
            typeof(Crashes));
    }

    private static void InitLogger()
    {
        var logFile = Path.Combine(path1: FileSystem.AppDataDirectory, path2: LogConfiguration.FileName);
        Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.File(path: logFile, rollingInterval: RollingInterval.Month, retainedFileCountLimit: 6, restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();

        Log.Information("Application Startup");
    }
}
