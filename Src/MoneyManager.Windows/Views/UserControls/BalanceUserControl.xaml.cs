using MoneyManager.Foundation.Interfaces.ViewModels;
using MvvmCross.Platform;

namespace MoneyManager.Windows.Views.UserControls
{
    public sealed partial class BalanceUserControl
    {
        public BalanceUserControl()
        {
            InitializeComponent();

            // This has to be done here. Otherwise the NotifyPropertyChanged event won't be fired anymore.
            DataContext = Mvx.Resolve<IBalanceViewModel>();
        }
    }
}