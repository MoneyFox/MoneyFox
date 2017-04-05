using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoneyFox.Business.Manager;
using MoneyFox.Foundation;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Tests;
using Moq;
using Xunit;

namespace MoneyFox.Business.Tests.Manager
{
    public class EndMonthManagerTests
    {
        [Fact]
        public void EndofMonthManager_AccountIsNegative()
        {
            AccountViewModel account1 = new AccountViewModel
            {
                Id = 1,
                CurrentBalance = 100
            };
            
            var accounts = new List<AccountViewModel>
            {
                new AccountViewModel {Id=2, CurrentBalance=100}, 
                account1
            };

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.Setup(x => x.GetList(It.IsAny<Expression<Func<PaymentViewModel, bool>>>())).Returns(new List<PaymentViewModel>
            {
                new PaymentViewModel {Id = 10, ChargedAccountId=1, Amount=100,Date= DateTime.Now},
                new PaymentViewModel {Id = 15, ChargedAccountId=1, Amount=100, Date= DateTime.Now}
            });

            var testManager = new EndOfMonthManager(paymentRepoSetup.Object);

            testManager.CheckEndOfMonthBalanceForAccounts(accounts);
            account1.IsOverdrawn.ShouldBeTrue();
        }

        [Fact]
        public void EndofMonthManager_AccountIsPositive()
        {
            AccountViewModel account1 = new AccountViewModel
            {
                Id = 1,
                CurrentBalance = -100,
            };

            var paymentRepoSetup = new Mock<IPaymentRepository>();
            paymentRepoSetup.Setup(x => x.GetList(It.IsAny<Expression<Func<PaymentViewModel, bool>>>())).Returns(new List<PaymentViewModel>
            {
                new PaymentViewModel {Id = 10, TargetAccountId=1, Amount=100,Date= DateTime.Now, Type = PaymentType.Income},
                new PaymentViewModel {Id = 15, TargetAccountId=1, Amount=100, Date= DateTime.Now, Type = PaymentType.Income}
            });

            var accounts = new List<AccountViewModel>
            {
                new AccountViewModel {Id=2, CurrentBalance=100},
                account1
            };

            EndOfMonthManager testManager = new EndOfMonthManager(paymentRepoSetup.Object);
            testManager.CheckEndOfMonthBalanceForAccounts(accounts);

            account1.IsOverdrawn.ShouldBeFalse();
        }

        [Fact]
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
