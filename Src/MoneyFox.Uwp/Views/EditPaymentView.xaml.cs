using Windows.UI.Xaml.Navigation;
using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;
using Windows.UI.Xaml.Controls;

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
            if (e.Parameter != null && e.NavigationMode != NavigationMode.Back)
            {
                ViewModel.PaymentId = (int)e.Parameter;
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
