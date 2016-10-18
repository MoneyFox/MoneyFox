using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
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
            var endOfMonthManagerSetup = new Mock<IEndOfMonthManager>();
            endOfMonthManagerSetup.Setup(x => x.GetEndOfMonthBalanceForAccount(It.IsAny<Account>())).Returns(() => 0);
            endOfMonthManagerSetup.Setup(x => x.GetTotalEndOfMonthBalance(It.IsAny<IEnumerable<Account>>())).Returns(() => 0);

            var vm = new BalanceViewModel(new Mock<IAccountRepository>().Object, endOfMonthManagerSetup.Object);

            vm.UpdateBalanceCommand.Execute();

            vm.TotalBalance.ShouldBe(0);
            vm.EndOfMonthBalance.ShouldBe(0);
        }

        [TestMethod]
        public void GetTotalBalance_TwoPayments_SumOfPayments()
        {
            var vm = new BalanceViewModel(new Mock<IAccountRepository>().Object, new Mock<IEndOfMonthManager>().Object);
            vm.UpdateBalanceCommand.Execute();

            vm.TotalBalance.ShouldBe(0);
        }

        [TestMethod]
        public void GetTotalBalance_TwoAccounts_SumOfAccounts()
        {
            var accountMockSetup = new Mock<IAccountRepository>();
            accountMockSetup.Setup(x => x.GetList(null)).Returns(() => new List<Account>
            {
                new Account {CurrentBalance = 500},
                new Account {CurrentBalance = 200}
            });

            var vm = new BalanceViewModel(accountMockSetup.Object, new Mock<IEndOfMonthManager>().Object);
            vm.UpdateBalanceCommand.Execute();

            vm.TotalBalance.ShouldBe(700);
        }
    }
}