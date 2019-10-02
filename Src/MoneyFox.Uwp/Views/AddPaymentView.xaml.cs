using MoneyFox.Presentation.ViewModels;
using Windows.UI.Xaml.Navigation;
using MoneyFox.Domain;
using MoneyFox.Presentation.Utilities;
using Windows.UI.Xaml.Controls;

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
                ViewModel.PaymentType = (PaymentType)e.Parameter;
                ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                ResetPageCache();
            }
        }

        private void ResetPageCache()
        {
            var cacheSize = ((Frame)Parent).CacheSize;
            ((Frame)Parent).CacheSize = 0;
            ((Frame)Parent).CacheSize = cacheSize;
        }
    }
}
