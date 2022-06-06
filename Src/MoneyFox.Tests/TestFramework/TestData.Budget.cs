namespace MoneyFox.Tests.TestFramework
{

    using System.Collections.Generic;
    using System.Collections.Immutable;

    internal static partial class TestData
    {
        internal sealed class DefaultBudget : IBudget
        {
            public int Id => 10;
            public string Name => "Beverages";
            public decimal SpendingLimit => 100.50m;
            public decimal CurrentSpending => 60.20m;

            public IReadOnlyList<int> Categories { get; set;  } = ImmutableList.Create(11);
        }

        internal interface IBudget
        {
            int Id { get; }
            string Name { get; }
            decimal SpendingLimit { get; }
            IReadOnlyList<int> Categories { get; }
        }
    }

}
