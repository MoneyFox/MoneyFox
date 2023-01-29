namespace MoneyFox.Domain.Tests.TestFramework;

using System.Collections.Immutable;

internal static partial class TestData
{
    internal sealed class DefaultBudget : IBudget
    {
        public int Id { get; set; } = 10;
        public string Name { get; set; } = "Beverages";
        public decimal SpendingLimit { get; set; } = 100.50m;
        public decimal CurrentSpending { get; set; } = 60.20m;
        public BudgetTimeRange BudgetTimeRange { get; set; } = BudgetTimeRange.Last2Years;
        public IReadOnlyList<int> Categories { get; set; } = ImmutableList.Create(11);
    }

    internal interface IBudget
    {
        int Id { get; set; }
        string Name { get; }
        decimal SpendingLimit { get; }
        decimal CurrentSpending { get; }
        BudgetTimeRange BudgetTimeRange { get; }
        IReadOnlyList<int> Categories { get; }
    }
}
