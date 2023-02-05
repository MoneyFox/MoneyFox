namespace MoneyFox.Core.Queries;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetNumberOfPaymentsAssignedToCategory
{
    public record Query(int CategoryId) : IRequest<int>;

    public class Handler : IRequestHandler<Query, int>
    {
        private readonly IAppDbContext dbContext;

        public Handler(IAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            return await dbContext.Payments.Where(p => p.Category != null).Where(p => p.Category!.Id == request.CategoryId).CountAsync(cancellationToken);
        }
    }
}
