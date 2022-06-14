namespace MoneyFox.Core.ApplicationCore.UseCases.BudgetUpdate
{

    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Aggregates.BudgetAggregate;
    using MediatR;

    public static class UpdateBudget
    {
        public class Command : IRequest
        {
            public Command(int budgetId, string name, decimal spendingLimit, IReadOnlyList<int> categories)
            {
                BudgetId = budgetId;
                Name = name;
                SpendingLimit = spendingLimit;
                Categories = categories;
            }

            public int BudgetId { get; }
            public string Name { get; }
            public decimal SpendingLimit { get; }
            public IReadOnlyList<int> Categories { get; }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IBudgetRepository budgetRepository;

            public Handler(IBudgetRepository budgetRepository)
            {
                this.budgetRepository = budgetRepository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var loadedBudget = await budgetRepository.GetAsync(request.BudgetId);
                var spendingLimit = new SpendingLimit(request.SpendingLimit);
                loadedBudget.Change(budgetName: request.Name, spendingLimit: spendingLimit, includedCategories: request.Categories);
                await budgetRepository.UpdateAsync(loadedBudget);

                return Unit.Value;
            }
        }
    }

}
