namespace MoneyFox.Ui;

using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Application = Application;
using NavigationPage = Microsoft.Maui.Controls.NavigationPage;
using Page = Microsoft.Maui.Controls.Page;

public class DefaultNavigationPage : NavigationPage
{
    public DefaultNavigationPage(Page root) : base(root)
    {
        SetbarColors();
        On<iOS>().SetHideNavigationBarSeparator(true);
        Application.Current!.RequestedThemeChanged += (s, a) => { SetbarColors(); };
    }

    private void SetbarColors()
    {
        BarBackgroundColor = Application.Current?.RequestedTheme == AppTheme.Dark
            ? (Color)App.ResourceDictionary["Colors"]["BackgroundColorDark"]
            : (Color)App.ResourceDictionary["Colors"]["BackgroundColorLight"];

        BarTextColor = Application.Current?.RequestedTheme == AppTheme.Dark
            ? (Color)App.ResourceDictionary["Colors"]["TextPrimaryColorDark"]
            : (Color)App.ResourceDictionary["Colors"]["TextPrimaryColorLight"];
    }
}
