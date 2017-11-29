using MoneyFox.Business.Manager;
using MoneyFox.Business.ViewModels;
using MoneyFox.DataAccess.Pocos;
using Moq;
using MvvmCross.Test.Core;
using Xunit;

namespace MoneyFox.Business.Tests.ViewModels
{
    public class BalanceViewModelTests : MvxIoCSupportingTest
    {
        [Fact]
        public void GetTotalBalance_Zero()
        {
            var balanceCalculationManager = new Mock<IBalanceCalculationManager>();
            balanceCalculationManager.Setup(x => x.GetEndOfMonthBalanceForAccount(It.IsAny<Account>())).ReturnsAsync(() => 0);
            balanceCalculationManager.Setup(x => x.GetTotalEndOfMonthBalance()).ReturnsAsync(() => 0);

            var vm = new BalanceViewModel(balanceCalculationManager.Object);

            vm.UpdateBalanceCommand.Execute();

            vm.TotalBalance.ShouldBe(0);
            vm.EndOfMonthBalance.ShouldBe(0);
        }

        [Fact]
        public void GetTotalBalance_TwoPayments_SumOfPayments()
        {
            var vm = new BalanceViewModel(new Mock<IBalanceCalculationManager>().Object);
            vm.UpdateBalanceCommand.Execute();

            vm.TotalBalance.ShouldBe(0);
        }

        [Fact]
        public void GetTotalBalance_TwoAccounts_SumOfAccounts()
        {
            var balanceCalculationManager = new Mock<IBalanceCalculationManager>();
            balanceCalculationManager.Setup(x => x.GetTotalBalance())
                .ReturnsAsync(() => 700);

            var vm = new BalanceViewModel(balanceCalculationManager.Object);
            vm.UpdateBalanceCommand.Execute();

            vm.TotalBalance.ShouldBe(700);
        }
    }
}