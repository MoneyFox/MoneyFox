namespace MoneyFox.Core.Features.BudgetDeletion;

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
        public Command(BudgetId budgetId)
        {
            BudgetId = budgetId;
        }

        public BudgetId BudgetId { get; }
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
            var budgetToRemove = await appDbContext.Budgets.FirstOrDefaultAsync(predicate: b => b.Id == command.BudgetId, cancellationToken: cancellationToken);
            if (budgetToRemove is null)
            {
                return;
            }

            appDbContext.Budgets.Remove(budgetToRemove);
            await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
