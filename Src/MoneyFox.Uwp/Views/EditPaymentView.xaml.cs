using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MoneyFox.Presentation;
using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class EditPaymentView
    {
        public override string Header => ViewModelLocator.EditPaymentVm.Title;

        private EditPaymentViewModel ViewModel => DataContext as EditPaymentViewModel;

        public EditPaymentView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null && e.NavigationMode != NavigationMode.Back)
            {
                ViewModel.PaymentId = (int) e.Parameter;
                ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back) ResetPageCache();
        }

        private void ResetPageCache()
        {
            int cacheSize = ((Frame) Parent).CacheSize;
            ((Frame) Parent).CacheSize = 0;
            ((Frame) Parent).CacheSize = cacheSize;
        }
    }
}
