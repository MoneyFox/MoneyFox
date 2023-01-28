namespace MoneyFox.Domain.Tests.TestFramework;

using FluentAssertions;
using FluentAssertions.Execution;
using MoneyFox.Domain.Aggregates.CategoryAggregate;

internal static class CategoryAssertion
{
    public static void AssertCategory(Category actual, TestData.DefaultCategory expected)
    {
        using (new AssertionScope())
        {
            actual.Name.Should().Be(expected.Name);
            actual.Note.Should().Be(expected.Note);
            actual.RequireNote.Should().Be(expected.RequireNote);
        }
    }
}
