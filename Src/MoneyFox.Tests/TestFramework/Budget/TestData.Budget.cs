namespace MoneyFox.Tests.TestFramework.Budget
{

    using System.Collections.Generic;

    internal static class TestData
    {
        internal sealed class DefaultBudget : IBudget
        {
            public string Name { get; } = "Beverages";
            public double SpendingLimit { get; } = 100.50;

            public List<int> Categories { get; } = new List<int> { 11 };
        }

        internal interface IBudget
        {
            string Name { get; }
            double SpendingLimit { get; }
            List<int> Categories { get; }
        }
    }

}
