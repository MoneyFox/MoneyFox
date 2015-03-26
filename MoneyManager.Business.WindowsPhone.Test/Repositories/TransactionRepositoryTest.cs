using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.Repositories;
using MoneyManager.Business.WindowsPhone.Test.Mocks;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Business.WindowsPhone.Test.Repositories {
    [TestClass]
    public class TransactionRepositoryTest {

        private TransactionDataAccessMock _transactionDataAccessMock;
            
        [TestInitialize]
        public void Init() {
            _transactionDataAccessMock = new TransactionDataAccessMock();
        }

        [TestMethod]
        [Ignore]
        [TestCategory("Integration")]
        public void TransactionRepository_LoadDataFromDbThroughRepository() {
        }

        [TestMethod]
        public void TransactionRepository_SaveWithouthAccount() {
            var repository = new TransactionRepository(_transactionDataAccessMock);

            var transaction = new FinancialTransaction {
                Amount = 20,
                AmountWithoutExchange = 20
            };

            repository.Save(transaction);

            Assert.AreEqual(20, _transactionDataAccessMock.FinancialTransactionTestList[0].Amount);
        }
    }
}
