using System;
using System.Collections.ObjectModel;
using System.IO;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.Repositories {
    public class TransactionRepository : ITransactionRepository {
        private ObservableCollection<FinancialTransaction> _data;
        private readonly IDataAccess<FinancialTransaction> _dataAccess;

        public TransactionRepository(IDataAccess<FinancialTransaction> dataAccess) {
            _dataAccess = dataAccess;
            _data = new ObservableCollection<FinancialTransaction>(_dataAccess.LoadList());
        }

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

        public void Save(FinancialTransaction item) {
            if (item.ChargedAccount == null) {
                throw new InvalidDataException("charged accout is missing");
            }

            if (item.Id == 0) {
                _data.Add(item);
            }
           _dataAccess.Save(item);
        }

        public void Delete(FinancialTransaction item) {
            _data.Remove(item);
            _dataAccess.Delete(item);
        }
    }
}
