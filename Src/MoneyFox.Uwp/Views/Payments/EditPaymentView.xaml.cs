using MoneyFox.Uwp.ViewModels.Payments;
using Windows.UI.Xaml.Navigation;

namespace MoneyFox.Uwp.Views.Payments
{
    public sealed partial class EditPaymentView
    {
        private EditPaymentViewModel ViewModel => (EditPaymentViewModel) DataContext;

        public EditPaymentView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.InitializeCommand.Execute((int)e.Parameter);
        }
    }
}
