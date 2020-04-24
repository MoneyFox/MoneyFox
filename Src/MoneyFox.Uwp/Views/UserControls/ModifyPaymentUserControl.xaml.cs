using Microsoft.Toolkit.Uwp.UI.Animations;
using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Uwp.ViewModels;
using NLog;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MoneyFox.Uwp.Views.UserControls
{
    public sealed partial class ModifyPaymentUserControl
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        private ModifyPaymentViewModel ViewModel => (ModifyPaymentViewModel)DataContext;

        public ModifyPaymentUserControl()
        {
            InitializeComponent();
        }

        private async void ToggleRecurringVisibility(object sender, RoutedEventArgs e)
        {
            if(ViewModel.SelectedPayment == null)
                return;
            if(ViewModel.SelectedPayment.IsRecurring)
            {
                await RecurringStackPanel.Fade(1).StartAsync();
            }
            else
            {
                await RecurringStackPanel.Fade().StartAsync();
            }
        }

        private void SetVisibilityInitially(object sender, RoutedEventArgs e)
        {
            if(ViewModel == null)
            {
                logManager.Warn("ViewModel is null on SetVisibilityInitially");
                return;
            }

            if(ViewModel.SelectedPayment == null)
            {
                logManager.Warn("SelectedPayment is null on SetVisibilityInitially");
                return;
            }

            if(!ViewModel.SelectedPayment.IsRecurring)
            {
                ToggleRecurringVisibility(this, null);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (e.AddedItems[0] as ComboBoxItem).Content?.ToString() ?? "";

            if(selectedItem == Strings.IncomeLabel)
            {
                ViewModel.SelectedPayment.Type = PaymentType.Income;
            }
            else if(selectedItem == Strings.ExpenseLabel)
            {
                ViewModel.SelectedPayment.Type = PaymentType.Expense;
            }
            else if(selectedItem == Strings.TransferLabel)
            {
                ViewModel.SelectedPayment.Type = PaymentType.Transfer;
            }
        }
    }
}
