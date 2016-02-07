using MoneyManager.Foundation.Interfaces.ViewModels;
using MvvmCross.Platform;

namespace MoneyManager.Windows.Views.UserControls
{
    public sealed partial class BalanceUserControl
    {
        public BalanceUserControl()
        {
            InitializeComponent();

            // TODO: Set this in the hosting viewmodel.
            DataContext = Mvx.Resolve<IBalanceViewModel>();
        }
    }
}