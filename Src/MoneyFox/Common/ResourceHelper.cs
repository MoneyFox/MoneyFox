using Xamarin.Forms;

namespace MoneyFox.Common
{
    public static class ResourceHelper
    {
        public static Color GetCurrentTextColor()
        {
            if(App.Current.UserAppTheme == OSAppTheme.Dark)
            {
                App.Current.Resources.TryGetValue("TextPrimaryColor_Dark", out var darkTextColor);
                return (Color)darkTextColor;
            }
            App.Current.Resources.TryGetValue("TextPrimaryColor_Light", out var lightTextColor);
            return (Color)lightTextColor;
        }

        public static Color GetCurrentBackgroundColor()
        {
            if(App.Current.UserAppTheme == OSAppTheme.Dark)
            {
                App.Current.Resources.TryGetValue("BackgroundColorDark", out var darkBackgroundColor);
                return (Color)darkBackgroundColor;
            }
            App.Current.Resources.TryGetValue("BackgroundColorLight", out var lightBackgroundColor);
            return (Color)lightBackgroundColor;
        }
    }
}
