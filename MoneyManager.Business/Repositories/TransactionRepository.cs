using System.Collections.ObjectModel;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.Repositories {
    public class TransactionRepository : ITransactionRepository {
        private ObservableCollection<FinancialTransaction> _data;
        private readonly IDataAccess<FinancialTransaction> _dataAccess;

        public TransactionRepository(IDataAccess<FinancialTransaction> dataAccess) {
            _dataAccess = dataAccess;
        }

        public ObservableCollection<FinancialTransaction> Data {
            get { return _data ?? (_data = new ObservableCollection<FinancialTransaction>(_dataAccess.LoadList())); }
            set {
                if (_data != null && _data == value) {
                    return;
                }
                _data = value;
            }
        }

        public FinancialTransaction Selected { get; set; }

        public void Save(FinancialTransaction item) {
            _dataAccess.Save(item);
        }

        public void Delete(FinancialTransaction item) {
            _dataAccess.Delete(item);
        }
    }
}
