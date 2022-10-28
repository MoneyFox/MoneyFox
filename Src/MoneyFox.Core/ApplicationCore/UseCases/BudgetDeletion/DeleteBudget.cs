namespace MoneyFox.Core.ApplicationCore.UseCases.BudgetDeletion;

using System.Threading;
using System.Threading.Tasks;
using Domain.Aggregates.BudgetAggregate;
using MediatR;

public static class DeleteBudget
{
    public class Command : IRequest
    {
        public Command(int budgetId)
        {
            BudgetId = budgetId;
        }

        public int BudgetId { get; }
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
            await budgetRepository.DeleteAsync(request.BudgetId);

            return Unit.Value;
        }
    }
}
