using MediatR;
using MoneyFox.Application.Accounts.Queries.GetExcludedAccount;
using MoneyFox.Application.Accounts.Queries.GetIncludedAccountBalanceSummary;
using MoneyFox.Application.Payments.Queries.GetUnclearedPaymentsOfThisMonth;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Domain.Exceptions;
using MoneyFox.Ui.Shared.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MoneyFox.Uwp.Src
{
    /// <summary>
    /// Provides different calculations for the balance at the end of month.
    /// </summary>
    public interface IBalanceCalculationService
    {
        /// <summary>
        /// Returns the sum of all account balances that are not excluded.
        /// </summary>
        Task<decimal> GetTotalBalanceAsync();

        /// <summary>
        /// Returns the sum of the balance of the passed accounts at the ned of month.
        /// </summary>
        /// <returns>Sum of the end of month balance.</returns>
        Task<decimal> GetTotalEndOfMonthBalanceAsync();

        /// <summary>
        /// Returns the the balance of the passed accounts at the ned of month.
        /// </summary>
        /// <param name="account">Account to calculate the balance.</param>
        /// <returns>The end of month balance.</returns>
        Task<decimal> GetEndOfMonthBalanceForAccountAsync(AccountViewModel account);
    }

    /// <inheritdoc/>
    public class BalanceCalculationService : IBalanceCalculationService
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Constructor
        /// </summary>
        public BalanceCalculationService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <inheritdoc/>
        public async Task<decimal> GetTotalBalanceAsync() => await mediator.Send(new GetIncludedAccountBalanceSummaryQuery());

        /// <inheritdoc/>
        public async Task<decimal> GetTotalEndOfMonthBalanceAsync()
        {
            List<Account> excluded = await mediator.Send(new GetExcludedAccountQuery());

            decimal balance = await GetTotalBalanceAsync();

            foreach(Payment payment in await mediator.Send(new GetUnclearedPaymentsOfThisMonthQuery()))
            {
                switch(payment.Type)
                {
                    case PaymentType.Expense:
                        balance -= payment.Amount;
                        break;

                    case PaymentType.Income:
                        balance += payment.Amount;
                        break;

                    case PaymentType.Transfer:
                        balance = HandleTransfer(excluded, balance, payment);
                        break;

                    default:
                        throw new InvalidPaymentTypeException();
                }
            }

            return balance;
        }

        private static decimal HandleTransfer(List<Account> excluded, decimal balance, Payment payment)
        {
            foreach(Account account in excluded)
            {
                if(payment.TargetAccount == null)
                {
                    throw new InvalidOperationException("Uninitialized property: " + nameof(payment.TargetAccount));
                }

                if(Equals(account.Id, payment.ChargedAccount.Id))
                {
                    //Transfer from excluded account
                    balance += payment.Amount;
                }

                if(Equals(account.Id, payment.TargetAccount.Id))
                {
                    //Transfer to excluded account
                    balance -= payment.Amount;
                }
            }

            return balance;
        }

        /// <inheritdoc/>
        public async Task<decimal> GetEndOfMonthBalanceForAccountAsync(AccountViewModel account)
        {
            decimal balance = account.CurrentBalance;

            List<Payment> paymentList = await mediator.Send(new GetUnclearedPaymentsOfThisMonthQuery { AccountId = account.Id });

            foreach(Payment payment in paymentList)

            {
                switch(payment.Type)
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
            if(accountId == payment.ChargedAccount.Id)
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
