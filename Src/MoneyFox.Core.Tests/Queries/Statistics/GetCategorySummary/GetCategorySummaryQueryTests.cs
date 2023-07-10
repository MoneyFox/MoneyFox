namespace MoneyFox.Core.Tests.Queries.Statistics.GetCategorySummary;

using Core.Queries.Statistics;
using Core.Queries.Statistics.GetCategorySummary;
using FluentAssertions;

public sealed class GetCategorySummaryQueryTests
{
    [Fact]
    public void ThrowsException_WhenStartDateIsAfterEndDate()
    {
        // Act
        var act = () => new GetCategorySummary.Query(
            startDate: DateOnly.FromDateTime(DateTime.Today).AddDays(1),
            endDate: DateOnly.FromDateTime(DateTime.Today));

        // Assert
        act.Should().Throw<InvalidDateRangeException>();
    }

    [Fact]
    public void CreateQueryAndAssignValues()
    {
        // Act
        var query = new GetCategorySummary.Query(startDate: DateOnly.FromDateTime(DateTime.Today), endDate: DateOnly.FromDateTime(DateTime.Today));

        // Assert
        query.StartDate.Should().Be(DateOnly.FromDateTime(DateTime.Today));
        query.EndDate.Should().Be(DateOnly.FromDateTime(DateTime.Today));
    }
}
