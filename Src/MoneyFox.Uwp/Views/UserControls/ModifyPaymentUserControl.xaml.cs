using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Uwp.ViewModels;
using NLog;
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
