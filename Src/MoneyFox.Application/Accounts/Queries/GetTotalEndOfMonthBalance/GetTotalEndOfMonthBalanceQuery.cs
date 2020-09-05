using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Helpers;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.QueryObjects;
using MoneyFox.Domain.Entities;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Accounts.Queries.GetTotalEndOfMonthBalance
{
    public class GetTotalEndOfMonthBalanceQuery : IRequest<decimal>
    {
        public GetTotalEndOfMonthBalanceQuery(int accountId = 0)
        {
            AccountId = accountId;
        }

        public int AccountId { get; }

        public class Handler : IRequestHandler<GetTotalEndOfMonthBalanceQuery, decimal>
        {
            private readonly Logger logManager = LogManager.GetCurrentClassLogger();

            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            private int accountId = 0;

            public async Task<decimal> Handle(GetTotalEndOfMonthBalanceQuery request, CancellationToken cancellationToken)
            {
                logManager.Info("Calculate EndOfMonth Balance.");

                logManager.Info($"Passed Account Id: {request.AccountId}");
                accountId = request.AccountId;

                List<Account> excluded = await contextAdapter.Context.Accounts.AreExcluded().ToListAsync();
                decimal balance = await GetCurrentAccountBalanceAsync();

                foreach(Payment payment in await GetUnclearedPaymentsForThisMonthAsync())
                {
                    if(payment.ChargedAccount == null)
                    {
                        throw new InvalidOperationException($"Navigation Property not initialized properly: {nameof(payment.ChargedAccount)}");
                    }

                    balance = PaymentAmountHelper.AddPaymentToBalance(payment, excluded, balance);
                }

                return balance;
            }

            private async Task<decimal> GetCurrentAccountBalanceAsync()
                => (await contextAdapter.Context
                                 .Accounts
                                 .WithId(accountId)
                                 .Select(x => x.CurrentBalance)
                                 .ToListAsync())
                                 .Sum();

            private async Task<List<Payment>> GetUnclearedPaymentsForThisMonthAsync()
                => await contextAdapter.Context
                                       .Payments
                                       .HasAccountId(accountId)
                                       .Include(x => x.ChargedAccount)
                                       .Include(x => x.TargetAccount)
                                       .AreNotCleared()
                                       .HasDateSmallerEqualsThan(HelperFunctions.GetEndOfMonth())
                                       .ToListAsync();
        }
    }
}
