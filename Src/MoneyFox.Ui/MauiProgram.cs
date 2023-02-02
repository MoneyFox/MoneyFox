namespace MoneyFox.Ui;

using System.Reflection;
using CommunityToolkit.Maui;
using Controls;
using Core.Common;
using JetBrains.Annotations;
using Microsoft.AppCenter;
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
    [UsedImplicitly]
    public static MauiApp CreateMauiApp()
    {
        var configuration = GetConfiguration();
        SetupSerilog();
        SetupAppCenter(configuration);
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
            .AddCustomAppShellHandler()
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
                    handler.PlatformView.BorderThickness = new(0);
#endif
                }
            });

#if WINDOWS
        PickerHandler.Mapper.Add(nameof(View.HorizontalOptions), MapHorizontalOptions);
#endif
        return builder.Build();
    }

    private static MauiAppBuilder AddCustomAppShellHandler(this MauiAppBuilder builder)
    {
#if IOS
        builder.ConfigureMauiHandlers(handlers => { handlers.AddHandler(viewType: typeof(Shell), handlerType: typeof(Platforms.iOS.Renderer.CustomShellRenderer)); });
#endif
        return builder;
    }

#if WINDOWS
    private static void MapHorizontalOptions(IViewHandler handler, IView view)
    {
        if (view is not View mauiView)
        {
            return;
        }

        if (handler.PlatformView is not Microsoft.UI.Xaml.FrameworkElement element)
        {
            return;
        }

        element.HorizontalAlignment = mauiView.HorizontalOptions.Alignment switch
        {
            LayoutAlignment.Start  => Microsoft.UI.Xaml.HorizontalAlignment.Left,
            LayoutAlignment.Center => Microsoft.UI.Xaml.HorizontalAlignment.Center,
            LayoutAlignment.End    => Microsoft.UI.Xaml.HorizontalAlignment.Right,
            LayoutAlignment.Fill   => Microsoft.UI.Xaml.HorizontalAlignment.Stretch,
            _ => throw new ArgumentOutOfRangeException(nameof(element))
        };
    }
#endif

    private static IConfigurationRoot GetConfiguration()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MoneyFox.Ui.appsettings.json");

        return stream == null ? throw new FileNotFoundException("'appsettings.json' was not found.") : new ConfigurationBuilder().AddJsonStream(stream).Build();
    }

    private static void SetupAppCenter(IConfiguration configuration)
    {
        Crashes.GetErrorAttachments = _ =>
        {
            var logFile = LogFileService.GetLatestLogFileInfo();
            if (logFile == null)
            {
                return Array.Empty<ErrorAttachmentLog>();
            }

            try
            {
                using var logFileStream = new FileStream(path: logFile.FullName, mode: FileMode.Open, access: FileAccess.Read, share: FileShare.ReadWrite);
                using var logFileReader = new StreamReader(logFileStream);
                var logText = logFileReader.ReadToEnd();

                return new[] { ErrorAttachmentLog.AttachmentWithText(text: logText, fileName: logFile.Name) };
            }
            catch (Exception)
            {
                // Don't block the normal exception upload.
            }

            return Array.Empty<ErrorAttachmentLog>();
        };

        var appCenter = configuration.GetRequiredSection("AppCenter").Get<AppCenterOption>()!;
        AppCenter.Start(
            appSecret: $"android={appCenter.AndroidSecret};" + $"windowsdesktop={appCenter.WindowsSecret};" + $"ios={appCenter.IosSecret};",
            typeof(Analytics),
            typeof(Crashes));
    }

    private static void SetupSerilog()
    {
        var logFile = Path.Combine(path1: FileSystem.AppDataDirectory, path2: LogConfiguration.FileName);
        Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.File(
                path: logFile,
                rollingInterval: RollingInterval.Month,
                retainedFileCountLimit: 6,
                restrictedToMinimumLevel: LogEventLevel.Information,
                shared: true)
            .CreateLogger();

        Log.Information("Application Startup");
    }
}
