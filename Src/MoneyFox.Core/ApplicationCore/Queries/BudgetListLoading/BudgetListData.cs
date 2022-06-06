namespace MoneyFox.Core.ApplicationCore.Queries.BudgetListLoading
{

    public class BudgetListData
    {
        public BudgetListData(string name, decimal spendingLimit)
        {
            Name = name;
            SpendingLimit = spendingLimit;
        }

        public string Name { get; }

        public decimal SpendingLimit { get; }
    }

}
