using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using PropertyChanged;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MoneyManager.DataAccess.DataAccess
{
    [ImplementPropertyChanged]
    public class TransactionDataAccess : AbstractDataAccess<FinancialTransaction>
    {
        public ObservableCollection<FinancialTransaction> AllTransactions { get; set; }

        public FinancialTransaction SelectedTransaction { get; set; }

        protected override void SaveToDb(FinancialTransaction transaction)
        {
            SaveToDb(transaction, false);
        }

        public void SaveToDb(FinancialTransaction transaction, bool skipRecurring)
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                if (AllTransactions == null)
                {
                    LoadList();
                }

                AllTransactions.Add(transaction);
                AllTransactions = new ObservableCollection<FinancialTransaction>(AllTransactions.OrderByDescending(x => x.Date));

                dbConn.Insert(transaction, typeof (FinancialTransaction));
            }
        }

        protected override void DeleteFromDatabase(FinancialTransaction transaction)
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                AllTransactions.Remove(transaction);
                dbConn.Delete(transaction);
            }
        }

        protected override void GetListFromDb()
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                AllTransactions =
                    new ObservableCollection<FinancialTransaction>(dbConn.Table<FinancialTransaction>().ToList());
            }
        }

        public IEnumerable<FinancialTransaction> GetRelatedTransactions(int accountId)
        {
            if (AllTransactions == null)
            {
                LoadList();
            }

            return AllTransactions
                .Where(x => x.ChargedAccountId == accountId || x.TargetAccountId == accountId)
                .ToList();
        }

        protected override void UpdateItem(FinancialTransaction transaction)
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                dbConn.Update(transaction);
            }
        }

        public IEnumerable<FinancialTransaction> GetUnclearedTransactions()
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                return dbConn.Table<FinancialTransaction>().Where(x => x.Cleared == false
                                                                       && x.Date <= DateTime.Now).ToList();
            }
        }

        public List<FinancialTransaction> LoadRecurringList()
        {
            if (AllTransactions == null)
            {
                LoadList();
            }

            return AllTransactions
                .Where(x => x.IsRecurring)
                .Where(x => x.RecurringTransaction != null)
                .Where(x => x.RecurringTransaction.IsEndless || x.RecurringTransaction.EndDate >= DateTime.Now.Date)
                .ToList();
        }
    }
}