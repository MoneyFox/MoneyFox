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
                fonts.AddFont("materialdesignicons-webfont.ttf", "MaterialIcons");
            })
            .UseMauiCommunityToolkit();

        return builder.Build();
    }
}
