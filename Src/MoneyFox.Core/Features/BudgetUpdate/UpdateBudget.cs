namespace MoneyFox.Core.Features.BudgetUpdate;

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
            BudgetId budgetId,
            string name,
            decimal spendingLimit,
            int numberOfMonths,
            IReadOnlyList<int> categories)
        {
            BudgetId = budgetId;
            Name = name;
            SpendingLimit = spendingLimit;
            Categories = categories;
            NumberOfMonths = numberOfMonths;
        }

        public BudgetId BudgetId { get; }
        public string Name { get; }
        public decimal SpendingLimit { get; }
        public int NumberOfMonths { get; }
        public IReadOnlyList<int> Categories { get; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var loadedBudget = await appDbContext.Budgets.SingleAsync(predicate: b => b.Id == command.BudgetId, cancellationToken: cancellationToken);
            SpendingLimit spendingLimit = new(command.SpendingLimit);
            loadedBudget.Change(
                budgetName: command.Name,
                spendingLimit: spendingLimit,
                includedCategories: command.Categories,
                budgetInterval: new(command.NumberOfMonths));

            await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
