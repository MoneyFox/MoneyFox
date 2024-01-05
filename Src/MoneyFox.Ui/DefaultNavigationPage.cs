namespace MoneyFox.Ui;

using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Application = Microsoft.Maui.Controls.Application;

public class DefaultNavigationPage : Microsoft.Maui.Controls.NavigationPage
{
    public DefaultNavigationPage(Microsoft.Maui.Controls.Page root) : base(root)
    {
        BarBackgroundColor = Application.Current?.RequestedTheme == AppTheme.Dark
            ? (Color)App.ResourceDictionary["Colors"]["BackgroundColorDark"]
            : (Color)App.ResourceDictionary["Colors"]["BackgroundColorLight"];

        BarTextColor = Application.Current?.RequestedTheme == AppTheme.Dark
            ? (Color)App.ResourceDictionary["Colors"]["TextPrimaryColorDark"]
            : (Color)App.ResourceDictionary["Colors"]["TextPrimaryColorLight"];

        On<iOS>().SetHideNavigationBarSeparator(true);
    }
}
