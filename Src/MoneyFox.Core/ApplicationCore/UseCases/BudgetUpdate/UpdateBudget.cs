namespace MoneyFox.Core.ApplicationCore.UseCases.BudgetUpdate
{

    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public static class UpdateBudget
    {
        public class Command : IRequest
        {
            public Command(string name, decimal spendingLimit, IReadOnlyList<int> categories)
            {
                Name = name;
                SpendingLimit = spendingLimit;
                Categories = categories;
            }

            public string Name { get; }
            public decimal SpendingLimit { get; }
            public IReadOnlyList<int> Categories { get; }
        }

        public class Handler : IRequestHandler<UpdateBudget.Command, Unit>
        {
            public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new System.NotImplementedException();
            }
        }

    }

}
