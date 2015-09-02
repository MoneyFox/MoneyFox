using System.IO;
using System.Linq;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.Tests.Mocks;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using Xunit;

namespace MoneyManager.Core.Tests.Repositories
{
    public class RecurringTransactionRepositoryTest
    {
        [Fact]
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
                Amount = 20
            };

            repository.Save(transaction);

            recurringTransactionDataAccessMock.RecurringTransactionTestList[0].ShouldBeSameAs(transaction);
            recurringTransactionDataAccessMock.RecurringTransactionTestList[0].ChargedAccount.ShouldBeSameAs(account);
        }

        [Fact]
        public void TransactionRepository_SaveWithouthAccount()
        {
                var recurringTransactionDataAccessMock = new RecurringTransactionDataAccessMock();
                var repository = new RecurringTransactionRepository(recurringTransactionDataAccessMock);

                var transaction = new RecurringTransaction
                {
                    Amount = 20
                };

                Assert.Throws<InvalidDataException>(() => repository.Save(transaction));

        }

        [Fact]
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
                Amount = 20
            };

            repository.Save(transaction);
            recurringTransactionDataAccessMock.RecurringTransactionTestList[0].ShouldBeSameAs(transaction);

            repository.Delete(transaction);

            recurringTransactionDataAccessMock.RecurringTransactionTestList.Any().ShouldBeFalse();
            repository.Data.Any().ShouldBeFalse();
        }

        [Fact]
        public void RecurringTransactionRepository_AccessCache()
        {
            new RecurringTransactionRepository(new RecurringTransactionDataAccessMock()).Data.ShouldNotBeNull();
        }

        [Fact]
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
                Amount = 20
            };

            var secondTransaction = new RecurringTransaction
            {
                ChargedAccount = account,
                Amount = 60
            };

            repository.Save(transaction);
            repository.Save(secondTransaction);

            repository.Data.Count.ShouldBe(2);
            repository.Data[0].ShouldBeSameAs(transaction);
            repository.Data[1].ShouldBeSameAs(secondTransaction);
        }
    }
}