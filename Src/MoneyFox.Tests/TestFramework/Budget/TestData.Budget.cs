namespace MoneyFox.Tests.TestFramework.Budget
{

    using System.Collections.Generic;

    internal static class TestData
    {
        internal sealed class DefaultBudget : IBudget
        {
            public int Id => 10;
            public string Name => "Beverages";
            public decimal SpendingLimit => 100.50m;

            public List<int> Categories { get; } = new List<int> { 11 };
        }

        internal interface IBudget
        {
            int Id { get; }
            string Name { get; }
            decimal SpendingLimit { get; }
            List<int> Categories { get; }
        }
    }

}
