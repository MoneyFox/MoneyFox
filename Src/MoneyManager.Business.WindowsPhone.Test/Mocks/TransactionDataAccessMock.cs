using System.Collections.Generic;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.WindowsPhone.Test.Mocks {
    public class TransactionDataAccessMock : IDataAccess<FinancialTransaction> {
        public List<FinancialTransaction> FinancialTransactionTestList = new List<FinancialTransaction>();

        public void Save(FinancialTransaction itemToSave) {
            FinancialTransactionTestList.Add(itemToSave);
        }

        public void Delete(FinancialTransaction item) {
            if (FinancialTransactionTestList.Contains(item)) {
                FinancialTransactionTestList.Remove(item);
            }
        }

        public List<FinancialTransaction> LoadList() {
            return new List<FinancialTransaction>();
        }
    }
}