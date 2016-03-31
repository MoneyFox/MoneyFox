using Microsoft.Practices.ServiceLocation;
using MoneyFox.Core.ViewModels;

namespace MoneyFox.Windows.Views
{
    public sealed partial class PaymentViewModelListView
    {
        public PaymentViewModelListView()
        {
            InitializeComponent();
            DataContext = ServiceLocator.Current.GetInstance<PaymentViewModelListViewModel>();
        }
    }
}