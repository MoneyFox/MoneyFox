using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using MoneyManager.Core.Helpers;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.Tests.Mocks;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Exceptions;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.TestFoundation;
using Moq;
using Xunit;

namespace MoneyManager.Core.Tests.Repositories
{
    public class PaymentRepositoryTests
    {
        [Fact]
        public void SaveWithouthAccount_NoAccount_InvalidDataException()
        {
            var accountRepoSetup = new Mock<IDataAccess<Account>>();
            accountRepoSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            var repository = new PaymentRepository(new TransactionDataAccessMock(),
                new RecurringTransactionDataAccessMock());

            var transaction = new Payment
            {
                Amount = 20
            };

            Assert.Throws<AccountMissingException>(() => repository.Save(transaction));
        }

        [Theory]
        [InlineData(PaymentType.Income)]
        public void Save_DifferentTransactionTypes_CorrectlySaved(PaymentType type)
        {
            var accountRepoSetup = new Mock<IDataAccess<Account>>();
            accountRepoSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            var transactionDataAccessMock = new TransactionDataAccessMock();
            var repository = new PaymentRepository(transactionDataAccessMock,
                new RecurringTransactionDataAccessMock());

            var account = new Account
            {
                Name = "TestAccount"
            };

            var transaction = new Payment
            {
                ChargedAccount = account,
                TargetAccount = null,
                Amount = 20,
                Type = (int) type
            };

            repository.Save(transaction);

            transactionDataAccessMock.FinancialTransactionTestList[0].ShouldBeSameAs(transaction);
            transactionDataAccessMock.FinancialTransactionTestList[0].ChargedAccount.ShouldBeSameAs(account);
            transactionDataAccessMock.FinancialTransactionTestList[0].TargetAccount.ShouldBeNull();
            transactionDataAccessMock.FinancialTransactionTestList[0].Type.ShouldBe((int) type);
        }

        [Fact]
        public void Save_TransferTransaction_CorrectlySaved()
        {
            var accountRepoSetup = new Mock<IDataAccess<Account>>();
            accountRepoSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            var transactionDataAccessMock = new TransactionDataAccessMock();
            var repository = new PaymentRepository(transactionDataAccessMock,
                new RecurringTransactionDataAccessMock());

            var account = new Account
            {
                Name = "TestAccount"
            };

            var targetAccount = new Account
            {
                Name = "targetAccount"
            };

            var transaction = new Payment
            {
                ChargedAccount = account,
                TargetAccount = targetAccount,
                Amount = 20,
                Type = (int) PaymentType.Transfer
            };

            repository.Save(transaction);

            transactionDataAccessMock.FinancialTransactionTestList[0].ShouldBeSameAs(transaction);
            transactionDataAccessMock.FinancialTransactionTestList[0].ChargedAccount.ShouldBeSameAs(account);
            transactionDataAccessMock.FinancialTransactionTestList[0].TargetAccount.ShouldBeSameAs(targetAccount);
            transactionDataAccessMock.FinancialTransactionTestList[0].Type.ShouldBe((int) PaymentType.Transfer);
        }

        [Fact]
        public void TransactionRepository_Delete()
        {
            var accountRepoSetup = new Mock<IDataAccess<Account>>();
            accountRepoSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            var transactionDataAccessMock = new TransactionDataAccessMock();
            var repository = new PaymentRepository(transactionDataAccessMock,
                new RecurringTransactionDataAccessMock());

            var account = new Account
            {
                Name = "TestAccount"
            };

            var transaction = new Payment
            {
                ChargedAccount = account,
                Amount = 20
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
            var accountRepoSetup = new Mock<IDataAccess<Account>>();
            accountRepoSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            new PaymentRepository(new TransactionDataAccessMock(), new RecurringTransactionDataAccessMock())
                .Data
                .ShouldNotBeNull();
        }

        [Fact]
        public void TransactionRepository_AddMultipleToCache()
        {
            var accountRepoSetup = new Mock<IDataAccess<Account>>();
            accountRepoSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            var repository = new PaymentRepository(new TransactionDataAccessMock(),
                new RecurringTransactionDataAccessMock());

            var account = new Account
            {
                Name = "TestAccount"
            };

            var transaction = new Payment
            {
                ChargedAccount = account,
                Amount = 20
            };

            var secondTransaction = new Payment
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

        [Fact]
        public void AddItemToDataList_SaveAccount_IsAddedToData()
        {
            var accountRepoSetup = new Mock<IDataAccess<Account>>();
            accountRepoSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            var repository = new PaymentRepository(new TransactionDataAccessMock(),
                new RecurringTransactionDataAccessMock());

            var account = new Account
            {
                Name = "TestAccount"
            };

            var transaction = new Payment
            {
                ChargedAccount = account,
                Amount = 20,
                Type = (int) PaymentType.Transfer
            };

            repository.Save(transaction);
            repository.Data.Contains(transaction).ShouldBeTrue();
        }

        [Fact]
        public void GetUnclearedTransactions_PastDate_PastTransactions()
        {
            var accountRepoSetup = new Mock<IDataAccess<Account>>();
            accountRepoSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            var repository = new PaymentRepository(new TransactionDataAccessMock(),
                new RecurringTransactionDataAccessMock());

            var account = new Account
            {
                Name = "TestAccount"
            };

            repository.Data = new ObservableCollection<Payment>(new List<Payment>
            {
                new Payment
                {
                    ChargedAccount = account,
                    Amount = 55,
                    Date = DateTime.Today.AddDays(-1),
                    Note = "this is a note!!!",
                    IsCleared = false
                }
            });

            var transactions = repository.GetUnclearedPayments();

            transactions.Count().ShouldBe(1);
        }

        /// <summary>
        ///     This Test may fail if the date overlaps with the month transition.
        /// </summary>
        [Fact]
        public void GetUnclearedTransactions_FutureDate_PastTransactions()
        {
            var accountRepoSetup = new Mock<IDataAccess<Account>>();
            accountRepoSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            var repository = new PaymentRepository(new TransactionDataAccessMock(),
                new RecurringTransactionDataAccessMock());

            var account = new Account
            {
                Name = "TestAccount"
            };

            repository.Save(new Payment
            {
                ChargedAccount = account,
                Amount = 55,
                Date = Utilities.GetEndOfMonth().AddDays(-1),
                Note = "this is a note!!!",
                IsCleared = false
            }
                );

            var transactions = repository.GetUnclearedPayments();
            transactions.Count().ShouldBe(0);

            transactions = repository.GetUnclearedPayments(Utilities.GetEndOfMonth());
            transactions.Count().ShouldBe(1);
        }

        [Fact]
        public void GetUnclearedTransactions_AccountNull()
        {
            var accountRepoSetup = new Mock<IDataAccess<Account>>();
            accountRepoSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            var repository = new PaymentRepository(new TransactionDataAccessMock(),
                new RecurringTransactionDataAccessMock());

            repository.Data.Add(new Payment
            {
                Amount = 55,
                Date = DateTime.Today.AddDays(-1),
                Note = "this is a note!!!",
                IsCleared = false
            }
                );

            var transactions = repository.GetUnclearedPayments();
            transactions.Count().ShouldBe(1);
        }

        [Fact]
        public void Load_FinancialTransaction_DataInitialized()
        {
            var accountRepoSetup = new Mock<IDataAccess<Account>>();
            accountRepoSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            var dataAccessSetup = new Mock<IDataAccess<Payment>>();
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Payment>
            {
                new Payment {Id = 10},
                new Payment {Id = 15}
            });

            var categoryRepository = new PaymentRepository(dataAccessSetup.Object,
                new Mock<IDataAccess<RecurringPayment>>().Object);
            categoryRepository.Load();

            categoryRepository.Data.Any(x => x.Id == 10).ShouldBeTrue();
            categoryRepository.Data.Any(x => x.Id == 15).ShouldBeTrue();
        }

        [Fact]
        public void GetRelatedTransactions_Account_CorrectAccounts()
        {
            var accountRepoSetup = new Mock<IDataAccess<Account>>();
            accountRepoSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            var dataAccessSetup = new Mock<IDataAccess<Payment>>();
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Payment>());

            var repo = new PaymentRepository(dataAccessSetup.Object,
                new Mock<IDataAccess<RecurringPayment>>().Object);

            var account1 = new Account {Id = 1};
            var account3 = new Account {Id = 3};

            repo.Data = new ObservableCollection<Payment>(new List<Payment>
            {
                new Payment {Id = 2, ChargedAccount = account1, ChargedAccountId = account1.Id},
                new Payment {Id = 5, ChargedAccount = account3, ChargedAccountId = account3.Id}
            });

            var result = repo.GetRelatedPayments(account1);

            result.Count().ShouldBe(1);
            result.First().Id.ShouldBe(2);
        }

        [Fact]
        public void LoadRecurringList_NoParameters_ListWithRecurringTrans()
        {
            var accountRepoSetup = new Mock<IDataAccess<Account>>();
            accountRepoSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            var dataAccessSetup = new Mock<IDataAccess<Payment>>();
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Payment>());

            var repo = new PaymentRepository(dataAccessSetup.Object,
                new Mock<IDataAccess<RecurringPayment>>().Object)
            {
                Data = new ObservableCollection<Payment>(new List<Payment>
                {
                    new Payment
                    {
                        Id = 1,
                        IsRecurring = true,
                        RecurringPayment = new RecurringPayment {Id = 1, IsEndless = true},
                        ReccuringTransactionId = 1
                    },
                    new Payment {Id = 2, IsRecurring = false},
                    new Payment
                    {
                        Id = 3,
                        IsRecurring = true,
                        RecurringPayment =
                            new RecurringPayment {Id = 2, IsEndless = false, EndDate = DateTime.Today.AddDays(10)},
                        ReccuringTransactionId = 2
                    },
                    new Payment
                    {
                        Id = 4,
                        IsRecurring = true,
                        RecurringPayment =
                            new RecurringPayment {Id = 3, IsEndless = false, EndDate = DateTime.Today.AddDays(-10)},
                        ReccuringTransactionId = 3
                    }
                })
            };

            var result = repo.LoadRecurringList().ToList();

            result.Count.ShouldBe(2);
            result[0].Id.ShouldBe(1);
            result[1].Id.ShouldBe(3);
        }

        [Theory]
        [InlineData(PaymentType.Spending, true)]
        [InlineData(PaymentType.Spending, false)]
        [InlineData(PaymentType.Income, true)]
        [InlineData(PaymentType.Income, false)]
        public void DeleteTransaction_WithoutSpending_DeletedAccountBalanceSet(PaymentType type, bool cleared)
        {
            var deletedId = 0;

            var account = new Account
            {
                Id = 3,
                Name = "just an account",
                CurrentBalance = 500
            };

            var transaction = new Payment
            {
                Id = 10,
                ChargedAccountId = account.Id,
                ChargedAccount = account,
                Amount = 50,
                Type = (int) type,
                IsCleared = cleared
            };

            var accountDataAccessSetup = new Mock<IDataAccess<Account>>();
            accountDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Account> {account});

            var transDataAccessSetup = new Mock<IDataAccess<Payment>>();
            transDataAccessSetup.Setup(x => x.DeleteItem(It.IsAny<Payment>()))
                .Callback((Payment trans) => deletedId = trans.Id);
            transDataAccessSetup.Setup(x => x.SaveItem(It.IsAny<Payment>()));
            transDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Payment> {transaction});

            var recTransDataAccessSetup = new Mock<IDataAccess<RecurringPayment>>();
            recTransDataAccessSetup.Setup(x => x.DeleteItem(It.IsAny<RecurringPayment>()));
            recTransDataAccessSetup.Setup(x => x.LoadList(It.IsAny<Expression<Func<RecurringPayment, bool>>>()))
                .Returns(new List<RecurringPayment>());

            new PaymentRepository(transDataAccessSetup.Object, recTransDataAccessSetup.Object).Delete(
                transaction);

            deletedId.ShouldBe(10);
            account.CurrentBalance.ShouldBe(500);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void DeleteTransaction_Transfer_Deleted(bool isCleared)
        {
            var deletedId = 0;

            var account1 = new Account
            {
                Id = 3,
                Name = "just an account",
                CurrentBalance = 500
            };
            var account2 = new Account
            {
                Id = 4,
                Name = "just an account",
                CurrentBalance = 900
            };

            var transaction = new Payment
            {
                Id = 10,
                ChargedAccountId = account1.Id,
                ChargedAccount = account1,
                TargetAccountId = account2.Id,
                TargetAccount = account2,
                Amount = 50,
                Type = (int) PaymentType.Transfer,
                IsCleared = isCleared
            };

            var accountDataAccessSetup = new Mock<IDataAccess<Account>>();
            accountDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Account> {account1, account2});

            var transDataAccessSetup = new Mock<IDataAccess<Payment>>();
            transDataAccessSetup.Setup(x => x.DeleteItem(It.IsAny<Payment>()))
                .Callback((Payment trans) => deletedId = trans.Id);
            transDataAccessSetup.Setup(x => x.SaveItem(It.IsAny<Payment>()));
            transDataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Payment> {transaction});

            var recTransDataAccessSetup = new Mock<IDataAccess<RecurringPayment>>();
            recTransDataAccessSetup.Setup(x => x.DeleteItem(It.IsAny<RecurringPayment>()));
            recTransDataAccessSetup.Setup(x => x.LoadList(It.IsAny<Expression<Func<RecurringPayment, bool>>>()))
                .Returns(new List<RecurringPayment>());

            new PaymentRepository(transDataAccessSetup.Object, recTransDataAccessSetup.Object).Delete(
                transaction);

            deletedId.ShouldBe(10);
            account1.CurrentBalance.ShouldBe(500);
            account2.CurrentBalance.ShouldBe(900);
        }
    }
}