namespace MoneyFox.Ui;

using CommunityToolkit.Maui;
using Microsoft.Maui.Handlers;
using MoneyFox.Ui.Controls;
using SkiaSharp.Views.Maui.Controls.Hosting;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>()
            .ConfigureFonts(
                fonts =>
                {
                    fonts.AddFont(filename: "OpenSans-Regular.ttf", alias: "OpenSansRegular");
                    fonts.AddFont(filename: "OpenSans-Semibold.ttf", alias: "OpenSansSemibold");
                    fonts.AddFont(filename: "ProductSans-Regular.ttf", alias: "Product");
                    fonts.AddFont(filename: "MaterialIconsRound-Regular.otf", alias: "MaterialIconsRound");
                    fonts.AddFont(filename: "materialdesignicons.ttf", alias: "MaterialIcons");
                })
            .ConfigureMauiHandlers(handlers =>
            {
#if IOS
                handlers.AddHandler(typeof(Shell), typeof(Platforms.iOS.Renderer.CustomShellRenderer));
#endif
            })
            .UseSkiaSharp(true)
            .UseMauiCommunityToolkit();

        EntryHandler.Mapper.AppendToMapping("Borderless", (handler, view) =>
        {
            if (view is BorderlessEntry)
            {
#if IOS
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;

#elif WINDOWS
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            }
        });

        return builder.Build();
    }
}
