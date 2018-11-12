using MoneyFox.Business.Adapter;
using MoneyFox.Business.Manager;
using MoneyFox.Foundation;
using Xamarin.Forms;

namespace MoneyFox
{
    public static class StyleHelper
    {
        public static Color AccentColor = Color.FromHex("#314a9b");
        public static Color PrimaryColor = Color.FromHex("#263873");
        public static Color SecondaryColor = Color.FromHex("#1a264c");

        public static Color WindowBackgroundColor = Color.FromHex("#EFF2F5");
        public static Color DialogBackgroundColor = Color.White;
        public static Color BottomTabBarColor = Color.WhiteSmoke;

        public static Color PrimaryFontColor = Color.Black;
        public static Color DeemphasizedColor = Color.FromHex("#5A6570");
        public static Color ListItemColor = Color.White;

        public static ImageSource AccountImageSource = ImageSource.FromFile("ic_accounts_black");
        public static ImageSource ExpenseImageSource = ImageSource.FromFile("ic_remove_black");
        public static ImageSource IncomeImageSource = ImageSource.FromFile("ic_add_black");
        public static ImageSource TransferImageSource = ImageSource.FromFile("ic_transfer_black");

        public static void Init()
        {
            if (new SettingsManager(new SettingsAdapter()).Theme == AppTheme.Dark)
            {
                SetDarkColors();
            }
        }

        private static void SetDarkColors()
        {
            AccountImageSource = ImageSource.FromFile("ic_accounts_white");
            ExpenseImageSource = ImageSource.FromFile("ic_remove_white");
            IncomeImageSource = ImageSource.FromFile("ic_add_white");
            TransferImageSource = ImageSource.FromFile("ic_transfer_white");

            WindowBackgroundColor = Color.FromHex("#2d2d30");
            DialogBackgroundColor = Color.Black;
            BottomTabBarColor = Color.FromHex("#1e1e1e");

            PrimaryFontColor = Color.White;
            DeemphasizedColor = Color.FromHex("#5A6570");
            ListItemColor = Color.Black;
        }
    }
}
