using Xamarin.Forms;

namespace MoneyFox.Common
{
    public static class ResourceHelper
    {
        public static Color GetCurrentTextColor()
        {
            if(App.Current.UserAppTheme == OSAppTheme.Dark)
            {
                App.Current.Resources.TryGetValue("TextPrimaryColor_Dark", out object? darkTextColor);
                return (Color)darkTextColor;
            }
            App.Current.Resources.TryGetValue("TextPrimaryColor_Light", out object? lightTextColor);
            return (Color)lightTextColor;
        }

        public static Color GetCurrentBackgroundColor()
        {
            if(App.Current.UserAppTheme == OSAppTheme.Dark)
            {
                App.Current.Resources.TryGetValue("BackgroundColorDark", out object? darkBackgroundColor);
                return (Color)darkBackgroundColor;
            }
            App.Current.Resources.TryGetValue("BackgroundColorLight", out object? lightBackgroundColor);
            return (Color)lightBackgroundColor;
        }
    }
}
