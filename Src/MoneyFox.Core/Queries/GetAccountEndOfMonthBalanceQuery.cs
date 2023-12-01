namespace MoneyFox.Core.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Extensions;
using Common.Extensions.QueryObjects;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetAccountEndOfMonthBalanceQuery(int accountId) : IRequest<decimal>
{
    private int AccountId { get; } = accountId;

    public class Handler(IAppDbContext dbContext, ISystemDateHelper dateHelper) : IRequestHandler<GetAccountEndOfMonthBalanceQuery, decimal>
    {
        private int accountId;

        public async Task<decimal> Handle(GetAccountEndOfMonthBalanceQuery request, CancellationToken cancellationToken)
        {
            accountId = request.AccountId;
            var account = await dbContext.Accounts.WithId(accountId).FirstAsync(cancellationToken: cancellationToken);
            var balance = await GetCurrentAccountBalanceAsync();
            foreach (var payment in await GetUnclearedPaymentsForThisMonthAsync())
            {
                if (payment.ChargedAccount == null)
                {
                    throw new InvalidOperationException($"Navigation Property not initialized properly: {nameof(payment.ChargedAccount)}");
                }

                balance = AddPaymentToBalance(payment: payment, account: account, currentBalance: balance);
            }

            return balance;
        }

        private static decimal AddPaymentToBalance(Payment payment, Account account, decimal currentBalance)
        {
            switch (payment.Type)
            {
                case PaymentType.Expense:
                    currentBalance -= payment.Amount;

                    break;
                case PaymentType.Income:
                    currentBalance += payment.Amount;

                    break;
                case PaymentType.Transfer:
                    currentBalance = CalculateBalanceForTransfer(account: account, balance: currentBalance, payment: payment);

                    break;
                default:
                    throw new InvalidPaymentTypeException();
            }

            return currentBalance;
        }

        private static decimal CalculateBalanceForTransfer(Account account, decimal balance, Payment payment)
        {
            if (Equals(objA: account.Id, objB: payment.ChargedAccount.Id))
            {
                //Transfer from excluded account
                balance -= payment.Amount;
            }

            if (payment.TargetAccount == null)
            {
                throw new InvalidOperationException($"Navigation Property not initialized properly: {nameof(payment.TargetAccount)}");
            }

            if (Equals(objA: account.Id, objB: payment.TargetAccount.Id))
            {
                //Transfer to excluded account
                balance += payment.Amount;
            }

            return balance;
        }

        private async Task<decimal> GetCurrentAccountBalanceAsync()
        {
            return (await dbContext.Accounts.WithId(accountId).Select(x => x.CurrentBalance).ToListAsync()).Sum();
        }

        private async Task<List<Payment>> GetUnclearedPaymentsForThisMonthAsync()
        {
            return await dbContext.Payments.HasAccountId(accountId)
                .Include(x => x.ChargedAccount)
                .Include(x => x.TargetAccount)
                .AreNotCleared()
                .HasDateSmallerEqualsThan(dateHelper.GetEndOfMonth())
                .ToListAsync();
        }
    }
}
