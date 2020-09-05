using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.QueryObjects;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Domain.Exceptions;
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

                    balance = AddPaymentToBalance(payment, excluded, balance);
                }

                return balance;
            }

            public static decimal AddPaymentToBalance(Payment payment, List<Account> excluded, decimal currentBalance)
            {
                switch(payment.Type)
                {
                    case PaymentType.Expense:
                        currentBalance -= payment.Amount;
                        break;

                    case PaymentType.Income:
                        currentBalance += payment.Amount;
                        break;

                    case PaymentType.Transfer:
                        currentBalance = CalculateBalanceForTransfer(excluded, currentBalance, payment);
                        break;

                    default:
                        throw new InvalidPaymentTypeException();
                }

                return currentBalance;
            }

            private static decimal CalculateBalanceForTransfer(List<Account> excluded, decimal balance, Payment payment)
            {
                foreach(Account account in excluded)
                {
                    if(Equals(account.Id, payment.ChargedAccount!.Id))
                    {
                        //Transfer from excluded account
                        balance += payment.Amount;
                    }

                    if(payment.TargetAccount == null)
                    {
                        throw new InvalidOperationException($"Navigation Property not initialized properly: {nameof(payment.TargetAccount)}");
                    }

                    if(Equals(account.Id, payment.TargetAccount.Id))
                    {
                        //Transfer to excluded account
                        balance -= payment.Amount;
                    }
                }

                return balance;
            }

            private async Task<decimal> GetCurrentAccountBalanceAsync()
            {
                var query = contextAdapter.Context.Accounts.AsQueryable();

                if(accountId != 0)
                {
                    query = query.WithId(accountId);
                }

                return (await query.Select(x => x.CurrentBalance)
                                  .ToListAsync())
                                  .Sum();
            }

            private async Task<List<Payment>> GetUnclearedPaymentsForThisMonthAsync()
            {
                var query = contextAdapter.Context.Payments.AsQueryable();

                if(accountId != 0)
                {
                    query = query.HasAccountId(accountId);
                }

                return await query.Include(x => x.ChargedAccount)
                                  .Include(x => x.TargetAccount)
                                  .AreNotCleared()
                                  .HasDateSmallerEqualsThan(HelperFunctions.GetEndOfMonth())
                                  .ToListAsync();
            }
        }
    }
}
