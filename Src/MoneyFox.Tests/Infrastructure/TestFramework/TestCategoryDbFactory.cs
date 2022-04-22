namespace MoneyFox.Tests.Infrastructure.TestFramework
{

    using MoneyFox.Core.Aggregates.CategoryAggregate;

    internal static class TestCategoryDbFactory
    {
        internal static Category CreateDbCategory(this TestData.ICategory category)
        {
            return new Category(category.Name, category.Note, category.RequireNote);
        }
    }
}
