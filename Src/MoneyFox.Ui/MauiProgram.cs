namespace MoneyFox.Ui;

using System.Reflection;
using CommunityToolkit.Maui;
using Controls;
using Core.Common;
using InversionOfControl;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Handlers;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Sharpnado.Tabs;
using SkiaSharp.Views.Maui.Controls.Hosting;

public static class MauiProgram
{
    [UsedImplicitly]
    public static MauiApp CreateMauiApp()
    {
        var configuration = GetConfiguration();
        SetupSerilog();
        var builder = MauiApp.CreateBuilder();
        builder.Configuration.AddConfiguration(configuration);
        builder.UseMauiApp<App>()
            .UseAptabase(configuration["Aptabase:Secret"] ?? string.Empty)
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
            .UseMauiCommunityToolkit()
            .UseSharpnadoTabs(loggerEnable: false)
            .AddMoneyFoxService();

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

    public static Action<IServiceCollection>? AddPlatformServicesAction { get; set; }

    private static void AddMoneyFoxService(this MauiAppBuilder builder)
    {
        AddPlatformServicesAction?.Invoke(builder.Services);
        new MoneyFoxConfig().Register(builder.Services);
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
#pragma warning disable S3928
            _ => throw new ArgumentOutOfRangeException()
#pragma warning restore S3928
        };
    }
#endif

    private static IConfigurationRoot GetConfiguration()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MoneyFox.Ui.appsettings.json");

        return stream == null ? throw new FileNotFoundException("'appsettings.json' was not found.") : new ConfigurationBuilder().AddJsonStream(stream).Build();
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
