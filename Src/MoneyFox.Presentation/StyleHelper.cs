using MoneyFox.BusinessLogic.Adapters;
using MoneyFox.Foundation;
using MoneyFox.ServiceLayer.Facades;
using Xamarin.Forms;

#pragma warning disable CA2211 // Non-constant fields should not be visible
namespace MoneyFox.Presentation
{
    public static class StyleHelper
    {
        public static Color AccentColor = Color.FromHex("#314a9b");
        public static Color PrimaryColor = Color.FromHex("#263873");
        public static Color SecondaryColor = Color.FromHex("#1a264c");

        public static Color WindowBackgroundColor = Color.FromHex("#EFF2F5");
        public static Color DialogBackgroundColor = Color.White;

        public static Color BottomTabBarColor = Color.WhiteSmoke;
        public static Color BarItemColor = Color.DarkGray;
        public static Color BarSelectedItemColor = AccentColor;

        public static Color PrimaryFontColor = Color.Black;
        public static Color DeemphasizedColor = Color.FromHex("#5A6570");
        public static Color ListItemColor = Color.White;

        public static ImageSource AccountImageSource = ImageSource.FromFile("ic_accounts_black");
        public static ImageSource StatisticSelectorImageSource = ImageSource.FromFile("ic_statistics_black");
        public static ImageSource SettingsImageSource = ImageSource.FromFile("ic_settings_black");
        public static ImageSource ExpenseImageSource = ImageSource.FromFile("ic_remove_black");
        public static ImageSource IncomeImageSource = ImageSource.FromFile("ic_add_black");
        public static ImageSource TransferImageSource = ImageSource.FromFile("ic_transfer_black");
        public static ImageSource IsClearedImageSource = ImageSource.FromFile("ic_cleared_black");
        public static ImageSource IsRecurringImageSource = ImageSource.FromFile("ic_recurring_black");

        public static void Init()
        {
            if (new SettingsFacade(new SettingsAdapter()).Theme == AppTheme.Dark)
            {
                SetDarkColors();
            }
        }

        private static void SetDarkColors()
        {
            AccountImageSource = ImageSource.FromFile("ic_accounts_white");
            StatisticSelectorImageSource = ImageSource.FromFile("ic_statistics_white");
            SettingsImageSource = ImageSource.FromFile("ic_settings_white");
            ExpenseImageSource = ImageSource.FromFile("ic_remove_white");
            IncomeImageSource = ImageSource.FromFile("ic_add_white");
            TransferImageSource = ImageSource.FromFile("ic_transfer_white");
            IsClearedImageSource = ImageSource.FromFile("ic_cleared_white");
            IsRecurringImageSource = ImageSource.FromFile("ic_recurring_white");

            WindowBackgroundColor = Color.Black; //Color.FromHex("#2d2d30");
            DialogBackgroundColor = Color.Black;
            BottomTabBarColor = Color.FromHex("#1e1e1e");

            BarItemColor = Color.DimGray;
            BarSelectedItemColor = Color.White;

            PrimaryFontColor = Color.White;
            DeemphasizedColor = Color.FromHex("#5A6570");
            ListItemColor = Color.FromHex("#1e1e1e");
        }
    }
}
