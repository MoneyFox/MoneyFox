using MoneyFox.Uwp.ViewModels.Settings;

namespace MoneyFox.Uwp.Views.Settings
{
    public sealed partial class SettingsView : BaseView
    {
        public SettingsView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.SettingsVm;
        }

        private WindowsSettingsViewModel ViewModel => (WindowsSettingsViewModel)DataContext;
    }
}