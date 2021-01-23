using MoneyFox.Uwp.ViewModels.Settings;

#nullable enable
namespace MoneyFox.Uwp.Views.Settings
{
    public sealed partial class SettingsHostView
    {
        public override bool ShowHeader => false;

        private WindowsSettingsViewModel ViewModel => (WindowsSettingsViewModel)DataContext;

        public SettingsHostView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.SettingsVm;
        }
    }
}
