using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.Repositories {
    public class TransactionRepository : ITransactionRepository {
        private readonly IDataAccess<FinancialTransaction> _dataAccess;
        private ObservableCollection<FinancialTransaction> _data;

        /// <summary>
        ///     Creates a TransactionRepository Object
        /// </summary>
        /// <param name="dataAccess">Instanced financial transaction data Access</param>
        public TransactionRepository(IDataAccess<FinancialTransaction> dataAccess) {
            _dataAccess = dataAccess;
            _data = new ObservableCollection<FinancialTransaction>(_dataAccess.LoadList());
        }

        /// <summary>
        ///     cached transaction data
        /// </summary>
        public ObservableCollection<FinancialTransaction> Data {
            get { return _data ?? (_data = new ObservableCollection<FinancialTransaction>(_dataAccess.LoadList())); }
            set {
                if (_data == null) {
                    _data = new ObservableCollection<FinancialTransaction>(_dataAccess.LoadList());
                }
                if (Equals(_data, value)) {
                    return;
                }
                _data = value;
            }
        }

        public FinancialTransaction Selected { get; set; }

        /// <summary>
        ///     Save a new item or update an existin one.
        /// </summary>
        /// <param name="item">item to save</param>
        public void Save(FinancialTransaction item) {
            if (item.ChargedAccount == null) {
                throw new InvalidDataException("charged accout is missing");
            }

            if (item.Id == 0) {
                _data.Add(item);
            }
            _dataAccess.Save(item);
        }

        /// <summary>
        ///     Deletes the passed item and removes the item from cache
        /// </summary>
        /// <param name="item">item to delete</param>
        public void Delete(FinancialTransaction item) {
            _data.Remove(item);
            _dataAccess.Delete(item);
        }

        /// <summary>
        ///     Returns all transaction with date before today
        /// </summary>
        /// <returns>list of uncleared transactions</returns>
        public IEnumerable<FinancialTransaction> GetUnclearedTransactions() {
            return GetUnclearedTransactions(DateTime.Today);
        }

        /// <summary>
        ///     Returns all transaction with date in this month
        /// </summary>
        /// <returns>list of uncleared transactions</returns>
        public IEnumerable<FinancialTransaction> GetUnclearedTransactions(DateTime date) {
            return Data.Where(x => x.Cleared == false
                                   && x.Date.Date <= date.Date).ToList();
        }

        /// <summary>
        ///     returns a list with transaction who is related to this account
        /// </summary>
        /// <param name="accountId">Id of the account</param>
        /// <returns>List of transactions</returns>
        public IEnumerable<FinancialTransaction> GetRelatedTransactions(int accountId) {
            return Data
                .Where(x => x.ChargedAccountId == accountId || x.TargetAccountId == accountId)
                .OrderByDescending(x => x.Date)
                .ToList();
        }

        /// <summary>
        ///     returns a list with transaction who recure in a given timeframe
        /// </summary>
        /// <returns>list of recurring transactions</returns>
        public List<FinancialTransaction> LoadRecurringList() {
            return Data
                .Where(x => x.IsRecurring)
                .Where(x => x.RecurringTransaction != null)
                .Where(x => x.RecurringTransaction.IsEndless || x.RecurringTransaction.EndDate >= DateTime.Now.Date)
                .ToList();
        }
    }
}