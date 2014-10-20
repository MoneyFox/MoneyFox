using BugSense.Core.Model;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Models;
using MoneyManager.Src;
using MoneyManager.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;

namespace MoneyManager.DataAccess
{
    public class RecurringTransactionDataAccess : AbstractDataAccess<RecurringTransaction>
    {
        public ObservableCollection<RecurringTransaction> AllRecurringTransactions { get; set; }

        public RecurringTransaction SelectedRecurringTransaction { get; set; }

        private TransactionDataAccess transactionData
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>(); }
        }

        public void Save(FinancialTransaction transaction)
        {
            var recurringTransaction = RecurringTransactionHelper.GetRecurringFromFinancialTransaction(transaction);

            if (AllRecurringTransactions != null)
            {
                AllRecurringTransactions.Add(recurringTransaction);
            }

            SaveToDb(recurringTransaction);
            transaction.ReccuringTransactionId = recurringTransaction.Id;
        }

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
                    (AllRecurringTransactions.OrderBy(x => x.StartDate));

                dbConn.Insert(itemToAdd, typeof(RecurringTransaction));
            }
        }

        protected override void DeleteFromDatabase(RecurringTransaction itemToDelete)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                if (AllRecurringTransactions != null)
                {
                    AllRecurringTransactions.Remove(itemToDelete);
                }

                RemoveRecurringForTransactions(itemToDelete);

                dbConn.Delete(itemToDelete);
            }
        }

        private void RemoveRecurringForTransactions(RecurringTransaction recTrans)
        {
            var relatedTrans = transactionData.AllTransactions.Where(x => x.IsRecurring && x.ReccuringTransactionId == recTrans.Id);

            foreach (var transaction in relatedTrans)
            {
                transaction.IsRecurring = false;
                transaction.ReccuringTransactionId = null;
                transactionData.Update(transaction);
            }
        }

        public void Delete(int reccuringTransactionId)
        {
            var recTrans = AllRecurringTransactions.FirstOrDefault(x => x.Id == reccuringTransactionId);
            if (recTrans != null)
            {
                Delete(recTrans, true);
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

        public void Update(FinancialTransaction transaction)
        {
            var recTransaction = RecurringTransactionHelper.GetRecurringFromFinancialTransaction(transaction);

            if (!transaction.ReccuringTransactionId.HasValue)
            {
                Save(recTransaction);
            }
            else
            {
                recTransaction.Id = transaction.ReccuringTransactionId.Value;
                Update(recTransaction);
            }
        }
    }
}