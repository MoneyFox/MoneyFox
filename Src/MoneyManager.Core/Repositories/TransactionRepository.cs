using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;

namespace MoneyManager.Core.Repositories
{
    [ImplementPropertyChanged]
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IDataAccess<FinancialTransaction> dataAccess;
        private ObservableCollection<FinancialTransaction> data;

        /// <summary>
        ///     Creates a TransactionRepository Object
        /// </summary>
        /// <param name="dataAccess">Instanced financial transaction data Access</param>
        public TransactionRepository(IDataAccess<FinancialTransaction> dataAccess)
        {
            this.dataAccess = dataAccess;
            data = new ObservableCollection<FinancialTransaction>(this.dataAccess.LoadList());
        }

        /// <summary>
        ///     cached transaction data
        /// </summary>
        public ObservableCollection<FinancialTransaction> Data
        {
            get { return data ?? (data = new ObservableCollection<FinancialTransaction>(dataAccess.LoadList())); }
            set
            {
                if (data == null)
                {
                    data = new ObservableCollection<FinancialTransaction>(dataAccess.LoadList());
                }
                if (Equals(data, value))
                {
                    return;
                }
                data = value;
            }
        }

        /// <summary>
        ///     The currently selected Transaction
        /// </summary>
        public FinancialTransaction Selected { get; set; }

        /// <summary>
        ///     Save a new item or update an existin one.
        /// </summary>
        /// <param name="item">item to save</param>
        public void Save(FinancialTransaction item)
        {
            if (item.ChargedAccount == null)
            {
                throw new InvalidDataException("charged accout is missing");
            }

            if (item.Id == 0)
            {
                data.Add(item);
            }
            dataAccess.Save(item);
        }

        /// <summary>
        ///     Deletes the passed item and removes the item from cache
        /// </summary>
        /// <param name="item">item to delete</param>
        public void Delete(FinancialTransaction item)
        {
            data.Remove(item);
            dataAccess.Delete(item);
        }

        /// <summary>
        ///     Loads all transactions from the database to the data collection
        /// </summary>
        public void Load()
        {
            Data = new ObservableCollection<FinancialTransaction>(dataAccess.LoadList());
        }

        /// <summary>
        ///     Returns all transaction with date before today
        /// </summary>
        /// <returns>list of uncleared transactions</returns>
        public IEnumerable<FinancialTransaction> GetUnclearedTransactions()
        {
            return GetUnclearedTransactions(DateTime.Today);
        }

        /// <summary>
        ///     Returns all transaction with date in this month
        /// </summary>
        /// <returns>list of uncleared transactions</returns>
        public IEnumerable<FinancialTransaction> GetUnclearedTransactions(DateTime date)
        {
            return Data.Where(x => x.Cleared == false
                                   && x.Date.Date <= date.Date).ToList();
        }

        /// <summary>
        ///     returns a list with transaction who is related to this account
        /// </summary>
        /// <param name="account">account to search the related</param>
        /// <returns>List of transactions</returns>
        public IEnumerable<FinancialTransaction> GetRelatedTransactions(Account account)
        {
            return Data
                .Where(x => x.ChargedAccount != null)
                .Where(
                    x =>
                        x.ChargedAccount.Id == account.Id ||
                        (x.TargetAccount != null && x.TargetAccount.Id == account.Id))
                .OrderByDescending(x => x.Date)
                .ToList();
        }

        /// <summary>
        ///     returns a list with transaction who recure in a given timeframe
        /// </summary>
        /// <returns>list of recurring transactions</returns>
        public List<FinancialTransaction> LoadRecurringList()
        {
            return Data
                .Where(x => x.IsRecurring)
                .Where(x => x.RecurringTransaction != null)
                .Where(x => x.RecurringTransaction.IsEndless || x.RecurringTransaction.EndDate >= DateTime.Now.Date)
                .ToList();
        }
    }
}