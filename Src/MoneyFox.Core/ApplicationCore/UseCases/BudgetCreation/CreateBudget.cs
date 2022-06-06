namespace MoneyFox.Core.ApplicationCore.UseCases.BudgetCreation
{

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Aggregates.BudgetAggregate;
    using MediatR;

    public static class CreateBudget
    {
        public class Query : IRequest
        {
            public Query(string name, decimal spendingLimit, IReadOnlyList<int> categories)
            {
                Name = name;
                SpendingLimit = spendingLimit;
                Categories = categories;
            }

            public string Name { get; }
            public decimal SpendingLimit { get; }
            public IReadOnlyList<int> Categories { get; }
        }

        public class Handler : IRequestHandler<Query>
        {
            private readonly IBudgetRepository repository;

            public Handler(IBudgetRepository repository)
            {
                this.repository = repository;
            }

            public async Task<Unit> Handle(Query request, CancellationToken cancellationToken)
            {
                var budget = new Budget(request.Name, request.SpendingLimit, request.Categories);
                await repository.AddAsync(budget);

                return Unit.Value;
            }
        }
    }

}
