using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using SQLite.Net;

namespace MoneyManager.DataAccess.DataAccess
{
    internal class TransactionDataAccess : AbstractDataAccess<FinancialTransaction>
    {
        public TransactionDataAccess()
        {
            LoadList();
        }

        public ObservableCollection<FinancialTransaction> AllTransactions { get; set; }

        public FinancialTransaction SelectedTransaction { get; set; }

        private AccountDataAccess AccountDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        private RecurringTransactionDataAccess RecurringTransactionData
        {
            get { return ServiceLocator.Current.GetInstance<RecurringTransactionDataAccess>(); }
        }

        protected override void SaveToDb(FinancialTransaction transaction)
        {
            SaveToDb(transaction, false);
        }

        public void SaveToDb(FinancialTransaction transaction, bool skipRecurring)
        {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                if (AllTransactions == null)
                {
                    AllTransactions = new ObservableCollection<FinancialTransaction>();
                }

                AllTransactions.Add(transaction);
                AllTransactions = new ObservableCollection<FinancialTransaction>(AllTransactions.OrderBy(x => x.Date));

                dbConn.Insert(transaction, typeof (FinancialTransaction));
            }
        }

        protected override void DeleteFromDatabase(FinancialTransaction transaction)
        {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                AllTransactions.Remove(transaction);
                dbConn.Delete(transaction);
            }
        }


        protected override void GetListFromDb()
        {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                AllTransactions =
                    new ObservableCollection<FinancialTransaction>(dbConn.Table<FinancialTransaction>().ToList());
            }
        }

        public IEnumerable<FinancialTransaction> GetRelatedTransactions(int accountId)
        {
            return AllTransactions
                .Where(x => x.ChargedAccountId == accountId || x.TargetAccountId == accountId)
                .ToList();
        }

        protected override async void UpdateItem(FinancialTransaction transaction)
        {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                dbConn.Update(transaction);
            }
        }


        public IEnumerable<FinancialTransaction> GetUnclearedTransactions()
        {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                return dbConn.Table<FinancialTransaction>().Where(x => x.Cleared == false
                                                                       && x.Date <= DateTime.Now).ToList();
            }
        }

        public List<FinancialTransaction> LoadRecurringList()
        {
            using (SQLiteConnection db = SqlConnectionFactory.GetSqlConnection())
            {
                //Have to make a list before apply the where statements
                return db.Table<FinancialTransaction>()
                    .ToList()
                    .Where(x => x.IsRecurring)
                    .Where(x => x.RecurringTransaction.IsEndless || x.RecurringTransaction.EndDate >= DateTime.Now.Date)
                    .ToList();
            }
        }
    }
}