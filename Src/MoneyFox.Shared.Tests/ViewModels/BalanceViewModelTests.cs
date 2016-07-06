using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.ViewModels;
using Moq;
using MvvmCross.Platform.Core;
using MvvmCross.Test.Core;

namespace MoneyFox.Shared.Tests.ViewModels
{
    [TestClass]
    public class BalanceViewModelTests : MvxIoCSupportingTest
    {
        [TestInitialize]
        public void Init()
        {
            MvxSingleton.ClearAllSingletons();
            Setup();
        }

        [TestMethod]
        public void GetTotalBalance_Zero()
        {
            var paymentMockSetup = new Mock<IPaymentRepository>();
            paymentMockSetup.Setup(x => x.GetUnclearedPayments(It.IsAny<DateTime>())).Returns(() => new List<Payment>());

            var vm = new BalanceViewModel(new Mock<IAccountRepository>().Object,
                paymentMockSetup.Object);

            vm.UpdateBalanceCommand.Execute();

            vm.TotalBalance.ShouldBe(0);
            vm.EndOfMonthBalance.ShouldBe(0);
        }

        [TestMethod]
        public void GetTotalBalance_TwoExpense_SumOfPayments()
        {
            var paymentMockSetup = new Mock<IPaymentRepository>();
            paymentMockSetup.Setup(x => x.GetUnclearedPayments(It.IsAny<DateTime>()))
                .Returns(() => new List<Payment>
                {
                    new Payment {Amount = 20, Type = (int) PaymentType.Expense},
                    new Payment {Amount = 60, Type = (int) PaymentType.Expense}
                });

            var vm = new BalanceViewModel(new Mock<IAccountRepository>().Object,
                paymentMockSetup.Object);

            vm.UpdateBalanceCommand.Execute();

            vm.TotalBalance.ShouldBe(0);
            vm.EndOfMonthBalance.ShouldBe(-80);
        }

        [TestMethod]
        public void GetTotalBalance_TwoPayments_SumOfPayments()
        {
            var paymentMockSetup = new Mock<IPaymentRepository>();
            paymentMockSetup.Setup(x => x.GetUnclearedPayments(It.IsAny<DateTime>()))
                .Returns(() => new List<Payment>
                {
                    new Payment {Amount = 20, Type = (int) PaymentType.Expense},
                    new Payment {Amount = 60, Type = (int) PaymentType.Income}
                });

            var vm = new BalanceViewModel(new Mock<IAccountRepository>().Object,
                paymentMockSetup.Object);

            vm.UpdateBalanceCommand.Execute();

            vm.TotalBalance.ShouldBe(0);
            vm.EndOfMonthBalance.ShouldBe(40);
        }

        [TestMethod]
        public void GetTotalBalance_TwoAccounts_SumOfAccounts()
        {
            var paymentMockSetup = new Mock<IPaymentRepository>();
            paymentMockSetup.Setup(x => x.GetUnclearedPayments()).Returns(() => new List<Payment>());

            var accountMockSetup = new Mock<IAccountRepository>();
            accountMockSetup.SetupGet(x => x.Data).Returns(() => new ObservableCollection<Account>
            {
                new Account {CurrentBalance = 500},
                new Account {CurrentBalance = 200}
            });

            var vm = new BalanceViewModel(accountMockSetup.Object,
                paymentMockSetup.Object);

            vm.UpdateBalanceCommand.Execute();

            vm.TotalBalance.ShouldBe(700);
            vm.EndOfMonthBalance.ShouldBe(700);
        }
    }
}