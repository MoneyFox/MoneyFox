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
        var act = () => new GetCategorySummary.Query(StartDate: DateTime.Today.AddDays(1), EndDate: DateTime.Today);

        // Assert
        act.Should().Throw<InvalidDateRangeException>();
    }

    [Fact]
    public void CreateQueryAndAssignValues()
    {
        // Act
        var query = new GetCategorySummary.Query(StartDate: DateTime.Today, EndDate: DateTime.Today);

        // Assert
        query.StartDate.Should().Be(DateTime.Today);
        query.EndDate.Should().Be(DateTime.Today);
    }
}
