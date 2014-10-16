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

        private AddTransactionViewModel addTransactionView
        {
            get { return ServiceLocator.Current.GetInstance<AddTransactionViewModel>(); }
        }

        public void Save(FinancialTransaction transaction)
        {
            var recurringTransaction = GetRecurringFromFinancialTransaction(transaction);

            if (AllRecurringTransactions != null)
            {
                AllRecurringTransactions.Add(recurringTransaction);
            }

            SaveToDb(recurringTransaction);
            transaction.ReccuringTransactionId = recurringTransaction.Id;
        }

        private RecurringTransaction GetRecurringFromFinancialTransaction(FinancialTransaction transaction)
        {
            return new RecurringTransaction
            {
                ChargedAccountId = transaction.ChargedAccountId,
                StartDate = transaction.Date,
                EndDate = addTransactionView.EndDate,
                IsEndless = addTransactionView.IsEndless,
                Amount = transaction.Amount,
                Currency = transaction.CurrencyCulture,
                CategoryId = transaction.CategoryId,
                Type = transaction.Type,
                Recurrence = addTransactionView.Recurrence,
                Note = transaction.Note,
            };
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

                dbConn.Delete(itemToDelete);
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
            var recTransaction = GetRecurringFromFinancialTransaction(transaction);
            recTransaction.Id = transaction.ReccuringTransactionId.Value;
            Update(recTransaction);
        }
    }
}