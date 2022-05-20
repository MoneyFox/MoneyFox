namespace MoneyFox;

using CommunityToolkit.Maui;

public class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("materialdesignicons-webfont.ttf", "MaterialIcons");
            }).UseMauiCommunityToolkit();

        return builder.Build();
    }
}
