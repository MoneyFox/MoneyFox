namespace MoneyFox.Domain.Tests.TestFramework;

using Aggregates.CategoryAggregate;

internal static class TestCategoryDbFactory
{
    internal static Category CreateDbCategory(this TestData.ICategory category)
    {
        return new(name: category.Name, note: category.Note, requireNote: category.RequireNote);
    }
}
