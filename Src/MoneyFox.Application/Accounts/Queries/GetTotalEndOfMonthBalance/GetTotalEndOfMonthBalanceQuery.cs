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
        public class Handler : IRequestHandler<GetTotalEndOfMonthBalanceQuery, decimal>
        {
            private readonly Logger logManager = LogManager.GetCurrentClassLogger();

            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<decimal> Handle(GetTotalEndOfMonthBalanceQuery request, CancellationToken cancellationToken)
            {
                logManager.Info("Calculate EndOfMonth Balance.");

                List<Account> excluded = await contextAdapter.Context.Accounts.AreExcluded().ToListAsync();
                decimal balance = await GetCurrentAccountBalanceAsync();

                foreach(Payment payment in await GetUnclearedPaymentsForThisMonth())
                {
                    if(payment.ChargedAccount == null)
                        throw new InvalidOperationException($"Navigation Property not initialized properly: {nameof(payment.ChargedAccount)}");

                    switch(payment.Type)
                    {
                        case PaymentType.Expense:
                            balance -= payment.Amount;
                            break;

                        case PaymentType.Income:
                            balance += payment.Amount;
                            break;

                        case PaymentType.Transfer:
                            balance = CalculateBalanceForTransfer(excluded, balance, payment);
                            break;

                        default:
                            throw new InvalidPaymentTypeException();
                    }
                }

                return balance;
            }

            private async Task<decimal> GetCurrentAccountBalanceAsync()
                => (await contextAdapter.Context
                                 .Accounts
                                 .AreNotExcluded()
                                 .Select(x => x.CurrentBalance)
                                 .ToListAsync())
                                 .Sum();

            private async Task<List<Payment>> GetUnclearedPaymentsForThisMonth()
                => await contextAdapter.Context
                                       .Payments
                                       .Include(x => x.ChargedAccount)
                                       .Include(x => x.TargetAccount)
                                       .AreNotCleared()
                                       .HasDateSmallerEqualsThan(HelperFunctions.GetEndOfMonth())
                                       .ToListAsync();

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
                        throw new InvalidOperationException($"Navigation Property not initialized properly: {nameof(payment.TargetAccount)}");

                    if(Equals(account.Id, payment.TargetAccount.Id))
                    {
                        //Transfer to excluded account
                        balance -= payment.Amount;
                    }
                }

                return balance;
            }
        }
    }
}
