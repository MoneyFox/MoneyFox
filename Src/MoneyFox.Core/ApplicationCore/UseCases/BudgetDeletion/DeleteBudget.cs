namespace MoneyFox.Core.ApplicationCore.UseCases.BudgetDeletion
{

    public static class DeleteBudget
    {
        public class Command
        {
            public Command(int budgetId)
            {
                BudgetId = budgetId;
            }

            public int BudgetId { get; }
        }
    }

}
