using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MoneyFox.Business.Helpers;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation;
using MoneyFox.Foundation.DataModels;

namespace MoneyFox.Windows.Views
{
    public sealed partial class ModifyPaymentView
    {
        public ModifyPaymentView()
        {
            InitializeComponent();

            // code to handle bottom app bar when keyboard appears
            // workaround since otherwise the keyboard would overlay some controls
            InputPane.GetForCurrentView().Showing +=
                (s, args) => { BottomCommandBar.Visibility = Visibility.Collapsed; };
            InputPane.GetForCurrentView().Hiding += (s, args2) =>
            {
                if (BottomCommandBar.Visibility == Visibility.Collapsed)
                {
                    BottomCommandBar.Visibility = Visibility.Visible;
                }
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back)
            {
                var viewModel = (ModifyPaymentViewModel) DataContext;

                var payment = e.Parameter as PaymentViewModel;
                if (payment != null)
                {
                    //  This payment type will be ignored. Has to be set though.
                    viewModel.Init(PaymentType.Expense, payment.Id);
                }
                else if (e.Parameter?.GetType() == typeof(PaymentType))
                {
                    viewModel.Init((PaymentType) e.Parameter);
                }
            }

            base.OnNavigatedTo(e);
        }

        private void TextBoxOnFocus(object sender, RoutedEventArgs e)
        {
            TextBoxAmount.SelectAll();
        }

        private void FormatTextBoxOnLostFocus(object sender, RoutedEventArgs e)
        {
            double amount;
            double.TryParse(TextBoxAmount.Text, out amount);
            TextBoxAmount.Text = Utilities.FormatLargeNumbers(amount);
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
            var cacheSize = ((Frame) Parent).CacheSize;
            ((Frame) Parent).CacheSize = 0;
            ((Frame) Parent).CacheSize = cacheSize;
        }
    }
}