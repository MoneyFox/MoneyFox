using Microsoft.Practices.ServiceLocation;
using MoneyManager.Models;
using MoneyManager.Src;
using MoneyManager.ViewModels;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.DataAccess
{
    [ImplementPropertyChanged]
    public class TransactionDataAccess : AbstractDataAccess<FinancialTransaction>
    {
        public ObservableCollection<FinancialTransaction> AllTransactions { get; set; }

        public ObservableCollection<FinancialTransaction> RelatedTransactions { get; set; }

        public FinancialTransaction SelectedTransaction { get; set; }

        private AccountDataAccess AccountDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        protected override void SaveToDb(FinancialTransaction transaction)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                if (AllTransactions == null)
                {
                    AllTransactions = new ObservableCollection<FinancialTransaction>();
                }

                new ViewModelLocator().AccountDataAccess.AddTransactionAmount(transaction);

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

                AccountDataAccess.AddTransactionAmount(transaction);
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
            var accountId = ServiceLocator.Current.GetInstance<AccountDataAccess>().SelectedAccount.Id;
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
                AccountDataAccess.AddTransactionAmount(transaction);
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