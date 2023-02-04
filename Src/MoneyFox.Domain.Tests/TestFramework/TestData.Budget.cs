namespace MoneyFox.Domain.Tests.TestFramework;

using System.Collections.Immutable;
using Domain.Aggregates.BudgetAggregate;

internal static partial class TestData
{
    internal sealed class DefaultBudget : IBudget
    {
        public int Id { get; set; } = 10;
        public string Name { get; set; } = "Beverages";
        public SpendingLimit SpendingLimit { get; set; } = new(100.50m);
        public decimal CurrentSpending { get; set; } = 60.20m;
        public BudgetInterval Interval { get; set; } = new BudgetInterval(1);
        public BudgetTimeRange BudgetTimeRange { get; set; } = BudgetTimeRange.Last2Years;
        public IReadOnlyList<int> Categories { get; set; } = ImmutableList.Create(11);
    }

    internal interface IBudget
    {
        int Id { get; set; }
        string Name { get; }
        SpendingLimit SpendingLimit { get; }
        decimal CurrentSpending { get; }
        BudgetInterval Interval { get; }
        BudgetTimeRange BudgetTimeRange { get; }
        IReadOnlyList<int> Categories { get; }
    }
}
