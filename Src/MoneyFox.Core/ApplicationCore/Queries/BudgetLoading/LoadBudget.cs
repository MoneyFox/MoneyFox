namespace MoneyFox.Core.ApplicationCore.Queries.BudgetLoading
{

    using MediatR;

    public static class LoadBudget
    {
        public class Query : IRequest<BudgetData>
        {
            public Query(int budgetId)
            {
                BudgetId = budgetId;
            }

            public int BudgetId { get; }
        }
    }

}
