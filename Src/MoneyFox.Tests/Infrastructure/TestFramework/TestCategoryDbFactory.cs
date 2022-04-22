namespace MoneyFox.Tests.Infrastructure.TestFramework
{

    using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;

    internal static class TestCategoryDbFactory
    {
        internal static Category CreateDbCategory(this TestData.ICategory category)
        {
            return new Category(category.Name, category.Note, category.RequireNote);
        }
    }
}
