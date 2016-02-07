using MoneyManager.Core.ViewModels;
using MvvmCross.Platform;

namespace MoneyManager.Windows.Views
{
    public sealed partial class PaymentListView
    {
        public PaymentListView()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<PaymentListViewModel>();
        }
    }
}