using MoneyFox.Uwp.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class PaymentListView
    {
        public override string Header => ViewModelLocator.PaymentListVm.Title;

        private PaymentListViewModel ViewModel => (PaymentListViewModel) DataContext;

        public PaymentListView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter != null)
                ViewModel.AccountId = (int) e.Parameter;
        }

        private void AppBarToggleButton_Click(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement) sender);
        }
    }
}
