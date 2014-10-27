using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using Windows.UI.Xaml;

namespace MoneyManager.UserControls
{
    public sealed partial class TotalBalanceUserControl
    {
        public TotalBalanceUserControl()
        {
            InitializeComponent();
        }

        private void TotalBalanceUserControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Current.GetInstance<TotalBalanceViewModel>().UpdateBalance();
        }
    }
}