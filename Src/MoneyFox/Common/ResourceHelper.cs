using Xamarin.Forms;

namespace MoneyFox.Common
{
    public static class ResourceHelper
    {
        public static Color CurrentBackgroundColor
        {
            get
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
}
