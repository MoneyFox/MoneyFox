using AutoMapper;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccountById;
using MoneyFox.Uwp.ViewModels.Accounts;
using System.Threading.Tasks;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Payments
{
    /// <summary>
    ///     This ViewModel is for the usage in the payment list when a concrete account is selected
    /// </summary>
    public class PaymentListBalanceViewModel : BalanceViewModel
    {
        private readonly int accountId;
        private readonly IBalanceCalculationService balanceCalculationService;
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        /// <summary>
        ///     Constructor
        /// </summary>
        public PaymentListBalanceViewModel(IMediator mediator,
            IMapper mapper,
            IBalanceCalculationService balanceCalculationService,
            int accountId) : base(balanceCalculationService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.balanceCalculationService = balanceCalculationService;
            this.accountId = accountId;
        }

        /// <summary>
        ///     Calculates the sum of all accounts at the current moment.
        /// </summary>
        /// <returns>Sum of the balance of all accounts.</returns>
        protected override async Task<decimal> CalculateTotalBalanceAsync()
        {
            var account = mapper.Map<AccountViewModel>(await mediator.Send(new GetAccountByIdQuery(accountId)));

            return account.CurrentBalance;
        }

        /// <summary>
        ///     Calculates the sum of the selected account at the end of the month.     This includes all payments coming
        ///     until the end of month.
        /// </summary>
        /// <returns>Balance of the selected account including all payments to come till end of month.</returns>
        protected override async Task<decimal> GetEndOfMonthValueAsync()
        {
            var account = mapper.Map<AccountViewModel>(await mediator.Send(new GetAccountByIdQuery(accountId)));

            return await balanceCalculationService.GetEndOfMonthBalanceForAccountAsync(account);
        }
    }
}