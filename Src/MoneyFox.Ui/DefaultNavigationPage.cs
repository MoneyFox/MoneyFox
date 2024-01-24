namespace MoneyFox.Ui;

using Common.Navigation;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Application = Application;
using NavigationPage = NavigationPage;
using Page = Page;

public class DefaultNavigationPage : NavigationPage
{
    public DefaultNavigationPage(Page root) : base(root)
    {
        SetBarColors();
        // For the moment removed, since it causes issues with the separator for some reason..
        // https://stackoverflow.com/questions/77821225/net-maui-hide-separator-in-title?noredirect=1#comment137211588_77821225
        //Application.Current!.RequestedThemeChanged += (_, _) => { SetBarColors(); };
        On<iOS>().SetHideNavigationBarSeparator(true);

        Popped += (sender, _) =>
        {
            var view = (sender as NavigationPage)!.Navigation.NavigationStack.LastOrDefault();
            if (view?.BindingContext is NavigableViewModel navigableViewModel)
            {
                navigableViewModel.OnNavigatedBackAsync(null);
            }
        };
    }

    private void SetBarColors()
    {
        BarBackgroundColor = Application.Current?.RequestedTheme == AppTheme.Dark
            ? (Color)App.ResourceDictionary["Colors"]["BackgroundColorDark"]
            : (Color)App.ResourceDictionary["Colors"]["BackgroundColorLight"];

        BarTextColor = Application.Current?.RequestedTheme == AppTheme.Dark
            ? (Color)App.ResourceDictionary["Colors"]["TextPrimaryColorDark"]
            : (Color)App.ResourceDictionary["Colors"]["TextPrimaryColorLight"];
    }
}
