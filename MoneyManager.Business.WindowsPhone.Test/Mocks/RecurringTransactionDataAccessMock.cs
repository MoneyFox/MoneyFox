using System.Collections.Generic;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.WindowsPhone.Test.Mocks {
    public class RecurringTransactionDataAccessMock : IDataAccess<RecurringTransaction> {
        public List<RecurringTransaction> RecurringTransactionTestList = new List<RecurringTransaction>();

        public void Save(RecurringTransaction itemToSave) {
            RecurringTransactionTestList.Add(itemToSave);
        }

        public void Delete(RecurringTransaction item) {
            if (RecurringTransactionTestList.Contains(item)) {
                RecurringTransactionTestList.Remove(item);
            }
        }

        public List<RecurringTransaction> LoadList() {
            return new List<RecurringTransaction>();
        }
    }
}