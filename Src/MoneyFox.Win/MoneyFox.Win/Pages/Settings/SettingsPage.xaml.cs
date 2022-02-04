using MoneyFox.Win.ViewModels.Settings;

namespace MoneyFox.Win.Pages.Settings
{
    public sealed partial class SettingsPage : BasePage
    {
        private WindowsSettingsViewModel ViewModel => (WindowsSettingsViewModel)DataContext;

        public SettingsPage()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.SettingsVm;
        }
    }
}