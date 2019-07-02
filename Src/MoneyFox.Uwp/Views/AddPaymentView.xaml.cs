using MoneyFox.Foundation;
using MoneyFox.Presentation.ViewModels;
using Windows.UI.Xaml.Navigation;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class AddPaymentView
    {
        private AddPaymentViewModel ViewModel => DataContext as AddPaymentViewModel;

        public AddPaymentView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null) ViewModel.SelectedPayment.Type = (PaymentType)e.Parameter;
        }
    }
}
