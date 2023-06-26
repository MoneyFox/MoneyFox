namespace MoneyFox.Domain.Tests.TestFramework;

using System.Collections.Immutable;
using Domain.Aggregates.BudgetAggregate;

internal static partial class TestData
{
    internal sealed class DefaultBudget : IBudget
    {
        public int Id { get; set; } = 10;
        public string Name { get; init; } = "Beverages";
        public SpendingLimit SpendingLimit { get; init; } = new(100.50m);
        public decimal CurrentSpending { get; init; } = 60.20m;
        public decimal MonthlyBudget => SpendingLimit / Interval;
        public BudgetInterval Interval { get; init; } = new(1);
        public IReadOnlyList<int> Categories { get; init; } = ImmutableList.Create(11);
    }

    internal interface IBudget
    {
        int Id { get; set; }
        string Name { get; }
        SpendingLimit SpendingLimit { get; }
        decimal CurrentSpending { get; }
        decimal MonthlyBudget { get; }
        BudgetInterval Interval { get; }
        IReadOnlyList<int> Categories { get; }
    }
}
