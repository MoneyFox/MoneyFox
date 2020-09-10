using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Uwp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace MoneyFox.Uwp.Views.Payments
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
            var selectedItem = ((ComboBoxItem)e.AddedItems[0]).Content?.ToString() ?? "";

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
