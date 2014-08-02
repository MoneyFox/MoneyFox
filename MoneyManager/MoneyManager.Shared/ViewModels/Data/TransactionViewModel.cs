using MoneyManager.Models;
using MoneyManager.Src;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MoneyManager.ViewModels.Data
{
    [ImplementPropertyChanged]
    public class TransactionViewModel : AbstractDataAccess<FinancialTransaction>
    {
        public ObservableCollection<FinancialTransaction> AllTransactions { get; set; }

        public ObservableCollection<FinancialTransaction> RelatedTransactions { get; set; }

        public FinancialTransaction SelectedTransaction { get; set; }

        private AccountViewModel accountViewModel
        {
            get { return new ViewModelLocator().AccountViewModel; }
        }

        protected override void SaveToDb(FinancialTransaction transaction)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                if (AllTransactions == null)
                {
                    AllTransactions = new ObservableCollection<FinancialTransaction>();
                }

                new ViewModelLocator().AccountViewModel.AddTransactionAmount(transaction);

                AllTransactions.Add(transaction);
                dbConn.Insert(transaction, typeof(FinancialTransaction));
            }
        }

        protected override void DeleteFromDatabase(FinancialTransaction transaction)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                if (RelatedTransactions != null && RelatedTransactions.Contains(transaction))
                {
                    RelatedTransactions.Remove(transaction);
                }

                AllTransactions.Remove(transaction);
                dbConn.Delete(transaction);

                transaction.Amount = -transaction.Amount;

                accountViewModel.AddTransactionAmount(transaction);
            }
        }

        public void DeleteAssociatedTransactionsFromDatabase(int accountId)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                if (AllTransactions == null)
                {
                    AllTransactions = new ObservableCollection<FinancialTransaction>();
                }

                var transactions = dbConn.Table<FinancialTransaction>()
                    .Where(x => x.ChargedAccountId == accountId)
                    .ToList();

                foreach (var transaction in transactions)
                {
                    AllTransactions.Remove(transaction);
                    dbConn.Delete(transaction);
                }
            }
        }

        protected override void GetListFromDb()
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                AllTransactions = new ObservableCollection<FinancialTransaction>
                    (dbConn.Table<FinancialTransaction>().ToList());
            }
        }

        public void GetRelatedTransactions()
        {
            var accountId = new ViewModelLocator().AccountViewModel.SelectedAccount.Id;
            RelatedTransactions = new ObservableCollection<FinancialTransaction>(
                AllTransactions
                    .Where(x => x.ChargedAccountId == accountId).ToList());
        }

        protected override void UpdateItem(FinancialTransaction transaction)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                dbConn.Update(transaction);
            }
        }

        public void ClearTransaction()
        {
            var transactions = GetUnclearedTransactions();
            foreach (var transaction in transactions)
            {
                accountViewModel.AddTransactionAmount(transaction);
            }
        }

        public List<FinancialTransaction> GetUnclearedTransactions()
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                return dbConn.Table<FinancialTransaction>().Where(x => x.Cleared == false
                    && x.Date <= DateTime.Now).ToList();
            }
        }
    }
}