using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Views;

namespace MoneyManager.UserControls
{
    public sealed partial class SettingsGeneralUserControl
    {
        public SettingsGeneralUserControl()
        {
            InitializeComponent();
        }

        private void OpenSelectCurrencyDialog(object sender, RoutedEventArgs routedEventArgs)
        {
            ServiceLocator.Current.GetInstance<SelectCurrencyViewModel>().InvocationType = InvocationType.Setting;
            ((Frame)Window.Current.Content).Navigate(typeof(SelectCurrency));
        }
    }
}