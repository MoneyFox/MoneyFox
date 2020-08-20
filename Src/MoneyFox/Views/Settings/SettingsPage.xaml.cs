using MoneyFox.ViewModels.Settings;

namespace MoneyFox.Views.Settings
{
    public partial class SettingsPage
    {
        private SettingsViewModel ViewModel => (SettingsViewModel)BindingContext;
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.SettingsViewModel;
        }

        protected override async void OnAppearing() => await ViewModel.InitializeAsync();
    }
}