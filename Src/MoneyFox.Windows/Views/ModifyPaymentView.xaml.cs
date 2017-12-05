using System.Collections.Generic;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Toolkit.Uwp.UI.Animations;
using MoneyFox.Business.Helpers;
using MoneyFox.Business.ViewModels;

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

        private async void ToggleRecurringVisibility(object sender, RoutedEventArgs e)
        {
            var viewModel = (ModifyPaymentViewModel)ViewModel;
            if (viewModel.SelectedPayment.IsRecurring)
            {
                await RecurringStackPanel.Fade(1).StartAsync();
            }
            else
            {
                await RecurringStackPanel.Fade().StartAsync();
            }
        }

        private void SetVisibiltyInitialy(object sender, RoutedEventArgs e)
        {
            var viewModel = (ModifyPaymentViewModel)ViewModel;

            if (viewModel == null)
            {
                Analytics.TrackEvent("Error: viewModel is null on SetVisibiltyInitialy");
                return;
            }
            if (viewModel.SelectedPayment == null)
            {
                Analytics.TrackEvent("Error: SelectedPayment is null on SetVisibiltyInitialy");
                return;
            }

            if (!viewModel.SelectedPayment.IsRecurring)
            {
                ToggleRecurringVisibility(this, null);
            }
        }
    }
}