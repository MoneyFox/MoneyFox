namespace MoneyFox.Core.Queries;

using System.Threading;
using System.Threading.Tasks;
using Common.Extensions.QueryObjects;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetAccountCountQuery : IRequest<int>
{
    public class Handler : IRequestHandler<GetAccountCountQuery, int>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<int> Handle(GetAccountCountQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Accounts.AreActive().CountAsync(cancellationToken);
        }
    }
}
