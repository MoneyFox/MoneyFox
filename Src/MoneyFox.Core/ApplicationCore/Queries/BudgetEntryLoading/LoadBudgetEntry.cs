namespace MoneyFox.Core.ApplicationCore.Queries.BudgetEntryLoading
{

    using MediatR;

    public static class LoadBudgetEntry
    {
        public class Query : IRequest<BudgetEntryData>
        {
            public Query(int budgetId)
            {
                BudgetId = budgetId;
            }

            public int BudgetId { get; }
        }
    }

}
