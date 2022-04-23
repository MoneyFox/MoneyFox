namespace MoneyFox.Win;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Domain.Exceptions;
using Core.ApplicationCore.Queries;
using MediatR;
using ViewModels.Accounts;

/// <summary>
///     Provides different calculations for the balance at the end of month.
/// </summary>
public interface IBalanceCalculationService
{
    /// <summary>
    ///     Returns the sum of all account balances that are not excluded.
    /// </summary>
    Task<decimal> GetTotalBalanceAsync();

    /// <summary>
    ///     Returns the sum of the balance of the passed accounts at the ned of month.
    /// </summary>
    /// <returns>Sum of the end of month balance.</returns>
    Task<decimal> GetTotalEndOfMonthBalanceAsync();

    /// <summary>
    ///     Returns the the balance of the passed accounts at the ned of month.
    /// </summary>
    /// <param name="account">Account to calculate the balance.</param>
    /// <returns>The end of month balance.</returns>
    Task<decimal> GetEndOfMonthBalanceForAccountAsync(AccountViewModel account);
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
    public async Task<decimal> GetTotalBalanceAsync()
    {
        return await mediator.Send(new GetIncludedAccountBalanceSummaryQuery());
    }

    /// <inheritdoc />
    public async Task<decimal> GetTotalEndOfMonthBalanceAsync()
    {
        var excluded = await mediator.Send(new GetExcludedAccountQuery());
        var balance = await GetTotalBalanceAsync();
        foreach (var payment in await mediator.Send(new GetUnclearedPaymentsOfThisMonthQuery()))
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
                    balance = HandleTransfer(excluded: excluded, balance: balance, payment: payment);

                    break;
                default:
                    throw new InvalidPaymentTypeException();
            }
        }

        return balance;
    }

    /// <inheritdoc />
    public async Task<decimal> GetEndOfMonthBalanceForAccountAsync(AccountViewModel account)
    {
        var balance = account.CurrentBalance;
        var paymentList = await mediator.Send(new GetUnclearedPaymentsOfThisMonthQuery { AccountId = account.Id });
        foreach (var payment in paymentList)
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
                    balance = HandleTransferAmount(payment: payment, balance: balance, accountId: account.Id);

                    break;
                default:
                    throw new InvalidPaymentTypeException();
            }
        }

        return balance;
    }

    private static decimal HandleTransfer(List<Account> excluded, decimal balance, Payment payment)
    {
        foreach (var accountId in excluded.Select(x => x.Id))
        {
            if (payment.TargetAccount == null)
            {
                throw new InvalidOperationException("Uninitialized property: " + nameof(payment.TargetAccount));
            }

            if (Equals(objA: accountId, objB: payment.ChargedAccount.Id))
            {
                //Transfer from excluded account
                balance += payment.Amount;
            }

            if (Equals(objA: accountId, objB: payment.TargetAccount.Id))
            {
                //Transfer to excluded account
                balance -= payment.Amount;
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
