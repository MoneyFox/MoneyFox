using MoneyFox.Presentation.ViewModels;
using Windows.UI.Xaml.Navigation;
using MoneyFox.Domain;
using MoneyFox.Presentation.Utilities;

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
            if (e.Parameter != null && e.NavigationMode != NavigationMode.Back)
            {
                ViewModel.SelectedPayment.Type = (PaymentType)e.Parameter;
                ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
            }
        }
    }
}
