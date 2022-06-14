namespace MoneyFox.Tests.TestFramework
{

    using System.Collections.Generic;
    using System.Collections.Immutable;

    internal static partial class TestData
    {
        internal sealed class DefaultBudget : IBudget
        {
            public int Id { get; set; } = 10;
            public string Name { get; set; } = "Beverages";
            public decimal SpendingLimit { get; set; } = 100.50m;
            public decimal CurrentSpending { get; set; } = 60.20m;

            public IReadOnlyList<int> Categories { get; set; } = ImmutableList.Create(11);
        }

        internal interface IBudget
        {
            int Id { get; }
            string Name { get; }
            decimal SpendingLimit { get; }
            decimal CurrentSpending { get; }
            IReadOnlyList<int> Categories { get; }
        }
    }

}
