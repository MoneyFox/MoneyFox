using Windows.UI.Xaml.Navigation;
using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class EditPaymentView
    {
        private EditPaymentViewModel ViewModel => DataContext as EditPaymentViewModel;

        public EditPaymentView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null) ViewModel.PaymentId = (int)e.Parameter;
        }
    }
}
