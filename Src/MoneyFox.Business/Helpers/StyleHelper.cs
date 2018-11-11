using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MvvmCross;
using Xamarin.Forms;

namespace MoneyFox.Business.Helpers
{
    public static class StyleHelper
    {
        public static Color AccentColor = Color.FromHex("#314a9b");
        public static Color PrimaryColor = Color.FromHex("#263873");
        public static Color SecondaryColor = Color.FromHex("#1a264c");

        public static Color WindowBackgroundColor = Color.FromHex("#EFF2F5");
        public static Color BottomTabBarColor = Color.WhiteSmoke;

        public static Color PrimaryFontColor = Color.Black;
        public static Color DeemphasizedColor = Color.FromHex("#5A6570");
        public static Color ListItemColor = Color.White;

        public static void Init()
        {
            if (Mvx.IoCProvider.Resolve<ISettingsManager>().Theme == AppTheme.Dark)
            {
                SetDarkColors();
            }
        }

        private static void SetDarkColors()
        {
            WindowBackgroundColor = Color.FromHex("#2d2d30");
            BottomTabBarColor = Color.FromHex("#1e1e1e");

            PrimaryFontColor = Color.White;
            DeemphasizedColor = Color.FromHex("#5A6570");
            ListItemColor = Color.Black;
        }
    }
}
