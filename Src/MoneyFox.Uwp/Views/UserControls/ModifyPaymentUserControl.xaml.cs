using Windows.UI.Xaml;
using Microsoft.Toolkit.Uwp.UI.Animations;
using MoneyFox.Presentation.ViewModels;
using NLog;

namespace MoneyFox.Uwp.Views.UserControls
{
    public sealed partial class ModifyPaymentUserControl
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        public ModifyPaymentUserControl()
        {
            InitializeComponent();
        }

        private async void ToggleRecurringVisibility(object sender, RoutedEventArgs e)
        {
            var viewModel = (ModifyPaymentViewModel) DataContext;

            if (viewModel.SelectedPayment == null) return;
            if (viewModel.SelectedPayment.IsRecurring)
            {
                await RecurringStackPanel.Fade(1).StartAsync();
                RecurringStackPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                RecurringStackPanel.Visibility = Visibility.Visible;
                await RecurringStackPanel.Fade().StartAsync();
            }
        }

        private void SetVisibilityInitially(object sender, RoutedEventArgs e)
        {
            var viewModel = (ModifyPaymentViewModel) DataContext;

            if (viewModel == null)
            {
                logManager.Warn("ViewModel is null on SetVisibilityInitially");
                return;
            }

            if (viewModel.SelectedPayment == null)
            {
                logManager.Warn("SelectedPayment is null on SetVisibilityInitially");
                return;
            }

            if (!viewModel.SelectedPayment.IsRecurring) ToggleRecurringVisibility(this, null);
        }
    }
}
