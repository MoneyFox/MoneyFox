using MoneyFox.Uwp.ViewModels.Settings;

namespace MoneyFox.Uwp.Views.Settings
{
    public sealed partial class SettingsView : BaseView
    {
        private WindowsSettingsViewModel ViewModel => (WindowsSettingsViewModel)DataContext;

        public SettingsView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.SettingsVm;
        }
    }
}
