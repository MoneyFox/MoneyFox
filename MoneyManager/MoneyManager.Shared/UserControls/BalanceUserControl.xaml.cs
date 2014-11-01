using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using Windows.UI.Xaml;

namespace MoneyManager.UserControls
{
    public sealed partial class BalanceUserControl
    {
        public BalanceUserControl()
        {
            InitializeComponent();
        }

        private void BalanceUserControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Current.GetInstance<BalanceViewModel>().UpdateBalance();
        }
    }
}