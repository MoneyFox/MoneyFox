using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.ServiceLocation;
using MoneyFox.Core.ViewModels;
using MoneyManager.Core.Helpers;
using MoneyManager.Core.ViewModels;

namespace MoneyManager.Windows.Views
{
    public sealed partial class ModifyPaymentView
    {
        public ModifyPaymentView()
        {
            InitializeComponent();
            DataContext = ServiceLocator.Current.GetInstance<ModifyPaymentViewModel>();

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