namespace MoneyFox.Core.ApplicationCore.UseCases.BudgetCreation;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly IBudgetRepository repository;

        public Handler(IBudgetRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            SpendingLimit spendingLimit = new(request.SpendingLimit);
            Budget budget = new(name: request.Name, spendingLimit: spendingLimit, timeRange: request.BudgetTimeRange, includedCategories: request.Categories);
            await repository.AddAsync(budget);

            return Unit.Value;
        }
    }
}
