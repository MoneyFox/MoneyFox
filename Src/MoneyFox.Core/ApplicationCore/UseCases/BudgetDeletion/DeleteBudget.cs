namespace MoneyFox.Core.ApplicationCore.UseCases.BudgetDeletion;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain.Aggregates.BudgetAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var budgetToRemove = await appDbContext.Budgets.FirstOrDefaultAsync(b => b.Id == request.BudgetId, cancellationToken);
            if (budgetToRemove is null)
            {
                return Unit.Value;
            }

            appDbContext.Budgets.Remove(budgetToRemove);
            await appDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
