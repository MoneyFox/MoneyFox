namespace MoneyFox.Tests.TestFramework.Category
{

    internal static class TestData
    {
        public sealed class DefaultCategory : ICategory
        {
            public int Id { get; } = 10;
            public string Name { get; } = "Beverages";
            public string? Note { get; } = "Water, Beer, Wine and those..";
            public bool RequireNote { get; } = false;
        }

        public interface ICategory
        {
            int Id { get; }
            string Name { get; }
            string? Note { get; }
            bool RequireNote { get; }
        }
    }

}
