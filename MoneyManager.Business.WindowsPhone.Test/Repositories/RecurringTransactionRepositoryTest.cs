using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.Repositories;
using MoneyManager.Business.WindowsPhone.Test.Mocks;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Business.WindowsPhone.Test.Repositories {
    [TestClass]
    public class RecurringTransactionRepositoryTest {
        private RecurringTransactionDataAccessMock _recurringTransactionDataAccessMock;

        [TestInitialize]
        public void Init() {
            _recurringTransactionDataAccessMock = new RecurringTransactionDataAccessMock();
        }

        [TestMethod]
        public void RecurringTransactionRepository_Save() {
            var repository = new RecurringTransactionRepository(_recurringTransactionDataAccessMock);

            var account = new Account {
                Name = "TestAccount"
            };

            var transaction = new RecurringTransaction {
                ChargedAccount = account,
                Amount = 20,
                AmountWithoutExchange = 20
            };

            repository.Save(transaction);

            Assert.IsTrue(transaction == _recurringTransactionDataAccessMock.RecurringTransactionTestList[0]);
            Assert.IsTrue(account == _recurringTransactionDataAccessMock.RecurringTransactionTestList[0].ChargedAccount);
        }

        [TestMethod]
        public void RecurringTransactionRepository_Delete() {
            var repository = new RecurringTransactionRepository(_recurringTransactionDataAccessMock);

            var account = new Account {
                Name = "TestAccount"
            };

            var transaction = new RecurringTransaction {
                ChargedAccount = account,
                Amount = 20,
                AmountWithoutExchange = 20
            };

            repository.Save(transaction);
            Assert.AreSame(transaction, _recurringTransactionDataAccessMock.RecurringTransactionTestList[0]);

            repository.Delete(transaction);

            Assert.IsFalse(_recurringTransactionDataAccessMock.RecurringTransactionTestList.Any());
            Assert.IsFalse(repository.Data.Any());
        }

        [TestMethod]
        public void RecurringTransactionRepository_AccessCache() {
            Assert.IsNotNull(new RecurringTransactionRepository(_recurringTransactionDataAccessMock).Data);
        }

        [TestMethod]
        public void RecurringTransactionRepository_AddMultipleToCache() {
            var repository = new RecurringTransactionRepository(_recurringTransactionDataAccessMock);

            var account = new Account {
                Name = "TestAccount"
            };

            var transaction = new RecurringTransaction {
                ChargedAccount = account,
                Amount = 20,
                AmountWithoutExchange = 20
            };

            var secondTransaction = new RecurringTransaction {
                ChargedAccount = account,
                Amount = 60,
                AmountWithoutExchange = 60
            };

            repository.Save(transaction);
            repository.Save(secondTransaction);

            Assert.AreEqual(2, repository.Data.Count);
            Assert.AreSame(transaction, repository.Data[0]);
            Assert.AreSame(secondTransaction, repository.Data[1]);
        }

    }
}
