namespace MoneyFox.Tests.Infrastructure.TestFramework
{

    internal static class TestData
    {
        public sealed class DefaultCategory : ICategory
        {
            public string Name { get; } = "Beverages";
            public string? Note { get; } = "Water, Beer, Wine and those..";
            public bool RequireNote { get; } = false;
        }

        public interface ICategory
        {
            string Name { get; }
            string? Note { get; }
            bool RequireNote { get; }
        }
    }

}
