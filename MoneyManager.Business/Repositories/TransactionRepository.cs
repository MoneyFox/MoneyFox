using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.Repositories {
    public class TransactionRepository : ITransactionRepository {
        private ObservableCollection<FinancialTransaction> _data;
        private readonly IDataAccess<FinancialTransaction> _dataAccess;

        /// <summary>
        /// Creates a TransactionRepository Object
        /// </summary>
        /// <param name="dataAccess">Instanced financial transaction data Access</param>
        public TransactionRepository(IDataAccess<FinancialTransaction> dataAccess) {
            _dataAccess = dataAccess;
            _data = new ObservableCollection<FinancialTransaction>(_dataAccess.LoadList());
        }

        /// <summary>
        /// cached transaction data
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
        /// Save a new item or update an existin one.
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
        /// Deletes the passed item and removes the item from cache
        /// </summary>
        /// <param name="item">item to delete</param>
        public void Delete(FinancialTransaction item) {
            _data.Remove(item);
            _dataAccess.Delete(item);
        }

        public IEnumerable<FinancialTransaction> GetUnclearedTransactions() {
            return GetUnclearedTransactions(DateTime.Today);
        }

        public IEnumerable<FinancialTransaction> GetUnclearedTransactions(DateTime date) {
            return Data.Where(x => x.Cleared == false
                                   && x.Date.Date <= date.Date).ToList();
        }
    }
}
