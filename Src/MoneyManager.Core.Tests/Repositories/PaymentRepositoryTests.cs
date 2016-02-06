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
using Moq;
using Xunit;
using XunitShouldExtension;

namespace MoneyManager.Core.Tests.Repositories
{
    public class PaymentRepositoryTests
    {
        [Fact]
        public void SaveWithouthAccount_NoAccount_InvalidDataException()
        {
            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            var categoryDataAccessSetup = new Mock<IRepository<Category>>();
            categoryDataAccessSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Category>());

            var repository = new PaymentRepository(new PaymentDataAccessMock(),
                new RecurringPaymentDataAccessMock(),
                accountRepositorySetup.Object,
                categoryDataAccessSetup.Object);

            var payment = new Payment
            {
                Amount = 20
            };

            Assert.Throws<AccountMissingException>(() => repository.Save(payment));
        }

        [Theory]
        [InlineData(PaymentType.Income)]
        public void Save_DifferentPaymentTypes_CorrectlySaved(PaymentType type)
        {
            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            var categoryDataAccessSetup = new Mock<IRepository<Category>>();
            categoryDataAccessSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Category>());

            var paymentDataAccessMock = new PaymentDataAccessMock();
            var repository = new PaymentRepository(paymentDataAccessMock,
                new RecurringPaymentDataAccessMock(),
                accountRepositorySetup.Object,
                categoryDataAccessSetup.Object);

            var account = new Account
            {
                Name = "TestAccount"
            };

            var payment = new Payment
            {
                ChargedAccount = account,
                TargetAccount = null,
                Amount = 20,
                Type = (int) type
            };

            repository.Save(payment);

            paymentDataAccessMock.PaymentTestList[0].ShouldBeSameAs(payment);
            paymentDataAccessMock.PaymentTestList[0].ChargedAccount.ShouldBeSameAs(account);
            paymentDataAccessMock.PaymentTestList[0].TargetAccount.ShouldBeNull();
            paymentDataAccessMock.PaymentTestList[0].Type.ShouldBe((int) type);
        }

        [Fact]
        public void Save_TransferPayment_CorrectlySaved()
        {
            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            var categoryDataAccessSetup = new Mock<IRepository<Category>>();
            categoryDataAccessSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Category>());

            var paymentDataAccessMock = new PaymentDataAccessMock();
            var repository = new PaymentRepository(new PaymentDataAccessMock(),
                new RecurringPaymentDataAccessMock(),
                accountRepositorySetup.Object,
                categoryDataAccessSetup.Object);

            var account = new Account
            {
                Name = "TestAccount"
            };

            var targetAccount = new Account
            {
                Name = "targetAccount"
            };

            var payment = new Payment
            {
                ChargedAccount = account,
                TargetAccount = targetAccount,
                Amount = 20,
                Type = (int) PaymentType.Transfer
            };

            repository.Save(payment);

            paymentDataAccessMock.PaymentTestList[0].ShouldBeSameAs(payment);
            paymentDataAccessMock.PaymentTestList[0].ChargedAccount.ShouldBeSameAs(account);
            paymentDataAccessMock.PaymentTestList[0].TargetAccount.ShouldBeSameAs(targetAccount);
            paymentDataAccessMock.PaymentTestList[0].Type.ShouldBe((int) PaymentType.Transfer);
        }

        [Fact]
        public void PaymentRepository_Delete()
        {
            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            var categoryDataAccessSetup = new Mock<IRepository<Category>>();
            categoryDataAccessSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Category>());

            var paymentDataAccessMock = new PaymentDataAccessMock();
            var repository = new PaymentRepository(new PaymentDataAccessMock(),
                new RecurringPaymentDataAccessMock(),
                accountRepositorySetup.Object,
                categoryDataAccessSetup.Object);

            var account = new Account
            {
                Name = "TestAccount"
            };

            var payment = new Payment
            {
                ChargedAccount = account,
                Amount = 20
            };

            repository.Save(payment);
            paymentDataAccessMock.PaymentTestList[0].ShouldBeSameAs(payment);

            repository.Delete(payment);

            paymentDataAccessMock.PaymentTestList.Any().ShouldBeFalse();
            repository.Data.Any().ShouldBeFalse();
        }

        [Fact]
        public void PaymentRepository_AccessCache()
        {
            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            var categoryRepositorySetup = new Mock<IRepository<Category>>();
            categoryRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Category>());

            new PaymentRepository(new PaymentDataAccessMock(),
                new RecurringPaymentDataAccessMock(), 
                accountRepositorySetup.Object, 
                categoryRepositorySetup.Object)
                .Data
                .ShouldNotBeNull();
        }

        [Fact]
        public void PaymentRepository_AddMultipleToCache()
        {
            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            var categoryDataAccessSetup = new Mock<IRepository<Category>>();
            categoryDataAccessSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Category>());

            var repository = new PaymentRepository(new PaymentDataAccessMock(),
                new RecurringPaymentDataAccessMock(),
                accountRepositorySetup.Object,
                categoryDataAccessSetup.Object);

            var account = new Account
            {
                Name = "TestAccount"
            };

            var payment = new Payment
            {
                ChargedAccount = account,
                Amount = 20
            };

            var secondPayment = new Payment
            {
                ChargedAccount = account,
                Amount = 60
            };

            repository.Save(payment);
            repository.Save(secondPayment);

            repository.Data.Count.ShouldBe(2);
            repository.Data[0].ShouldBeSameAs(payment);
            repository.Data[1].ShouldBeSameAs(secondPayment);
        }

        [Fact]
        public void AddItemToDataList_SaveAccount_IsAddedToData()
        {
            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            var categoryDataAccessSetup = new Mock<IRepository<Category>>();
            categoryDataAccessSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Category>());

            var repository = new PaymentRepository(new PaymentDataAccessMock(),
                new RecurringPaymentDataAccessMock(),
                accountRepositorySetup.Object,
                categoryDataAccessSetup.Object);

            var account = new Account
            {
                Name = "TestAccount"
            };

            var payment = new Payment
            {
                ChargedAccount = account,
                Amount = 20,
                Type = (int) PaymentType.Transfer
            };

            repository.Save(payment);
            repository.Data.Contains(payment).ShouldBeTrue();
        }

        [Fact]
        public void GetUnclearedPayments_PastDate_PastPayments()
        {
            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            var categoryDataAccessSetup = new Mock<IRepository<Category>>();
            categoryDataAccessSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Category>());

            var repository = new PaymentRepository(new PaymentDataAccessMock(),
                new RecurringPaymentDataAccessMock(),
                accountRepositorySetup.Object,
                categoryDataAccessSetup.Object);

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

            var payments = repository.GetUnclearedPayments();

            payments.Count().ShouldBe(1);
        }

        /// <summary>
        ///     This Test may fail if the date overlaps with the month transition.
        /// </summary>
        [Fact]
        public void GetUnclearedPayments_FutureDate_PastPayments()
        {
            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            var categoryDataAccessSetup = new Mock<IRepository<Category>>();
            categoryDataAccessSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Category>());

            var repository = new PaymentRepository(new PaymentDataAccessMock(),
                new RecurringPaymentDataAccessMock(),
                accountRepositorySetup.Object,
                categoryDataAccessSetup.Object);
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

            var payments = repository.GetUnclearedPayments();
            payments.Count().ShouldBe(0);

            payments = repository.GetUnclearedPayments(Utilities.GetEndOfMonth());
            payments.Count().ShouldBe(1);
        }

        [Fact]
        public void GetUnclearedPayments_AccountNull()
        {
            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            var categoryDataAccessSetup = new Mock<IRepository<Category>>();
            categoryDataAccessSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Category>());

            var repository = new PaymentRepository(new PaymentDataAccessMock(),
                new RecurringPaymentDataAccessMock(),
                accountRepositorySetup.Object,
                categoryDataAccessSetup.Object);

            repository.Data.Add(new Payment
            {
                Amount = 55,
                Date = DateTime.Today.AddDays(-1),
                Note = "this is a note!!!",
                IsCleared = false
            }
                );

            var payments = repository.GetUnclearedPayments();
            payments.Count().ShouldBe(1);
        }

        [Fact]
        public void Load_Payment_DataInitialized()
        {
            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            var dataAccessSetup = new Mock<IDataAccess<Payment>>();
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Payment>
            {
                new Payment {Id = 10},
                new Payment {Id = 15}
            });

            var categoryDataAccessSetup = new Mock<IRepository<Category>>();
            categoryDataAccessSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Category>());


            var paymentRepository = new PaymentRepository(dataAccessSetup.Object,
                new Mock<IDataAccess<RecurringPayment>>().Object,
                accountRepositorySetup.Object,
                categoryDataAccessSetup.Object);
            paymentRepository.Load();

            paymentRepository.Data.Any(x => x.Id == 10).ShouldBeTrue();
            paymentRepository.Data.Any(x => x.Id == 15).ShouldBeTrue();
        }

        [Fact]
        public void GetRelatedPayments_Account_CorrectAccounts()
        {
            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            var dataAccessSetup = new Mock<IDataAccess<Payment>>();
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Payment>());

            var categoryDataAccessSetup = new Mock<IRepository<Category>>();
            categoryDataAccessSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Category>());

            var repo = new PaymentRepository(dataAccessSetup.Object,
                new Mock<IDataAccess<RecurringPayment>>().Object,
                accountRepositorySetup.Object,
                categoryDataAccessSetup.Object);

            var account1 = new Account {Id = 1};
            var account3 = new Account {Id = 3};

            repo.Data = new ObservableCollection<Payment>(new List<Payment>
            {
                new Payment {Id = 2, ChargedAccount = account1, ChargedAccountId = account1.Id},
                new Payment {Id = 5, ChargedAccount = account3, ChargedAccountId = account3.Id}
            });

            var result = repo.GetRelatedPayments(account1).ToList();

            result.Count.ShouldBe(1);
            result.First().Id.ShouldBe(2);
        }

        [Fact]
        public void LoadRecurringList_NoParameters_ListWithRecurringTrans()
        {
            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            var dataAccessSetup = new Mock<IDataAccess<Payment>>();
            dataAccessSetup.Setup(x => x.LoadList(null)).Returns(new List<Payment>());

            var categoryDataAccessSetup = new Mock<IRepository<Category>>();
            categoryDataAccessSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Category>());

            var repo = new PaymentRepository(dataAccessSetup.Object,
                new Mock<IDataAccess<RecurringPayment>>().Object,
                accountRepositorySetup.Object,
                categoryDataAccessSetup.Object)
            {
                Data = new ObservableCollection<Payment>(new List<Payment>
                {
                    new Payment
                    {
                        Id = 1,
                        IsRecurring = true,
                        RecurringPayment = new RecurringPayment {Id = 1, IsEndless = true},
                        RecurringPaymentId = 1
                    },
                    new Payment {Id = 2, IsRecurring = false},
                    new Payment
                    {
                        Id = 3,
                        IsRecurring = true,
                        RecurringPayment =
                            new RecurringPayment {Id = 2, IsEndless = false, EndDate = DateTime.Today.AddDays(10)},
                        RecurringPaymentId = 2
                    },
                    new Payment
                    {
                        Id = 4,
                        IsRecurring = true,
                        RecurringPayment =
                            new RecurringPayment {Id = 3, IsEndless = false, EndDate = DateTime.Today.AddDays(-10)},
                        RecurringPaymentId = 3
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
        public void DeletePayment_WithoutSpending_DeletedAccountBalanceSet(PaymentType type, bool cleared)
        {
            var deletedId = 0;

            var account = new Account
            {
                Id = 3,
                Name = "just an account",
                CurrentBalance = 500
            };

            var payment = new Payment
            {
                Id = 10,
                ChargedAccountId = account.Id,
                ChargedAccount = account,
                Amount = 50,
                Type = (int) type,
                IsCleared = cleared
            };

            var paymentDataAccessMockSetup = new Mock<IDataAccess<Payment>>();
            paymentDataAccessMockSetup.Setup(x => x.DeleteItem(It.IsAny<Payment>()))
                .Callback((Payment trans) => deletedId = trans.Id);
            paymentDataAccessMockSetup.Setup(x => x.SaveItem(It.IsAny<Payment>()));
            paymentDataAccessMockSetup.Setup(x => x.LoadList(null)).Returns(new List<Payment> {payment});

            var recPaymentDataAccessMockSetup = new Mock<IDataAccess<RecurringPayment>>();
            recPaymentDataAccessMockSetup.Setup(x => x.DeleteItem(It.IsAny<RecurringPayment>()));
            recPaymentDataAccessMockSetup.Setup(x => x.LoadList(It.IsAny<Expression<Func<RecurringPayment, bool>>>()))
                .Returns(new List<RecurringPayment>());

            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account> { account });

            var categoryDataAccessSetup = new Mock<IRepository<Category>>();
            categoryDataAccessSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Category>());

            new PaymentRepository(paymentDataAccessMockSetup.Object, 
                recPaymentDataAccessMockSetup.Object, 
                accountRepositorySetup.Object, 
                categoryDataAccessSetup.Object)
                .Delete(
                payment);

            deletedId.ShouldBe(10);
            account.CurrentBalance.ShouldBe(500);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void DeletePayment_Transfer_Deleted(bool isCleared)
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

            var payment = new Payment
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


            var paymentDataAccessMockSetup = new Mock<IDataAccess<Payment>>();
            paymentDataAccessMockSetup.Setup(x => x.DeleteItem(It.IsAny<Payment>()))
                .Callback((Payment trans) => deletedId = trans.Id);
            paymentDataAccessMockSetup.Setup(x => x.SaveItem(It.IsAny<Payment>()));
            paymentDataAccessMockSetup.Setup(x => x.LoadList(null)).Returns(new List<Payment> {payment});

            var recPaymentDataAccessMockSetup = new Mock<IDataAccess<RecurringPayment>>();
            recPaymentDataAccessMockSetup.Setup(x => x.DeleteItem(It.IsAny<RecurringPayment>()));
            recPaymentDataAccessMockSetup.Setup(x => x.LoadList(It.IsAny<Expression<Func<RecurringPayment, bool>>>()))
                .Returns(new List<RecurringPayment>());

            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account> { account1, account2 });

            var categoryDataAccessSetup = new Mock<IRepository<Category>>();
            categoryDataAccessSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Category>());

            new PaymentRepository(paymentDataAccessMockSetup.Object, 
                recPaymentDataAccessMockSetup.Object,
                accountRepositorySetup.Object,
                categoryDataAccessSetup.Object).Delete(
                payment);

            deletedId.ShouldBe(10);
            account1.CurrentBalance.ShouldBe(500);
            account2.CurrentBalance.ShouldBe(900);
        }
    }
}