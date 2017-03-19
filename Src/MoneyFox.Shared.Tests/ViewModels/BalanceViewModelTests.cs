using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Tests;
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
            endOfMonthManagerSetup.Setup(x => x.GetEndOfMonthBalanceForAccount(It.IsAny<AccountViewModel>())).Returns(() => 0);
            endOfMonthManagerSetup.Setup(x => x.GetTotalEndOfMonthBalance(It.IsAny<IEnumerable<AccountViewModel>>())).Returns(() => 0);

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
            accountMockSetup.Setup(x => x.GetList(It.IsAny<Expression<Func<AccountViewModel, bool>>>()))
                .Returns(() => new List<AccountViewModel>
            {
                new AccountViewModel {CurrentBalance = 500, IsExcluded = false},
                new AccountViewModel {CurrentBalance = 200, IsExcluded = false}
            });

            var vm = new BalanceViewModel(accountMockSetup.Object, new Mock<IEndOfMonthManager>().Object);
            vm.UpdateBalanceCommand.Execute();

            vm.TotalBalance.ShouldBe(700);
        }
    }
}