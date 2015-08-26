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
        }

        private void BalanceUserControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            Mvx.Resolve<BalanceViewModel>().UpdateBalance();
        }
    }
}