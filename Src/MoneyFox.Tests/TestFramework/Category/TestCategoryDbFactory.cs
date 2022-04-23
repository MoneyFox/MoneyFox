namespace MoneyFox.Tests.TestFramework.Category
{

    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;

    internal static class TestCategoryDbFactory
    {
        internal static Category CreateDbCategory(this TestData.ICategory category)
        {
            return new Category(name: category.Name, note: category.Note, requireNote: category.RequireNote);
        }
    }

}
