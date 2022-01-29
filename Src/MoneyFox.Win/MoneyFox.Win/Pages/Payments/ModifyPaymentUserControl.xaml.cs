using Microsoft.UI.Xaml.Controls;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Core.Resources;
using MoneyFox.Win.ViewModels.Payments;

namespace MoneyFox.Win.Pages.Payments
{
    public sealed partial class ModifyPaymentUserControl
    {
        public ModifyPaymentViewModel ViewModel => (ModifyPaymentViewModel)DataContext;

        public ModifyPaymentUserControl()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedItem = ((ComboBoxItem)e.AddedItems[0]).Content?.ToString() ?? "";

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