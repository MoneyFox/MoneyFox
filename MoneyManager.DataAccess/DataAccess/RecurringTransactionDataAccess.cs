using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using PropertyChanged;
using SQLite.Net;

namespace MoneyManager.DataAccess.DataAccess
{
    [ImplementPropertyChanged]
    public class RecurringTransactionDataAccess : AbstractDataAccess<RecurringTransaction>
    {
        public ObservableCollection<RecurringTransaction> AllRecurringTransactions { get; set; }

        public RecurringTransaction SelectedRecurringTransaction { get; set; }

        protected override void SaveToDb(RecurringTransaction itemToAdd)
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
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
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                if (AllRecurringTransactions != null)
                {
                    AllRecurringTransactions.Remove(itemToDelete);
                }

                dbConn.Delete(itemToDelete);
            }
        }


        public void Delete(int reccuringTransactionId)
        {
            var recTrans = AllRecurringTransactions.FirstOrDefault(x => x.Id == reccuringTransactionId);
            if (recTrans != null)
            {
                Delete(recTrans);
            }
        }

        protected override void GetListFromDb()
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                AllRecurringTransactions =
                    new ObservableCollection<RecurringTransaction>(dbConn.Table<RecurringTransaction>().ToList());
            }
        }

        protected override void UpdateItem(RecurringTransaction itemToUpdate)
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                dbConn.Update(itemToUpdate);
                LoadList();
            }
        }

        public void Save(FinancialTransaction transaction, RecurringTransaction recurringTransaction)
        {
            if (AllRecurringTransactions != null)
            {
                AllRecurringTransactions.Add(recurringTransaction);
            }

            Save(recurringTransaction);
            transaction.ReccuringTransactionId = recurringTransaction.Id;
        }


        public void Update(FinancialTransaction transaction, RecurringTransaction recurringTransaction)
        {
            if (!transaction.ReccuringTransactionId.HasValue)
            {
                Save(recurringTransaction);
            }
            else
            {
                recurringTransaction.Id = transaction.ReccuringTransactionId.Value;
                Update(recurringTransaction);
            }
        }
    }
}