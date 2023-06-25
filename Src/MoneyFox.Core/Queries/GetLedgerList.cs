namespace MoneyFox.Core.Queries;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain;
using Domain.Aggregates.LedgerAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

internal static class GetLedgerList
{
    public record Query : IRequest<IReadOnlyCollection<LedgerData>>;

    public record LedgerData(LedgerId LedgerId, string Name, Money CurrentBalance, bool IsExcludeFromEndOfMonthSummary);

    public class Handler : IRequestHandler<Query, IReadOnlyCollection<LedgerData>>
    {
        private readonly IAppDbContext context;

        public Handler(IAppDbContext context)
        {
            this.context = context;
        }

        public async Task<IReadOnlyCollection<LedgerData>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await context.Ledgers.OrderBy(l => l.Name)
                .Select(l => new LedgerData(l.Id, l.Name, new(l.OpeningBalance, l.Currency), l.IsExcludeFromEndOfMonthSummary))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
