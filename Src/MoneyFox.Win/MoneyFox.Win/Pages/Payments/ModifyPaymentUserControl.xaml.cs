namespace MoneyFox.Win.Pages.Payments;

using Core.Aggregates.AccountAggregate;
using Core.Resources;
using Microsoft.UI.Xaml.Controls;
using ViewModels.Payments;

public sealed partial class ModifyPaymentUserControl
{
    public ModifyPaymentUserControl()
    {
        InitializeComponent();
    }

    public ModifyPaymentViewModel ViewModel => (ModifyPaymentViewModel)DataContext;

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = ((ComboBoxItem)e.AddedItems[0]).Content?.ToString() ?? "";
        if (selectedItem == Strings.IncomeLabel)
        {
            ViewModel.SelectedPayment.Type = PaymentType.Income;
        }
        else if (selectedItem == Strings.ExpenseLabel)
        {
            ViewModel.SelectedPayment.Type = PaymentType.Expense;
        }
        else if (selectedItem == Strings.TransferLabel)
        {
            ViewModel.SelectedPayment.Type = PaymentType.Transfer;
        }
    }
}
