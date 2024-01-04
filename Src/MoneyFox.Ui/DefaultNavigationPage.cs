namespace MoneyFox.Ui;

public class DefaultNavigationPage : NavigationPage
{
    public DefaultNavigationPage(Page root) : base(root)
    {
        BarBackgroundColor = Application.Current?.RequestedTheme == AppTheme.Dark
            ? (Color)App.ResourceDictionary["Colors"]["BackgroundColorDark"]
            : (Color)App.ResourceDictionary["Colors"]["BackgroundColorLight"];

        BarTextColor = Application.Current?.RequestedTheme == AppTheme.Dark
            ? (Color)App.ResourceDictionary["Colors"]["TextPrimaryColorDark"]
            : (Color)App.ResourceDictionary["Colors"]["TextPrimaryColorLight"];
    }
}
