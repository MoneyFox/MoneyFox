using MoneyManager;
using MoneyTracker.Src;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;

namespace MoneyTracker.UserControls
{
    public sealed partial class AddTransactionUserControl
    {
        public AddTransactionUserControl()
        {
            InitializeComponent();
            DataContext = App.TransactionViewModel.SelectedTransaction;
            CmbChargedAccount.ItemsSource = App.AccountViewModel.AllAccounts;
            CmbTargetAccount.ItemsSource = App.AccountViewModel.AllAccounts;
            CmbCategory.ItemsSource = App.CategoryViewModel.AllCategories;

            var recurrences = new List<RecurrenceType>()
            {
                RecurrenceType.Weekly, RecurrenceType.Monthly
            };
            ComboBoxRecurrenceType.ItemsSource = recurrences;

            App.TransactionViewModel.SelectedTransaction.Date = DateTime.Today;
        }

        public string GetRecurrenceType()
        {
            return ComboBoxRecurrenceType.SelectedValue.ToString();
        }

        public DateTime GetEndDate()
        {
            return DatePickerEndDate.Date.Date;
        }

        public void AdjustForm(TransactionType type)
        {
            if (type == TransactionType.Transfer)
            {
                CmbTargetAccount.Visibility = Visibility.Visible;
            }

            if (App.AccountViewModel.AllAccounts.Any())
            {
                CmbChargedAccount.SelectedItem = App.AccountViewModel.AllAccounts.FirstOrDefault();
                CmbTargetAccount.SelectedItem = App.AccountViewModel.AllAccounts.FirstOrDefault();
            }
        }
    }
}