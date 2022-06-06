namespace MoneyFox.Tests.TestFramework.Budget
{

    using System.Collections.Generic;

    internal static class TestData
    {
        internal sealed class DefaultBudget : IBudget
        {
            public string Name { get; } = "Beverages";
            public decimal SpendingLimit { get; } = 100.50m;

            public List<int> Categories { get; } = new List<int> { 11 };
        }

        internal interface IBudget
        {
            string Name { get; }
            decimal SpendingLimit { get; }
            List<int> Categories { get; }
        }
    }

}
