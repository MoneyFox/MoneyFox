using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.Models;
using MoneyManager.Src;

namespace MoneyManager.DataAccess
{
    public class RecurringTransactionDataAccess : AbstractDataAccess<RecurringTransaction>
    {
        public ObservableCollection<RecurringTransaction> AllRecurringTransactions { get; set; }

        protected override void SaveToDb(RecurringTransaction itemToAdd)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                if (AllRecurringTransactions == null)
                {
                    AllRecurringTransactions = new ObservableCollection<RecurringTransaction>();
                }

                AllRecurringTransactions.Add(itemToAdd);
                AllRecurringTransactions = new ObservableCollection<RecurringTransaction>
                    (AllRecurringTransactions.OrderBy(x => x.Date));

                dbConn.Insert(itemToAdd, typeof(RecurringTransaction));
            }
        }

        protected override void DeleteFromDatabase(RecurringTransaction itemToDelete)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                AllRecurringTransactions.Remove(itemToDelete);
                dbConn.Delete(itemToDelete);
            }
        }

        protected override void GetListFromDb()
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                AllRecurringTransactions = new ObservableCollection<RecurringTransaction>
                    (dbConn.Table<RecurringTransaction>().ToList());
            }
        }

        protected override void UpdateItem(RecurringTransaction itemToUpdate)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                dbConn.Update(itemToUpdate);
            }
        }
    }
}
