using Microsoft.Practices.ServiceLocation;
using MoneyFox.Core.ViewModels;

namespace MoneyFox.Windows.Views
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