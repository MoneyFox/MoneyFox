using Microsoft.Practices.ServiceLocation;
using MoneyManager.Models;
using MoneyManager.Src;
using MoneyManager.ViewModels;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;
using Telerik.UI.Xaml.Controls.Primitives;

namespace MoneyManager.DataAccess
{
    [ImplementPropertyChanged]
    public class AccountDataAccess : AbstractDataAccess<Account>
    {
        private TransactionDataAccess TransactionViewModel
        {
            get { return new TransactionDataAccess(); }
        }

        public Account SelectedAccount { get; set; }

        public ObservableCollection<Account> AllAccounts { get; set; }

        protected override void SaveToDb(Account account)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                if (AllAccounts == null)
                {
                    AllAccounts = new ObservableCollection<Account>();
                }

                AllAccounts.Add(account);
                account.Id = dbConn.Insert(account);
            }
        }

        protected override void DeleteFromDatabase(Account account)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                TransactionViewModel.DeleteAssociatedTransactionsFromDatabase(account.Id);

                AllAccounts.Remove(account);
                dbConn.Delete(account);
            }
        }

        protected override sealed void GetListFromDb()
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                AllAccounts = new ObservableCollection<Account>(dbConn.Table<Account>().ToList());
                ServiceLocator.Current.GetInstance<TotalBalanceViewModel>().UpdateBalance();
            }
        }

        protected override void UpdateItem(Account account)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                dbConn.Update(account, typeof(Account));
            }
        }

        public void RemoveTransactionAmount(FinancialTransaction transaction)
        {
            transaction.Amount = -transaction.Amount;
            AddTransactionAmount(transaction);
        }

        public void AddTransactionAmount(FinancialTransaction transaction)
        {
            if (transaction.ClearTransactionNow)
            {
                var account = AllAccounts.FirstOrDefault(x => x.Id == transaction.ChargedAccountId);
                if (account == null) return;

                var amount = transaction.Type == (int)TransactionType.Spending
                    ? -transaction.Amount
                    : transaction.Amount;

                account.CurrentBalance += amount;
                transaction.Cleared = true;
                UpdateItem(account);
            }
        }
    }
}