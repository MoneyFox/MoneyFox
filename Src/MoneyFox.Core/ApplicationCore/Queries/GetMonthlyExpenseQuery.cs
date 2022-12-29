namespace MoneyFox.Core.ApplicationCore.Queries;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using _Pending_.Common;
using Common.Extensions;
using Common.Extensions.QueryObjects;
using Common.Helpers;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetMonthlyExpenseQuery : IRequest<decimal>
{
    public class Handler : IRequestHandler<GetMonthlyExpenseQuery, decimal>
    {
        private readonly IAppDbContext appDbContext;
        private readonly ISystemDateHelper systemDateHelper;

        public Handler(IAppDbContext appDbContext, ISystemDateHelper systemDateHelper)
        {
            this.appDbContext = appDbContext;
            this.systemDateHelper = systemDateHelper;
        }

        public async Task<decimal> Handle(GetMonthlyExpenseQuery request, CancellationToken cancellationToken)
        {
            return (await appDbContext.Payments.HasDateLargerEqualsThan(SystemDateHelperExtensions.GetFirstDayMonth(systemDateHelper))
                .HasDateSmallerEqualsThan(systemDateHelper.GetEndOfMonth())
                .IsExpense()
                .Select(x => x.Amount)
                .ToListAsync(cancellationToken)).Sum();
        }
    }
}
