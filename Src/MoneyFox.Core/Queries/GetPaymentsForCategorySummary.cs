namespace MoneyFox.Core.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions;
using Common.Extensions.QueryObjects;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetPaymentsForCategorySummary
{
    public record Query(int? CategoryId, DateTime DateRangeFrom, DateTime DateRangeTo) : IRequest<IReadOnlyList<PaymentData>>;

    public class Handler : IRequestHandler<Query, IReadOnlyList<PaymentData>>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<IReadOnlyList<PaymentData>> Handle(Query request, CancellationToken cancellationToken)
        {
            IQueryable<Payment> query = appDbContext.Payments.Include(x => x.Category);
            query = request.CategoryId.HasValue ? query.Where(x => x.Category!.Id == request.CategoryId) : query.Where(x => x.Category == null);

            return await query.Where(x => x.Date >= request.DateRangeFrom)
                .Where(x => x.Date <= request.DateRangeTo)
                .WithoutTransfers()
                .Select(
                    p => new PaymentData(
                        p.Id,
                        p.ChargedAccount.Id,
                        p.Amount,
                        p.Category == null ? null : p.Category.Name,
                        p.Date.ToDateOnly(),
                        p.Note,
                        p.IsCleared,
                        p.IsRecurring,
                        p.Type))
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }

    public record PaymentData(
        int Id,
        int ChargedAccountId,
        decimal Amount,
        string? CategoryName,
        DateOnly Date,
        string? Note,
        bool IsCleared,
        bool IsRecurring,
        PaymentType Type);
}
