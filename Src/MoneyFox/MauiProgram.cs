namespace MoneyFox;

using CommunityToolkit.Maui;
using SkiaSharp.Views.Maui.Controls.Hosting;

public class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseSkiaSharp(true)
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("MaterialIconsRound-Regular.otf", "MaterialIconsRound");
                fonts.AddFont("RobotoMono-Regular.ttf", "Roboto");
            })
            .UseMauiCommunityToolkit();

        return builder.Build();
    }
}
