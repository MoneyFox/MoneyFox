namespace MoneyFox.Core.ApplicationCore.Queries.BudgetListLoading
{

    public class BudgetListData
    {
        public BudgetListData(int id, string name, decimal spendingLimit)
        {
            Id = id;
            Name = name;
            SpendingLimit = spendingLimit;
        }

        public int Id { get; }

        public string Name { get; }

        public decimal SpendingLimit { get; }
    }

}
