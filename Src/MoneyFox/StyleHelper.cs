using Xamarin.Forms;

namespace MoneyFox
{
    public static class StyleHelper
    {
        public static Color AccentColor;
        public static Color PrimaryColor;
        public static Color SecondaryColor;

        public static Color WindowBackgroundColor;
        public static Color BottomTabBarColor;

        public static Color PrimaryFontColor;
        public static Color DeemphasizedColor;
        public static Color ListItemColor;

        public static void Init()
        {
            AccentColor = Color.FromHex("#314a9b");
            PrimaryColor = Color.FromHex("#263873");
            SecondaryColor = Color.FromHex("#1a264c");

            WindowBackgroundColor = Color.FromHex("#EFF2F5");
            BottomTabBarColor = Color.WhiteSmoke;

            PrimaryFontColor = Color.Black;
            DeemphasizedColor = Color.FromHex("#5A6570");
            ListItemColor = Color.White;
        }
    }
}
