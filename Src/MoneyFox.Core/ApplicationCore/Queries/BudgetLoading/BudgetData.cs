namespace MoneyFox.Core.ApplicationCore.Queries.BudgetLoading
{

    using System.Collections.Generic;

    public sealed class BudgetData
    {
        public BudgetData(int id, string name, decimal spendingLimit, IReadOnlyList<BudgetCategory> categories)
        {
            Id = id;
            Name = name;
            SpendingLimit = spendingLimit;
            Categories = categories;
        }

        public int Id { get; }

        public string Name { get; }

        public decimal SpendingLimit { get; }

        public IReadOnlyList<BudgetCategory> Categories { get; }

        public class BudgetCategory
        {
            public BudgetCategory(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public int Id { get; }

            public string Name { get; }
        }
    }

}
