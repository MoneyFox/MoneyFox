using MoneyFox.Presentation.ViewModels.Settings;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Views
{
    public partial class SettingsRegionalPage : ContentPage
    {
        private IRegionalSettingsViewModel ViewModel => BindingContext as IRegionalSettingsViewModel;

        public SettingsRegionalPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.RegionalSettingsVm;
        }

        protected override async void OnAppearing()
        {
            await ViewModel.LoadAvailableCulturesCommand.ExecuteAsync();
        }
    }
}
