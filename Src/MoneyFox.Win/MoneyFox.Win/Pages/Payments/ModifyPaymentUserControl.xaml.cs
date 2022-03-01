namespace MoneyFox.Win.Pages.Payments;

using Core.Aggregates.Payments;
using Core.Resources;
using Microsoft.UI.Xaml.Controls;
using ViewModels.Payments;

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