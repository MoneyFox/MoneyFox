using MoneyFox.ViewModels.Settings;

namespace MoneyFox.Views.Settings
{
    public partial class SettingsPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.SettingsViewModel;
        }

        private SettingsViewModel ViewModel => (SettingsViewModel)BindingContext;

        protected override async void OnAppearing() => await ViewModel.InitializeAsync();
    }
}