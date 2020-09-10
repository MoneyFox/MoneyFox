using MoneyFox.Domain;
using MoneyFox.Uwp.ViewModels.Payments;
using Windows.UI.Xaml.Navigation;

namespace MoneyFox.Uwp.Views.Payments
{
    public sealed partial class AddPaymentView
    {
        public AddPaymentView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.PaymentType = (PaymentType)e.Parameter;
            ViewModel.InitializeCommand.Execute(null);
        }

        public AddPaymentViewModel ViewModel => (AddPaymentViewModel)DataContext;
    }
}
