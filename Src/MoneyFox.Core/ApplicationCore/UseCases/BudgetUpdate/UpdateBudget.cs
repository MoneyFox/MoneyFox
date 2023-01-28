namespace MoneyFox.Core.ApplicationCore.UseCases.BudgetUpdate;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain.Aggregates.BudgetAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class UpdateBudget
{
    public class Command : IRequest
    {
        public Command(
            int budgetId,
            string name,
            decimal spendingLimit,
            BudgetTimeRange budgetTimeRange,
            IReadOnlyList<int> categories)
        {
            BudgetId = budgetId;
            Name = name;
            SpendingLimit = spendingLimit;
            Categories = categories;
            BudgetTimeRange = budgetTimeRange;
        }

        public int BudgetId { get; }
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
            var loadedBudget = await appDbContext.Budgets.SingleAsync(predicate: b => b.Id == request.BudgetId, cancellationToken: cancellationToken);
            SpendingLimit spendingLimit = new(request.SpendingLimit);
            loadedBudget.Change(
                budgetName: request.Name,
                spendingLimit: spendingLimit,
                includedCategories: request.Categories,
                timeRange: request.BudgetTimeRange);

            await appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
