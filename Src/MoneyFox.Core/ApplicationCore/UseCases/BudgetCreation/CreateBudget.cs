namespace MoneyFox.Core.ApplicationCore.UseCases.BudgetCreation
{

    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Aggregates.BudgetAggregate;
    using MediatR;

    internal sealed class CreateBudget
    {
        public class Query : IRequest
        {
            public Query(string name, double spendingLimit)
            {
                Name = name;
                SpendingLimit = spendingLimit;
            }

            public string Name { get; }
            public double SpendingLimit { get; }
        }

        public class Handler : IRequestHandler<CreateBudget.Query>
        {
            private IBudgetRepository repository;

            public Handler(IBudgetRepository repository)
            {
                this.repository = repository;
            }

            public Task<Unit> Handle(Query request, CancellationToken cancellationToken)
            {
                throw new System.NotImplementedException();
            }
        }
    }

}
