namespace MoneyFox.Core.Queries;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetAccountNameByIdQuery(int accountId) : IRequest<string>
{
    public int AccountId { get; } = accountId;

    public class Handler(IAppDbContext dbContext) : IRequestHandler<GetAccountNameByIdQuery, string>
    {
        public async Task<string> Handle(GetAccountNameByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await dbContext.Accounts.Where(x => x.Id == request.AccountId).Select(x => x.Name).FirstOrDefaultAsync(cancellationToken);

            return account ?? string.Empty;
        }
    }
}
