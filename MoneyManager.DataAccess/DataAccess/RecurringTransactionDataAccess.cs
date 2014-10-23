using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using SQLite.Net;

namespace MoneyManager.DataAccess.DataAccess
{
    internal class RecurringTransactionDataAccess : AbstractDataAccess<RecurringTransaction>
    {
        public ObservableCollection<RecurringTransaction> AllRecurringTransactions { get; set; }

        public RecurringTransaction SelectedRecurringTransaction { get; set; }

        public void Save(FinancialTransaction transaction)
        {
            //TODO: Refactor
            //var recurringTransaction = RecurringTransactionHelper.GetRecurringFromFinancialTransaction(transaction);

            //if (AllRecurringTransactions != null)
            //{
            //    AllRecurringTransactions.Add(recurringTransaction);
            //}

            //SaveToDb(recurringTransaction);
            //transaction.ReccuringTransactionId = recurringTransaction.Id;
        }

        protected override void SaveToDb(RecurringTransaction itemToAdd)
        {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                if (AllRecurringTransactions == null)
                {
                    AllRecurringTransactions = new ObservableCollection<RecurringTransaction>();
                }

                AllRecurringTransactions.Add(itemToAdd);
                AllRecurringTransactions = new ObservableCollection<RecurringTransaction>
                    (AllRecurringTransactions.OrderBy(x => x.StartDate));

                dbConn.Insert(itemToAdd, typeof (RecurringTransaction));
            }
        }

        protected override void DeleteFromDatabase(RecurringTransaction itemToDelete)
        {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                if (AllRecurringTransactions != null)
                {
                    AllRecurringTransactions.Remove(itemToDelete);
                }

                //TODO: refactor
                //RemoveRecurringForTransactions(itemToDelete);

                dbConn.Delete(itemToDelete);
            }
        }


        public void Delete(int reccuringTransactionId)
        {
            RecurringTransaction recTrans = AllRecurringTransactions.FirstOrDefault(x => x.Id == reccuringTransactionId);
            if (recTrans != null)
            {
                Delete(recTrans);
            }
        }

        protected override void GetListFromDb()
        {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                AllRecurringTransactions =
                    new ObservableCollection<RecurringTransaction>(dbConn.Table<RecurringTransaction>().ToList());
            }
        }

        protected override void UpdateItem(RecurringTransaction itemToUpdate)
        {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                dbConn.Update(itemToUpdate);
                LoadList();
            }
        }

        public void Update(FinancialTransaction transaction)
        {
            //todo: Refactor
            //var recTransaction = RecurringTransactionHelper.GetRecurringFromFinancialTransaction(transaction);

            //if (!transaction.ReccuringTransactionId.HasValue)
            //{
            //    Save(recTransaction);
            //}
            //else
            //{
            //    recTransaction.Id = transaction.ReccuringTransactionId.Value;
            //    Update(recTransaction);
            //}
        }
    }
}