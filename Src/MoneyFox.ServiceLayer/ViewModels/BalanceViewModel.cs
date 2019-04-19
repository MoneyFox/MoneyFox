using System.Threading.Tasks;
using MoneyFox.ServiceLayer.Services;
using MvvmCross.Commands;
using ReactiveUI;

namespace MoneyFox.ServiceLayer.ViewModels
{
    /// <summary>
    ///     Representation of the BalanceView
    /// </summary>
    public class BalanceViewModel : ViewModelBase
    {
        private readonly IBalanceCalculationService balanceCalculationService;

        private double totalBalance;
        private double endOfMonthBalance;

        public BalanceViewModel(IScreen hostScreen, IBalanceCalculationService balanceCalculationService)
        {
            HostScreen = hostScreen;
            this.balanceCalculationService = balanceCalculationService;
        }

        public override string UrlPathSegment => "Balance";
        public override IScreen HostScreen { get; }

        /// <summary>
        ///     Balance of all relevant accounts at the end of the month.
        /// </summary>
        public double TotalBalance
        {
            get => totalBalance;
            set => this.RaiseAndSetIfChanged(ref totalBalance, value);
        }

        /// <summary>
        ///     Current Balance of all accounts.
        /// </summary>
        public double EndOfMonthBalance
        {
            get => endOfMonthBalance;
            set => this.RaiseAndSetIfChanged(ref endOfMonthBalance, value);
        }

        /// <summary>
        ///     Refreshes the balances. Depending on if it is displayed in a payment view or a general view it will adjust
        ///     itself and show different data.
        /// </summary>
        public MvxAsyncCommand UpdateBalanceCommand => new MvxAsyncCommand(UpdateBalance);

        /// <summary>
        ///     Refreshes the balances. Depending on if it is displayed in a payment view or a general view it will adjust
        ///     itself and show different data.
        /// </summary>
        private async Task UpdateBalance()
        {
            TotalBalance = await CalculateTotalBalance();
            EndOfMonthBalance = await GetEndOfMonthValue();
        }

        /// <summary>
        ///     Calculates the sum of all accounts at the current moment.
        /// </summary>
        /// <returns>Sum of the balance of all accounts.</returns>
        protected virtual async Task<double> CalculateTotalBalance() => await balanceCalculationService.GetTotalBalance();

        /// <summary>
        ///     Calculates the sum of all accounts at the end of the month.
        /// </summary>
        /// <returns>Sum of all balances including all payments to come till end of month.</returns>
        protected virtual async Task<double> GetEndOfMonthValue() =>  await balanceCalculationService.GetTotalEndOfMonthBalance();
    }
}