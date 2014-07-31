using MoneyManager.DataAccess;
using MoneyManager.Models;
using MoneyTracker.Models;
using MoneyTracker.Src;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;

namespace MoneyTracker.ViewModels
{
    [ImplementPropertyChanged]
    public class RecurrenceTransactionDAO : AbstractDataAccess<RecurringTransaction>
    {
        public ObservableCollection<RecurringTransaction> AllTransactions { get; set; }

        protected override void SaveToDb(RecurringTransaction transaction)
        {
            if (AllTransactions == null)
            {
                AllTransactions = new ObservableCollection<RecurringTransaction>();
            }

            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                AllTransactions.Add(transaction);
                dbConn.Insert(transaction, typeof(RecurringTransaction));
            }
        }

        protected override void DeleteFromDatabase(RecurringTransaction transaction)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                AllTransactions.Remove(transaction);
                dbConn.Delete(transaction);
            }
        }

        protected override void GetListFromDb()
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                AllTransactions = new ObservableCollection<RecurringTransaction>(dbConn.Table<RecurringTransaction>().ToList());
            }
        }

        protected override void UpdateItem(RecurringTransaction transaction)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                dbConn.Update(transaction);
            }
        }
    }
}