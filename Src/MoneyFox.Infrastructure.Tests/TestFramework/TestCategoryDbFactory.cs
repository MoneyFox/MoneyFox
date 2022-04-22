namespace MoneyFox.Infrastructure.Tests.TestFramework
{

    using Core.Aggregates.CategoryAggregate;

    internal static class TestCategoryDbFactory
    {
        internal static Category CreateDbCategory(this TestData.ICategory category)
        {
            return new Category(category.Name, category.Note, category.RequireNote);
        }
    }
}
