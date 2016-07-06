using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MoneyFox.Shared;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.ViewModels;

namespace MoneyFox.Windows.Views
{
    public sealed partial class ModifyPaymentView
    {
        public ModifyPaymentView()
        {
            InitializeComponent();
            //DataContext = Mvx.Resolve<ModifyPaymentViewModel>();

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

                var paymentViewModel = e.Parameter as Payment;
                if (paymentViewModel != null)
                {
                    //TODO Refactor this that on edit the Payment type isn't necessary since we don't need it here.
                    viewModel.Init(PaymentType.Expense.ToString(), paymentViewModel.Id, true);
                }
                else if (e.Parameter?.GetType() == typeof(PaymentType))
                {
                    viewModel.Init(e.Parameter.ToString(), paymentViewModel.Id);
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
            ResetPageCache();
        }

        private void ResetPageCache()
        {
            var cacheSize = ((Frame) Parent).CacheSize;
            ((Frame) Parent).CacheSize = 0;
            ((Frame) Parent).CacheSize = cacheSize;
        }
    }
}