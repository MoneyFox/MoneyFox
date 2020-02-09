using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Uwp;
using MoneyFox.Uwp.ViewModels;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class BalanceViewModelTests
    {
        [Fact]
        public async Task GetTotalBalance_Zero()
        {
            var balanceCalculationService = new Mock<IBalanceCalculationService>();
            balanceCalculationService.Setup(x => x.GetEndOfMonthBalanceForAccount(It.IsAny<AccountViewModel>())).ReturnsAsync(() => 0);
            balanceCalculationService.Setup(x => x.GetTotalEndOfMonthBalance()).ReturnsAsync(() => 0);

            var vm = new BalanceViewModel(balanceCalculationService.Object);

            await vm.UpdateBalanceCommand.ExecuteAsync();

            vm.TotalBalance.ShouldEqual(0);
            vm.EndOfMonthBalance.ShouldEqual(0);
        }

        [Fact]
        public async Task GetTotalBalance_TwoPayments_SumOfPayments()
        {
            var vm = new BalanceViewModel(new Mock<IBalanceCalculationService>().Object);
            await vm.UpdateBalanceCommand.ExecuteAsync();

            vm.TotalBalance.ShouldEqual(0);
        }

        [Fact]
        public async Task GetTotalBalance_TwoAccounts_SumOfAccounts()
        {
            var balanceCalculationService = new Mock<IBalanceCalculationService>();
            balanceCalculationService.Setup(x => x.GetTotalBalance())
                                     .ReturnsAsync(() => 700);

            var vm = new BalanceViewModel(balanceCalculationService.Object);
            await vm.UpdateBalanceCommand.ExecuteAsync();

            vm.TotalBalance.ShouldEqual(700);
        }
    }
}
