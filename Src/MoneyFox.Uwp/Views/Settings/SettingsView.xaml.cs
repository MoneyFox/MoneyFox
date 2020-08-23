using MoneyFox.Application.Resources;
using MoneyFox.Uwp.ViewModels.Settings;

namespace MoneyFox.Uwp.Views.Settings
{
    public sealed partial class SettingsView
    {
        public override string Header => Strings.SettingsTitle;

        private WindowsSettingsViewModel ViewModel => (WindowsSettingsViewModel)DataContext;

        public SettingsView()
        {
            InitializeComponent();
        }
    }
}
