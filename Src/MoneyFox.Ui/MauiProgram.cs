namespace MoneyFox.Ui;

using CommunityToolkit.Maui;
using SkiaSharp.Views.Maui.Controls.Hosting;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont(filename: "ProductSans-Regular.ttf", alias: "Product");
                fonts.AddFont(filename: "MaterialIconsRound-Regular.otf", alias: "MaterialIconsRound");
            })
            .UseSkiaSharp(true)
            .UseMauiCommunityToolkit();

        return builder.Build();
    }
}
