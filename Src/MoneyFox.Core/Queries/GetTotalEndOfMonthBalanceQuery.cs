namespace MoneyFox.Core.Queries
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using _Pending_;
    using _Pending_.Common;
    using _Pending_.Common.QueryObjects;
    using _Pending_.Exceptions;
    using Aggregates;
    using Aggregates.Payments;
    using Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using NLog;

    public class GetTotalEndOfMonthBalanceQuery : IRequest<decimal>
    {
        public class Handler : IRequestHandler<GetTotalEndOfMonthBalanceQuery, decimal>
        {
            private readonly Logger logManager = LogManager.GetCurrentClassLogger();

            private readonly IContextAdapter contextAdapter;
            private readonly ISystemDateHelper systemDateHelper;

            public Handler(IContextAdapter contextAdapter, ISystemDateHelper systemDateHelper)
            {
                this.contextAdapter = contextAdapter;
                this.systemDateHelper = systemDateHelper;
            }

            public async Task<decimal> Handle(GetTotalEndOfMonthBalanceQuery request, CancellationToken cancellationToken)
            {
                logManager.Info("Calculate EndOfMonth Balance.");
                var excluded = await contextAdapter.Context.Accounts.AreActive().AreExcluded().ToListAsync();
                var balance = await GetCurrentAccountBalanceAsync();
                foreach (var payment in await GetUnclearedPaymentsForThisMonthAsync())
                {
                    if (payment.ChargedAccount == null)
                    {
                        throw new InvalidOperationException($"Navigation Property not initialized properly: {nameof(payment.ChargedAccount)}");
                    }

                    balance = AddPaymentToBalance(payment: payment, excluded: excluded, currentBalance: balance);
                }

                return balance;
            }

            public static decimal AddPaymentToBalance(Payment payment, List<Account> excluded, decimal currentBalance)
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
                        currentBalance = CalculateBalanceForTransfer(excluded: excluded, balance: currentBalance, payment: payment);

                        break;
                    default:
                        throw new InvalidPaymentTypeException();
                }

                return currentBalance;
            }

            private static decimal CalculateBalanceForTransfer(List<Account> excluded, decimal balance, Payment payment)
            {
                foreach (var account in excluded)
                {
                    if (Equals(objA: account.Id, objB: payment.ChargedAccount.Id))
                    {
                        //Transfer from excluded account
                        balance += payment.Amount;
                    }

                    if (payment.TargetAccount == null)
                    {
                        throw new InvalidOperationException($"Navigation Property not initialized properly: {nameof(payment.TargetAccount)}");
                    }

                    if (Equals(objA: account.Id, objB: payment.TargetAccount.Id))
                    {
                        //Transfer to excluded account
                        balance -= payment.Amount;
                    }
                }

                return balance;
            }

            private async Task<decimal> GetCurrentAccountBalanceAsync()
            {
                return (await contextAdapter.Context.Accounts.AreActive().AreNotExcluded().Select(x => x.CurrentBalance).ToListAsync()).Sum();
            }

            private async Task<List<Payment>> GetUnclearedPaymentsForThisMonthAsync()
            {
                return await contextAdapter.Context.Payments.Include(x => x.ChargedAccount)
                    .Include(x => x.TargetAccount)
                    .AreNotCleared()
                    .HasDateSmallerEqualsThan(HelperFunctions.GetEndOfMonth(systemDateHelper))
                    .ToListAsync();
            }
        }
    }

}
