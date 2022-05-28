namespace MoneyFox.Tests.TestFramework.Budget
{

    internal static class TestData
    {
        internal sealed class DefaultBudget : IBudget
        {
            public string Name { get; } = "Beverages";
            public double SpendingLimit { get; } = 100.50;
        }

        internal interface IBudget
        {
            string Name { get; }
            double SpendingLimit { get; }
        }
    }

}
