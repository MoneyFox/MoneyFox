using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetExcludedAccount;
using MoneyFox.Application.Accounts.Queries.GetIncludedAccountBalanceSummary;
using MoneyFox.Application.Payments.Queries.GetUnclearedPaymentsOfThisMonth;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Domain.Exceptions;
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
        Task<decimal> GetTotalBalance();

        /// <summary>
        ///     Returns the sum of the balance of the passed accounts at the ned of month.
        /// </summary>
        /// <returns>Sum of the end of month balance.</returns>
        Task<decimal> GetTotalEndOfMonthBalance();

        /// <summary>
        ///     Returns the the balance of the passed accounts at the ned of month.
        /// </summary>
        /// <param name="account">Account to calculate the balance.</param>
        /// <returns>The end of month balance.</returns>
        Task<decimal> GetEndOfMonthBalanceForAccount(AccountViewModel account);
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
        public async Task<decimal> GetTotalBalance()
        {
            return await mediator.Send(new GetIncludedAccountBalanceSummaryQuery());
        }

        /// <inheritdoc />
        public async Task<decimal> GetTotalEndOfMonthBalance()
        {
            List<Account> excluded = await mediator.Send(new GetExcludedAccountQuery());

            decimal balance = await GetTotalBalance();

            foreach (Payment payment in await mediator.Send(new GetUnclearedPaymentsOfThisMonthQuery()))
            {
                switch (payment.Type)
                {
                    case PaymentType.Expense:
                        balance -= payment.Amount;
                        break;

                    case PaymentType.Income:
                        balance += payment.Amount;
                        break;

                    case PaymentType.Transfer:
                        foreach (Account account in excluded)
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
            }

            return balance;
        }

        /// <inheritdoc />
        public async Task<decimal> GetEndOfMonthBalanceForAccount(AccountViewModel account)
        {
            decimal balance = account.CurrentBalance;

            List<Payment> paymentList = await mediator.Send(new GetUnclearedPaymentsOfThisMonthQuery {AccountId = account.Id});

            foreach (Payment payment in paymentList)

            {
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
            }

            return balance;
        }

        private static decimal HandleTransferAmount(Payment payment, decimal balance, int accountId)
        {
            if (accountId == payment.ChargedAccount.Id)
            {
                balance -= payment.Amount;
            }
            else
            {
                balance += payment.Amount;
            }

            return balance;
        }
    }
}
