using MoneyFox.BusinessLogic.Adapters;
using MoneyFox.Foundation;
using MoneyFox.Presentation.Facades;
using Xamarin.Forms;

#pragma warning disable CA2211 // Non-constant fields should not be visible
namespace MoneyFox.Presentation
{
    public static class StyleHelper
    {
        public static Color AccentColor { get; } = Color.FromHex("#314a9b");

        public static Color PrimaryColor { get; } = Color.FromHex("#263873");
        public static Color SecondaryColor { get; } = Color.FromHex("#1a264c");

        public static Color WindowBackgroundColor { get; private set; } = Color.FromHex("#EFF2F5");
        public static Color DialogBackgroundColor { get; private set; } = Color.White;

        public static Color BottomTabBarColor { get; private set; } = Color.WhiteSmoke;
        public static Color BarItemColor { get; private set; } = Color.DarkGray;
        public static Color BarSelectedItemColor { get; private set; } = AccentColor;

        public static Color PrimaryFontColor { get; private set; } = Color.Black;
        public static Color DeemphasizedColor { get; private set; } = Color.FromHex("#5A6570");
        public static Color ListItemColor { get; private set; } = Color.White;

        public static ImageSource AccountImageSource { get; private set; } = ImageSource.FromFile("ic_accounts_black");
        public static ImageSource StatisticSelectorImageSource { get; private set; } = ImageSource.FromFile("ic_statistics_black");
        public static ImageSource SettingsImageSource { get; private set; } = ImageSource.FromFile("ic_settings_black");
        public static ImageSource ExpenseImageSource { get; private set; } = ImageSource.FromFile("ic_remove_black");
        public static ImageSource IncomeImageSource { get; private set; } = ImageSource.FromFile("ic_add_black");
        public static ImageSource TransferImageSource { get; private set; } = ImageSource.FromFile("ic_transfer_black");
        public static ImageSource IsClearedImageSource { get; private set; } = ImageSource.FromFile("ic_cleared_black");
        public static ImageSource IsRecurringImageSource { get; private set; } = ImageSource.FromFile("ic_recurring_black");

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

            WindowBackgroundColor = Color.Black; 
            DialogBackgroundColor = Color.FromHex("#2d2d30");;
            BottomTabBarColor = Color.FromHex("#1e1e1e");

            BarItemColor = Color.DimGray;
            BarSelectedItemColor = Color.White;

            PrimaryFontColor = Color.White;
            DeemphasizedColor = Color.FromHex("#5A6570");
            ListItemColor = Color.FromHex("#1e1e1e");
        }
    }
}
