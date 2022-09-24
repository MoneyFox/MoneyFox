namespace MoneyFox;

using CommunityToolkit.Maui;
using SkiaSharp.Views.Maui.Controls.Hosting;

public class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseSkiaSharp(true)
            .UseMauiApp<App>()
            .ConfigureFonts(
                fonts =>
                {
                    fonts.AddFont(filename: "ProductSans-Regular.ttf", alias: "Product");
                    fonts.AddFont(filename: "MaterialIconsRound-Regular.otf", alias: "MaterialIconsRound");
                    fonts.AddFont(filename: "RobotoMono-Regular.ttf", alias: "Roboto");
                })
            .UseMauiCommunityToolkit();

        return builder.Build();
    }
}
