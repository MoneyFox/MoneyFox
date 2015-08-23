using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyManager.Business.DataAccess;
using MoneyManager.Business.Repositories;
using MoneyManager.Core.Tests.Mocks;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Tests.Repositories
{
    [TestClass]
    public class RecurringTransactionRepositoryTest
    {
        [TestMethod]
        public void RecurringTransactionRepository_Save()
        {
            var recurringTransactionDataAccessMock = new RecurringTransactionDataAccessMock();
            var repository = new RecurringTransactionRepository(recurringTransactionDataAccessMock);

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

            Assert.IsTrue(transaction == recurringTransactionDataAccessMock.RecurringTransactionTestList[0]);
            Assert.IsTrue(account == recurringTransactionDataAccessMock.RecurringTransactionTestList[0].ChargedAccount);
        }

        [TestMethod]
        public void TransactionRepository_SaveWithouthAccount()
        {
            try
            {
                var recurringTransactionDataAccessMock = new RecurringTransactionDataAccessMock();
                var repository = new RecurringTransactionRepository(recurringTransactionDataAccessMock);

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
            var recurringTransactionDataAccessMock = new RecurringTransactionDataAccessMock();
            var repository = new RecurringTransactionRepository(recurringTransactionDataAccessMock);

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
            Assert.AreSame(transaction, recurringTransactionDataAccessMock.RecurringTransactionTestList[0]);

            repository.Delete(transaction);

            Assert.IsFalse(recurringTransactionDataAccessMock.RecurringTransactionTestList.Any());
            Assert.IsFalse(repository.Data.Any());
        }

        [TestMethod]
        public void RecurringTransactionRepository_AccessCache()
        {
            Assert.IsNotNull(new RecurringTransactionRepository(new RecurringTransactionDataAccessMock()).Data);
        }

        [TestMethod]
        public void RecurringTransactionRepository_AddMultipleToCache()
        {
            var recurringTransactionDataAccessMock = new RecurringTransactionDataAccessMock();
            var repository = new RecurringTransactionRepository(recurringTransactionDataAccessMock);

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
    }
}