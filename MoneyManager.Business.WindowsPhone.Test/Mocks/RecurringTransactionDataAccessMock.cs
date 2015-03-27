using System.Collections.Generic;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.WindowsPhone.Test.Mocks {
    public class RecurringTransactionDataAccessMock : IDataAccess<RecurringTransaction> {
        public List<RecurringTransaction> FinancialTransactionTestList = new List<RecurringTransaction>();

        public void Save(RecurringTransaction itemToSave) {
            FinancialTransactionTestList.Add(itemToSave);
        }

        public void Delete(RecurringTransaction item) {
            if (FinancialTransactionTestList.Contains(item)) {
                FinancialTransactionTestList.Remove(item);
            }
        }

        public List<RecurringTransaction> LoadList() {
            return new List<RecurringTransaction>();
        }

    }
}
