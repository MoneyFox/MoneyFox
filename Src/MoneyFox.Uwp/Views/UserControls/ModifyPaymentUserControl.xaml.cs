using Windows.UI.Xaml;
using Microsoft.AppCenter.Analytics;
using Microsoft.Toolkit.Uwp.UI.Animations;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.ServiceLayer.ViewModels;

namespace MoneyFox.Uwp.Views.UserControls
{
    public sealed partial class ModifyPaymentUserControl 
    {
        public ModifyPaymentUserControl()
        {
            InitializeComponent();
        }

        private async void ToggleRecurringVisibility(object sender, RoutedEventArgs e)
        {
            var viewModel = (ModifyPaymentViewModel)DataContext;
            if (viewModel.SelectedPayment == null) return;
            if (viewModel.SelectedPayment.IsRecurring)
            {
                await RecurringStackPanel.Fade(1).StartAsync();
            } else
            {
                await RecurringStackPanel.Fade().StartAsync();
            }
        }

        private void SetVisibilityInitialy(object sender, RoutedEventArgs e)
        {
            var viewModel = (ModifyPaymentViewModel)DataContext;

            if (viewModel == null)
            {
                Analytics.TrackEvent("Error: viewModel is null on SetVisibilityInitialy");
                return;
            }
            if (viewModel.SelectedPayment == null)
            {
                Analytics.TrackEvent("Error: SelectedPayment is null on SetVisibilityInitialy");
                return;
            }

            if (!viewModel.SelectedPayment.IsRecurring)
            {
                ToggleRecurringVisibility(this, null);
            }
        }
    }
}
