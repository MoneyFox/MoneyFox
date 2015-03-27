using System;
using System.Collections.ObjectModel;
using System.IO;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.Repositories {
    public class RecurringTransactionRepository :IRecurringTransactionRepository {
        private readonly IDataAccess<RecurringTransaction> _dataAccess;

        private ObservableCollection<RecurringTransaction> _data;

        public RecurringTransactionRepository(IDataAccess<RecurringTransaction> dataAccess) {
            _dataAccess = dataAccess;
            _data = new ObservableCollection<RecurringTransaction>(_dataAccess.LoadList());
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
                throw new InvalidDataException("charged accout is missing");
            }

            if (item.Id == 0) {
                _data.Add(item);
            }
            _dataAccess.Save(item);
        }

        public void Delete(RecurringTransaction item) {
            _data.Remove(item);
            _dataAccess.Delete(item);
        }
    }
}
