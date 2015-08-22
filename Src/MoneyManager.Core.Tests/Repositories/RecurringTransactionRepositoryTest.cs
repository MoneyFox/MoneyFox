using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.DataAccess;
using MoneyManager.Business.Repositories;
using MoneyManager.Business.WindowsPhone.Test.Mocks;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Business.WindowsPhone.Test.Repositories
{
    [TestClass]
    public class RecurringTransactionRepositoryTest
    {
        private RecurringTransactionDataAccessMock _recurringTransactionDataAccessMock;

        [TestInitialize]
        public void Init()
        {
            _recurringTransactionDataAccessMock = new RecurringTransactionDataAccessMock();
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void RecurringTransactionRepository_LoadDataFromDbThroughRepository()
        {
            using (var db = SqlConnectionFactory.GetSqlConnection())
            {
                db.DeleteAll<RecurringTransaction>();
                db.InsertWithChildren(new RecurringTransaction
                {
                    Amount = 999,
                    AmountWithoutExchange = 777,
                    ChargedAccount = new Account
                    {
                        Name = "testAccount"
                    }
                });
            }

            var repository = new RecurringTransactionRepository(new RecurringTransactionDataAccess());

            Assert.IsTrue(repository.Data.Any());
            Assert.AreEqual(999, repository.Data[0].Amount);
            Assert.AreEqual(777, repository.Data[0].AmountWithoutExchange);
        }

        [TestMethod]
        public void RecurringTransactionRepository_Save()
        {
            var repository = new RecurringTransactionRepository(_recurringTransactionDataAccessMock);

            var account = new Account
            {
                Name = "TestAccount"
            };

            var transaction = new RecurringTransaction
            {
                ChargedAccount = account,
                Amount = 20,
                AmountWithoutExchange = 20
            };

            repository.Save(transaction);

            Assert.IsTrue(transaction == _recurringTransactionDataAccessMock.RecurringTransactionTestList[0]);
            Assert.IsTrue(account == _recurringTransactionDataAccessMock.RecurringTransactionTestList[0].ChargedAccount);
        }

        [TestMethod]
        public void TransactionRepository_SaveWithouthAccount()
        {
            try
            {
                var repository = new RecurringTransactionRepository(_recurringTransactionDataAccessMock);

                var transaction = new RecurringTransaction
                {
                    Amount = 20,
                    AmountWithoutExchange = 20
                };

                repository.Save(transaction);
            }
            catch (InvalidDataException)
            {
                return;
            }
            catch (Exception)
            {
                Assert.Fail("wrong exception.");
            }
            Assert.Fail("No excpetion thrown");
        }

        [TestMethod]
        public void RecurringTransactionRepository_Delete()
        {
            var repository = new RecurringTransactionRepository(_recurringTransactionDataAccessMock);

            var account = new Account
            {
                Name = "TestAccount"
            };

            var transaction = new RecurringTransaction
            {
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
        public void RecurringTransactionRepository_AccessCache()
        {
            Assert.IsNotNull(new RecurringTransactionRepository(_recurringTransactionDataAccessMock).Data);
        }

        [TestMethod]
        public void RecurringTransactionRepository_AddMultipleToCache()
        {
            var repository = new RecurringTransactionRepository(_recurringTransactionDataAccessMock);

            var account = new Account
            {
                Name = "TestAccount"
            };

            var transaction = new RecurringTransaction
            {
                ChargedAccount = account,
                Amount = 20,
                AmountWithoutExchange = 20
            };

            var secondTransaction = new RecurringTransaction
            {
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
        public void RecurringTransactionRepository_Update()
        {
            using (var db = SqlConnectionFactory.GetSqlConnection())
            {
                db.DeleteAll<RecurringTransaction>();
            }

            var repository = new RecurringTransactionRepository(new RecurringTransactionDataAccess());

            var account = new Account
            {
                Name = "TestAccount"
            };

            var transaction = new RecurringTransaction
            {
                ChargedAccount = account,
                Amount = 20,
                AmountWithoutExchange = 20
            };


            repository.Save(transaction);
            Assert.AreEqual(1, repository.Data.Count);
            Assert.AreSame(transaction, repository.Data[0]);

            transaction.Amount = 789;

            repository.Save(transaction);

            Assert.AreEqual(1, repository.Data.Count);
            Assert.AreEqual(789, repository.Data[0].Amount);
        }
    }
}