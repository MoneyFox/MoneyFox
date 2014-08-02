using MoneyManager.Common;
using MoneyManager.Models;
using MoneyManager.Src;
using MoneyManager.ViewModels;
using MoneyManager.ViewModels.Data;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace MoneyManager.Views
{
    public sealed partial class AddTransaction
    {
        private readonly NavigationHelper navigationHelper;
        private Parameters parameters = new Parameters();

        public AddTransaction()
        {
            InitializeComponent();

            navigationHelper = new NavigationHelper(this);
        }

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        private FinancialTransaction selectedTransaction
        {
            get { return new ViewModelLocator().TransactionViewModel.SelectedTransaction; }
        }

        private TransactionViewModel transactionViewModel
        {
            get { return new ViewModelLocator().TransactionViewModel; }
        }

        private RecurrenceTransactionViewModel recurrenceTransactionViewModel
        {
            get { return new ViewModelLocator().RecurrenceTransactionViewModel; }
        }

        private async void Done_Click(object sender, RoutedEventArgs e)
        {
            selectedTransaction.Type = (int)parameters.TransactionType;

            if (selectedTransaction.ChargedAccountId == 0 ||
                (parameters.TransactionType == TransactionType.Transfer
                && selectedTransaction.TargetAccountId == 0))
            {
                await new MessageDialog(Utilities.GetTranslation("AccountMissingMessage")).ShowAsync();
                return;
            }

            if (parameters.TransactionType == TransactionType.Spending
                || parameters.TransactionType == TransactionType.Transfer)
            {
                selectedTransaction.Amount = -selectedTransaction.Amount;
            }

            if (parameters.Edit)
            {
                transactionViewModel.Update(selectedTransaction);
            }
            else
            {
                transactionViewModel.Save(selectedTransaction);
                CreateRecurringTransaction();
            }
            NavigationHelper.GoBack();
        }

        private void CreateRecurringTransaction()
        {
            if (selectedTransaction.IsRecurrence)
            {
                var recurrenceTransaction = new RecurringTransaction
                {
                    TransactionId = selectedTransaction.Id,
                    StartDate = selectedTransaction.Date,
                    EndDate = AddTransactionControl.GetEndDate(),
                    RecurringType = AddTransactionControl.GetRecurrenceType()
                };
                recurrenceTransactionViewModel.Save(recurrenceTransaction);
                selectedTransaction.ReccuringTransactionId = recurrenceTransaction.Id;
                transactionViewModel.Update(selectedTransaction);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.GoBack();
        }
    }
}