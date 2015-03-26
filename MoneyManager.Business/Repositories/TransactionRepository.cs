using System.Collections.ObjectModel;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.Repositories {
    public class TransactionRepository : IRepository<FinancialTransaction> {
        public ObservableCollection<FinancialTransaction> Data {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public FinancialTransaction Selected {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public void Save(FinancialTransaction item) {
            throw new System.NotImplementedException();
        }

        public void Delete(FinancialTransaction item) {
            throw new System.NotImplementedException();
        }
    }
}
