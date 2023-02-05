namespace MoneyFox.Core.Features.BudgetCreation;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain.Aggregates.BudgetAggregate;
using MediatR;

public static class CreateBudget
{
    public record Command(string Name, SpendingLimit SpendingLimit, BudgetInterval BudgetInterval, IReadOnlyList<int> Categories) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
        {
            SpendingLimit spendingLimit = new(command.SpendingLimit);
            Budget budget = new(name: command.Name, spendingLimit: spendingLimit, interval: command.BudgetInterval, includedCategories: command.Categories);
            await appDbContext.AddAsync(entity: budget, cancellationToken: cancellationToken);
            await appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
