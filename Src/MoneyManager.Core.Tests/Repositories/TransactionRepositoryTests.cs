using System;
using System.IO;
using System.Linq;
using MoneyManager.Core.Helper;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.Tests.Mocks;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using Xunit;

namespace MoneyManager.Core.Tests.Repositories
{
    public class TransactionRepositoryTests
    {
        [Fact]
        public void TransactionRepository_SaveWithouthAccount()
        {
            var transactionDataAccessMock = new TransactionDataAccessMock();
            var repository = new TransactionRepository(transactionDataAccessMock);

            var transaction = new FinancialTransaction
            {
                Amount = 20
            };

            Assert.Throws<InvalidDataException>(() => repository.Save(transaction));
        }

        [Fact]
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

            transactionDataAccessMock.FinancialTransactionTestList[0].ShouldBeSameAs(transaction);
            transactionDataAccessMock.FinancialTransactionTestList[0].ChargedAccount.ShouldBeSameAs(account);
        }

        [Fact]
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

            transactionDataAccessMock.FinancialTransactionTestList[0].ShouldBeSameAs(transaction);
            transactionDataAccessMock.FinancialTransactionTestList[0].ChargedAccount.ShouldBeSameAs(account);
            transactionDataAccessMock.FinancialTransactionTestList[0].TargetAccount.ShouldBeSameAs(targetAccount);
        }

        [Fact]
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
            transactionDataAccessMock.FinancialTransactionTestList[0].ShouldBeSameAs(transaction);

            repository.Delete(transaction);

            transactionDataAccessMock.FinancialTransactionTestList.Any().ShouldBeFalse();
            repository.Data.Any().ShouldBeFalse();
        }

        [Fact]
        public void TransactionRepository_AccessCache()
        {
            new TransactionRepository(new TransactionDataAccessMock()).Data.ShouldNotBeNull();
        }

        [Fact]
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

            repository.Data.Count.ShouldBe(2);
            repository.Data[0].ShouldBeSameAs(transaction);
            repository.Data[1].ShouldBeSameAs(secondTransaction);
        }

        [Fact]
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
            repository.Data.Contains(transaction).ShouldBeTrue();
        }

        [Fact]
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

            transactions.Count().ShouldBe(1);
        }

        /// <summary>
        ///     This Test may fail if the date overlaps with the month transition.
        /// </summary>
        [Fact]
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
            transactions.Count().ShouldBe(0);

            transactions = repository.GetUnclearedTransactions(Utilities.GetEndOfMonth());
            transactions.Count().ShouldBe(1);
        }

        [Fact]
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
            transactions.Count().ShouldBe(1);
        }
    }
}