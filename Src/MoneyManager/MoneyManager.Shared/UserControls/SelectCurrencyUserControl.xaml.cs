using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;

namespace MoneyManager.UserControls {
    public sealed partial class SelectCurrencyUserControl {
        public SelectCurrencyUserControl() {
            InitializeComponent();
        }

        private async void LoadCountries(object sender, RoutedEventArgs e) {
            await ServiceLocator.Current.GetInstance<SelectCurrencyViewModel>().LoadCountries();
        }
    }
}