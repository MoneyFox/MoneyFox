using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyManager.Core.Helper;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.Tests.Mocks;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Tests.Repositories
{
    [TestClass]
    public class TransactionRepositoryTests
    {
        [TestMethod]
        public void TransactionRepository_SaveWithouthAccount()
        {
            try
            {
                var transactionDataAccessMock = new TransactionDataAccessMock();
                var repository = new TransactionRepository(transactionDataAccessMock);

                var transaction = new FinancialTransaction
                {
                    Amount = 20
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
        public void TransactionRepository_Save()
        {
            var transactionDataAccessMock = new TransactionDataAccessMock();
            var repository = new TransactionRepository(transactionDataAccessMock);

            var account = new Account
            {
                Name = "TestAccount"
            };

            var transaction = new FinancialTransaction
            {
                ChargedAccount = account,
                Amount = 20
            };

            repository.Save(transaction);

            Assert.IsTrue(transaction == transactionDataAccessMock.FinancialTransactionTestList[0]);
            Assert.IsTrue(account == transactionDataAccessMock.FinancialTransactionTestList[0].ChargedAccount);
        }

        [TestMethod]
        public void TransactionRepository_SaveTransfer()
        {
            var transactionDataAccessMock = new TransactionDataAccessMock();
            var repository = new TransactionRepository(transactionDataAccessMock);

            var account = new Account
            {
                Name = "TestAccount"
            };

            var targetAccount = new Account
            {
                Name = "targetAccount"
            };

            var transaction = new FinancialTransaction
            {
                ChargedAccount = account,
                TargetAccount = targetAccount,
                Amount = 20,
                Type = (int) TransactionType.Transfer
            };

            repository.Save(transaction);

            Assert.IsTrue(transaction == transactionDataAccessMock.FinancialTransactionTestList[0]);
            Assert.IsTrue(account == transactionDataAccessMock.FinancialTransactionTestList[0].ChargedAccount);
            Assert.IsTrue(targetAccount == transactionDataAccessMock.FinancialTransactionTestList[0].TargetAccount);
        }

        [TestMethod]
        public void TransactionRepository_Delete()
        {
            var transactionDataAccessMock = new TransactionDataAccessMock();
            var repository = new TransactionRepository(transactionDataAccessMock);

            var account = new Account
            {
                Name = "TestAccount"
            };

            var transaction = new FinancialTransaction
            {
                ChargedAccount = account,
                Amount = 20,
            };

            repository.Save(transaction);
            Assert.AreSame(transaction, transactionDataAccessMock.FinancialTransactionTestList[0]);

            repository.Delete(transaction);

            Assert.IsFalse(transactionDataAccessMock.FinancialTransactionTestList.Any());
            Assert.IsFalse(repository.Data.Any());
        }

        [TestMethod]
        public void TransactionRepository_AccessCache()
        {
            Assert.IsNotNull(new TransactionRepository(new TransactionDataAccessMock()).Data);
        }

        [TestMethod]
        public void TransactionRepository_AddMultipleToCache()
        {
            var repository = new TransactionRepository(new TransactionDataAccessMock());

            var account = new Account
            {
                Name = "TestAccount"
            };

            var transaction = new FinancialTransaction
            {
                ChargedAccount = account,
                Amount = 20,
            };

            var secondTransaction = new FinancialTransaction
            {
                ChargedAccount = account,
                Amount = 60,
            };

            repository.Save(transaction);
            repository.Save(secondTransaction);

            Assert.AreEqual(2, repository.Data.Count);
            Assert.AreSame(transaction, repository.Data[0]);
            Assert.AreSame(secondTransaction, repository.Data[1]);
        }

        [TestMethod]
        public void TransactionRepository_AddItemToDataList()
        {
            var repository = new TransactionRepository(new TransactionDataAccessMock());

            var account = new Account
            {
                Name = "TestAccount"
            };

            var transaction = new FinancialTransaction
            {
                ChargedAccount = account,
                Amount = 20,
                Type = (int) TransactionType.Transfer
            };

            repository.Save(transaction);
            Assert.IsTrue(repository.Data.Contains(transaction));
        }

        [TestMethod]
        public void TransactionRepository_GetUnclearedTransactionsPast()
        {
            var repository = new TransactionRepository(new TransactionDataAccessMock());

            var account = new Account
            {
                Name = "TestAccount"
            };

            repository.Save(new FinancialTransaction
            {
                ChargedAccount = account,
                Amount = 55,
                Date = DateTime.Today.AddDays(-1),
                Note = "this is a note!!!",
                Cleared = false
            }
                );

            var transactions = repository.GetUnclearedTransactions();

            Assert.AreEqual(1, transactions.Count());
        }

        /// <summary>
        ///     This Test may fail if the date overlaps with the month transition.
        /// </summary>
        [TestMethod]
        public void TransactionRepository_GetUnclearedTransactionsFuture()
        {
            var repository = new TransactionRepository(new TransactionDataAccessMock());

            var account = new Account
            {
                Name = "TestAccount"
            };

            repository.Save(new FinancialTransaction
            {
                ChargedAccount = account,
                Amount = 55,
                Date = Utilities.GetEndOfMonth().AddDays(-1),
                Note = "this is a note!!!",
                Cleared = false
            }
                );

            var transactions = repository.GetUnclearedTransactions();
            Assert.AreEqual(0, transactions.Count());

            transactions = repository.GetUnclearedTransactions(Utilities.GetEndOfMonth());
            Assert.AreEqual(1, transactions.Count());
        }

        [TestMethod]
        public void TransactionRepository_GetUnclearedTransactions_AccountNull()
        {
            var repository = new TransactionRepository(new TransactionDataAccessMock());

            repository.Data.Add(new FinancialTransaction
            {
                Amount = 55,
                Date = DateTime.Today.AddDays(-1),
                Note = "this is a note!!!",
                Cleared = false
            }
                );

            var transactions = repository.GetUnclearedTransactions();

            Assert.AreEqual(1, transactions.Count());
        }
    }
}