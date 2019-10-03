using System.Threading.Tasks;
using GenericServices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Accounts.Queries;
using MoneyFox.Domain;
using MoneyFox.Domain.Exceptions;
using MoneyFox.Presentation.QueryObject;
using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Presentation.Services
{
    /// <summary>
    ///     Provides different calculations for the balance at the end of month.
    /// </summary>
    public interface IBalanceCalculationService
    {
        /// <summary>
        ///     Returns the sum of all account balances that are not excluded.
        /// </summary>
        Task<double> GetTotalBalance();

        /// <summary>
        ///     Returns the sum of the balance of the passed accounts at the ned of month.
        /// </summary>
        /// <returns>Sum of the end of month balance.</returns>
        Task<double> GetTotalEndOfMonthBalance();

        /// <summary>
        ///     Returns the the balance of the passed accounts at the ned of month.
        /// </summary>
        /// <param name="account">Account to calculate the balance.</param>
        /// <returns>The end of month balance.</returns>
        Task<double> GetEndOfMonthBalanceForAccount(AccountViewModel account);
    }

    /// <inheritdoc />
    public class BalanceCalculationService : IBalanceCalculationService
    {
        private readonly IMediator mediator;

        /// <summary>
        ///     Constructor
        /// </summary>
        public BalanceCalculationService(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        /// <inheritdoc />
        public async Task<double> GetTotalBalance() {
            return await mediator.Send(new GetIncludedAccountBalanceSummary());
        }

        /// <inheritdoc />
        public async Task<double> GetTotalEndOfMonthBalance()
        {
            var excluded = await mediator.Send(new GetExcludedAccountQuery());

            var balance = await GetTotalBalance();

            foreach (var payment in crudServices
                .ReadManyNoTracked<PaymentViewModel>()
                .AreNotCleared()
                .HasDateSmallerEqualsThan(HelperFunctions.GetEndOfMonth()))

                switch (payment.Type)
                {
                    case PaymentType.Expense:
                        balance -= payment.Amount;
                        break;

                    case PaymentType.Income:
                        balance += payment.Amount;
                        break;

                    case PaymentType.Transfer:
                        foreach (var account in excluded)
                        {
                            if (Equals(account.Id, payment.ChargedAccount.Id))
                            {
                                //Transfer from excluded account
                                balance += payment.Amount;
                                break;
                            }

                            if (Equals(account.Id, payment.TargetAccount.Id))
                            {
                                //Transfer to excluded account
                                balance -= payment.Amount;
                                break;
                            }
                        }
                        break;

                    default:
                        throw new InvalidPaymentTypeException();
                }
            return balance;
        }

        /// <inheritdoc />
        public async Task<double> GetEndOfMonthBalanceForAccount(AccountViewModel account)
        {
            var balance = account.CurrentBalance;

            var paymentList = await crudServices.ReadManyNoTracked<PaymentViewModel>()
                .AreNotCleared()
                .HasAccountId(account.Id)
                .HasDateSmallerEqualsThan(HelperFunctions.GetEndOfMonth())
                .ToListAsync();

            foreach (var payment in paymentList)

                switch (payment.Type)
                {
                    case PaymentType.Expense:
                        balance -= payment.Amount;
                        break;

                    case PaymentType.Income:
                        balance += payment.Amount;
                        break;

                    case PaymentType.Transfer:
                        balance = HandleTransferAmount(payment, balance, account.Id);
                        break;
                    default:
                        throw new InvalidPaymentTypeException();
                }
            return balance;
        }

        private double HandleTransferAmount(PaymentViewModel payment, double balance, int accountId)
        {
            if (accountId == payment.ChargedAccountId)
                balance -= payment.Amount;
            else
                balance += payment.Amount;
            return balance;
        }
    }
}
