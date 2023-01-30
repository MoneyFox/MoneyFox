namespace MoneyFox.Core.Features.BudgetCreation;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain.Aggregates.BudgetAggregate;
using MediatR;

public static class CreateBudget
{
    public class Command : IRequest
    {
        public Command(string name, decimal spendingLimit, BudgetTimeRange budgetTimeRange, IReadOnlyList<int> categories)
        {
            Name = name;
            SpendingLimit = spendingLimit;
            Categories = categories;
            BudgetTimeRange = budgetTimeRange;
        }

        public string Name { get; }
        public decimal SpendingLimit { get; }
        public BudgetTimeRange BudgetTimeRange { get; }
        public IReadOnlyList<int> Categories { get; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            SpendingLimit spendingLimit = new(request.SpendingLimit);
            Budget budget = new(name: request.Name, spendingLimit: spendingLimit, timeRange: request.BudgetTimeRange, includedCategories: request.Categories);
            await appDbContext.AddAsync(entity: budget, cancellationToken: cancellationToken);
            await appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
