using Microsoft.Practices.ServiceLocation;
using MoneyManager.Core.ViewModels;

namespace MoneyManager.Windows.Views
{
    public sealed partial class PaymentListView
    {
        public PaymentListView()
        {
            InitializeComponent();
            DataContext = ServiceLocator.Current.GetInstance<PaymentListViewModel>();
        }
    }
}