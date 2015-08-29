using Windows.UI.Xaml;
using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;

namespace MoneyManager.Windows.Controls
{
    public sealed partial class BalanceUserControl
    {
        public BalanceUserControl()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<BalanceViewModel>();
        }

        private void BalanceUserControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            //TODO Move to viewmodel
            Mvx.Resolve<BalanceViewModel>().UpdateBalance();
        }
    }
}