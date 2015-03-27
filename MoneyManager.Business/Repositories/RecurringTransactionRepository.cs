using System;
using System.Collections.ObjectModel;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.Repositories {
    public class RecurringTransactionRepository :IRecurringTransactionRepository {
        private readonly IDataAccess<RecurringTransaction> _dataAccess;

        private ObservableCollection<RecurringTransaction> _data;

        public RecurringTransactionRepository(IDataAccess<RecurringTransaction> dataAccess) {
            _dataAccess = dataAccess;
        }

        public ObservableCollection<RecurringTransaction> Data {
            get { return _data ?? (_data = new ObservableCollection<RecurringTransaction>(_dataAccess.LoadList())); }
            set {
                if (_data == null) {
                    _data = new ObservableCollection<RecurringTransaction>(_dataAccess.LoadList());
                }
                if (Equals(_data, value)) {
                    return;
                }
                _data = value;
            }
        }

        public RecurringTransaction Selected { get; set; }

        public void Save(RecurringTransaction item) {
            if (item.ChargedAccount == null) {
                throw new ArgumentException("charged accout is missing");
            }

            if (_data == null) {
                _data = new ObservableCollection<RecurringTransaction>(_dataAccess.LoadList());
            }

            if (item.Id == 0) {
                _data.Add(item);
            }
            _dataAccess.Save(item);
        }

        public void Delete(RecurringTransaction item) {
            if (_data == null) {
                _data = new ObservableCollection<RecurringTransaction>(_dataAccess.LoadList());
            }

            _data.Remove(item);
            _dataAccess.Delete(item);
        }
    }
}
