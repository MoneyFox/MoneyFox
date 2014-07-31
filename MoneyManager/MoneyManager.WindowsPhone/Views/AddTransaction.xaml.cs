using MoneyManager.Models;
using MoneyTracker.Models;
using MoneyTracker.Src;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

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

        public FinancialTransaction SelectedTransaction
        {
            get { return App.TransactionViewModel.SelectedTransaction; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            parameters = e.Parameter as Parameters;

            AddTransactionControl.AdjustForm(parameters.TransactionType);
        }

        private async void Done_Click(object sender, RoutedEventArgs e)
        {
            SelectedTransaction.Type = (int)parameters.TransactionType;

            if (SelectedTransaction.ChargedAccountId == 0 ||
                (parameters.TransactionType == TransactionType.Transfer
                && SelectedTransaction.TargetAccountId == 0))
            {
                await new MessageDialog(Utilities.GetTranslation("AccountMissingMessage")).ShowAsync();
                return;
            }

            if (parameters.TransactionType == TransactionType.Spending
                || parameters.TransactionType == TransactionType.Transfer)
            {
                App.TransactionViewModel.SelectedTransaction.Amount =
                    -App.TransactionViewModel.SelectedTransaction.Amount;
            }

            if (parameters.Edit)
            {
                App.TransactionViewModel.Update(App.TransactionViewModel.SelectedTransaction);
            }
            else
            {
                App.TransactionViewModel.Save(App.TransactionViewModel.SelectedTransaction);
                CreateRecurringTransaction();
            }
            NavigationHelper.GoBack();
        }

        private void CreateRecurringTransaction()
        {
            if (App.TransactionViewModel.SelectedTransaction.IsRecurrence)
            {
                var recurrenceTransaction = new RecurringTransaction
                {
                    TransactionId = SelectedTransaction.Id,
                    StartDate = SelectedTransaction.Date,
                    EndDate = AddTransactionControl.GetEndDate(),
                    RecurringType = AddTransactionControl.GetRecurrenceType()
                };
                App.RecurrenceTransactionViewModel.Save(recurrenceTransaction);
                App.TransactionViewModel.SelectedTransaction.ReccuringTransactionId = recurrenceTransaction.Id;
                App.TransactionViewModel.Update(App.TransactionViewModel.SelectedTransaction);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.GoBack();
        }
    }
}