using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using MoneyFox.Shared.Manager;
using MoneyFox.Shared.Model;
using Moq;
using MoneyFox.Shared.Repositories;

namespace MoneyFox.Shared.Tests.Manager
{
    [TestClass]
    public class EndMonthManagerTests
    {
        [TestMethod]
        public void EndofMonthManager_AccountIsNegative()
        {
            AccountViewModel account1 = new AccountViewModel
            {
                Id = 1,
                CurrentBalance = 100
            };

            var paymentDataAccess = new Mock<IDataAccess<PaymentViewModel>>();
            paymentDataAccess.Setup(x => x.LoadList(null)).Returns(new List<PaymentViewModel>
            {
                new PaymentViewModel {Id = 10, ChargedAccountId=1, Amount=100,Date= DateTime.Now},
                new PaymentViewModel {Id = 15, ChargedAccountId=1, Amount=100, Date= DateTime.Now}
            });

            var paymentrepository = new PaymentRepository(paymentDataAccess.Object);
            paymentrepository.Load();

            var accounts = new List<AccountViewModel>
            {
                new AccountViewModel {Id=2, CurrentBalance=100}, 
                account1
            };

            EndOfMonthManager testManager = new EndOfMonthManager(paymentrepository);

            testManager.CheckEndOfMonthBalanceForAccounts(accounts);
            account1.IsOverdrawn.ShouldBeTrue();
        }

        [TestMethod]
        public void EndofMonthManager_AccountIsPositive()
        {
            AccountViewModel account1 = new AccountViewModel
            {
                Id = 1,
                CurrentBalance = -100,
            };

            var paymentDataAccess = new Mock<IDataAccess<PaymentViewModel>>();
            paymentDataAccess.Setup(x => x.LoadList(It.IsAny<Expression<Func<PaymentViewModel, bool>>>())).Returns(new List<PaymentViewModel>
            {
                new PaymentViewModel {Id = 10, TargetAccountId=1, Amount=100,Date= DateTime.Now, Type = (int) PaymentType.Income},
                new PaymentViewModel {Id = 15, TargetAccountId=1, Amount=100, Date= DateTime.Now, Type = (int) PaymentType.Income}
            });

            var paymentrepository = new PaymentRepository(paymentDataAccess.Object);
            paymentrepository.Load();

            var accounts = new List<AccountViewModel>
            {
                new AccountViewModel {Id=2, CurrentBalance=100},
                account1
            };

            EndOfMonthManager testManager = new EndOfMonthManager(paymentrepository);
            testManager.CheckEndOfMonthBalanceForAccounts(accounts);

            account1.IsOverdrawn.ShouldBeFalse();
        }

        [TestMethod]
        public void GetTotalEndOfMonthBalance_TwoAccounts_SumOfAccounts()
        {
            var paymentMockSetup = new Mock<IPaymentRepository>();
            paymentMockSetup.Setup(x => x.GetList(null)).Returns(() => new List<PaymentViewModel>());

            var endOfMonthManager = new EndOfMonthManager(paymentMockSetup.Object);

            endOfMonthManager.GetTotalEndOfMonthBalance(new List<AccountViewModel>
            {
                new AccountViewModel {CurrentBalance = 500},
                new AccountViewModel {CurrentBalance = 200}
            }).ShouldBe(700);
        }
    }
}
