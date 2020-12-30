using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.QueryObjects;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Statistics.Queries
{
    public class GetAccountProgressionQuery : IRequest<List<StatisticEntry>>
    {
        public int AccountId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
    public class GetAccountProgressionHandler : IRequestHandler<GetAccountProgressionQuery, List<StatisticEntry>>
    {
        private readonly IContextAdapter contextAdapter;

        public GetAccountProgressionHandler(IContextAdapter contextAdapter)
        {
            this.contextAdapter = contextAdapter;
        }

        public async Task<List<StatisticEntry>> Handle(GetAccountProgressionQuery request, CancellationToken cancellationToken)
        {
            var payments = await contextAdapter.Context
                                               .Payments
                                               .Include(x => x.Category)
                                               .HasAccountId(request.AccountId)
                                               .HasDateLargerEqualsThan(request.StartDate.Date)
                                               .HasDateSmallerEqualsThan(request.EndDate.Date)
                                               .ToListAsync(cancellationToken);

            return payments.GroupBy(x => new { x.Date.Month, x.Date.Year })
                           .Select(x => new StatisticEntry(x.Sum(x => GetPaymentAmountForSum(x, request)), $"{x.Key.Month:d2} {x.Key.Year:yyyy}", ""))
                           .ToList();
        }

        private decimal GetPaymentAmountForSum(Payment payment, GetAccountProgressionQuery request)
        {
            return payment.Type switch
            {
                PaymentType.Expense => -payment.Amount,
                PaymentType.Income => payment.Amount,
                PaymentType.Transfer => payment.ChargedAccount.Id == request.AccountId
                                            ? -payment.Amount
                                            : payment.Amount,
                _ => 0,
            };
        }
    }

}
