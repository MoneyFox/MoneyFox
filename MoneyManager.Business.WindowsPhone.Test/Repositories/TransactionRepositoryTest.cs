using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.Repositories;
using MoneyManager.Business.WindowsPhone.Test.Mocks;
using MoneyManager.DataAccess;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using SQLiteNetExtensions.Extensions;

namespace MoneyManager.Business.WindowsPhone.Test.Repositories {
    [TestClass]
    public class TransactionRepositoryTest {
        private TransactionDataAccessMock _transactionDataAccessMock;

        [TestInitialize]
        public void Init() {
            _transactionDataAccessMock = new TransactionDataAccessMock();
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void TransactionRepository_LoadDataFromDbThroughRepository() {
            using (var db = SqlConnectionFactory.GetSqlConnection()) {
                db.DeleteAll<FinancialTransaction>();
                db.InsertWithChildren(new FinancialTransaction {
                    Amount = 999, 
                    AmountWithoutExchange = 777, 
                    ChargedAccount = new Account {
                        Name = "testAccount"
                    }
                });
            }

            var repository = new TransactionRepository(new TransactionDataAccess());

            Assert.IsTrue(repository.Data.Any());
            Assert.AreEqual(999, repository.Data[0].Amount);
            Assert.AreEqual(777, repository.Data[0].AmountWithoutExchange);
        }

        [TestMethod]
        public void TransactionRepository_SaveWithouthAccount() {
            try {
                var repository = new TransactionRepository(_transactionDataAccessMock);

                var transaction = new FinancialTransaction {
                    Amount = 20,
                    AmountWithoutExchange = 20
                };

                repository.Save(transaction);
            }
            catch (ArgumentException) {
            }
            catch (Exception) {
                Assert.Fail("wrong exception.");
            }
        }

        [TestMethod]
        public void TransactionRepository_Save() {
            var repository = new TransactionRepository(_transactionDataAccessMock);

            var account = new Account {
                Name = "TestAccount"
            };

            var transaction = new FinancialTransaction {
                ChargedAccount = account,
                Amount = 20,
                AmountWithoutExchange = 20
            };

            repository.Save(transaction);

            Assert.IsTrue(transaction == _transactionDataAccessMock.FinancialTransactionTestList[0]);
            Assert.IsTrue(account == _transactionDataAccessMock.FinancialTransactionTestList[0].ChargedAccount);
        }

        [TestMethod]
        public void TransactionRepository_SaveTransfer() {
            var repository = new TransactionRepository(_transactionDataAccessMock);

            var account = new Account {
                Name = "TestAccount"
            };

            var targetAccount = new Account {
                Name = "targetAccount"
            };

            var transaction = new FinancialTransaction {
                ChargedAccount = account,
                TargetAccount = targetAccount,
                Amount = 20,
                AmountWithoutExchange = 20,
                Type = (int) TransactionType.Transfer
            };

            repository.Save(transaction);

            Assert.IsTrue(transaction == _transactionDataAccessMock.FinancialTransactionTestList[0]);
            Assert.IsTrue(account == _transactionDataAccessMock.FinancialTransactionTestList[0].ChargedAccount);
            Assert.IsTrue(targetAccount == _transactionDataAccessMock.FinancialTransactionTestList[0].TargetAccount);
        }

        [TestMethod]
        public void TransactionRepository_Delete() {
            var repository = new TransactionRepository(_transactionDataAccessMock);

            var account = new Account {
                Name = "TestAccount"
            };

            var transaction = new FinancialTransaction {
                ChargedAccount = account,
                Amount = 20,
                AmountWithoutExchange = 20
            };

            repository.Save(transaction);
            Assert.AreSame(transaction, _transactionDataAccessMock.FinancialTransactionTestList[0]);

            repository.Delete(transaction);

            Assert.IsFalse(_transactionDataAccessMock.FinancialTransactionTestList.Any());
            Assert.IsFalse(repository.Data.Any());
        }

        [TestMethod]
        public void TransactionRepository_AccessCache() {
            Assert.IsNotNull(new TransactionRepository(_transactionDataAccessMock).Data);
        }

        [TestMethod]
        public void TransactionRepository_AddMultipleToCache() {
            var repository = new TransactionRepository(_transactionDataAccessMock);

            var account = new Account {
                Name = "TestAccount"
            };

            var transaction = new FinancialTransaction {
                ChargedAccount = account,
                Amount = 20,
                AmountWithoutExchange = 20
            };

            var secondTransaction = new FinancialTransaction {
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

        [TestMethod]
        public void TransactionRepository_AddItemToDataList() {
            var repository = new TransactionRepository(_transactionDataAccessMock);

            var account = new Account {
                Name = "TestAccount"
            };

            var transaction = new FinancialTransaction {
                ChargedAccount = account,
                Amount = 20,
                AmountWithoutExchange = 20,
                Type = (int) TransactionType.Transfer
            };

            repository.Save(transaction);
            Assert.IsTrue(repository.Data.Contains(transaction));
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void TransactionRepository_Update() {
            using (var db = SqlConnectionFactory.GetSqlConnection()) {
                db.DeleteAll<FinancialTransaction>();
            }

            var repository = new TransactionRepository(new TransactionDataAccess());
            var account = new Account {
                Name = "TestAccount"
            };

            var transaction = new FinancialTransaction {
                ChargedAccount = account,
                Amount = 20,
                AmountWithoutExchange = 20
            };

            repository.Save(transaction);
            Assert.AreEqual(1, repository.Data.Count);
            Assert.AreSame(transaction, repository.Data[0]);

            transaction.Amount = 30;

            repository.Save(transaction);

            Assert.AreEqual(1, repository.Data.Count);
            Assert.AreEqual(30, repository.Data[0].Amount);
        }
    }
}