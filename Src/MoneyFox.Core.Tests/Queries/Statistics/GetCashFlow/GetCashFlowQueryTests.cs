namespace MoneyFox.Core.Tests.Queries.Statistics.GetCashFlow;

using Core.Queries.Statistics;
using FluentAssertions;

public class GetCashFlowQueryTests
{
    [Fact]
    public void ThrowsException_WhenStartDateIsAfterEndDate()
    {
        // Act
        var act = () => new GetCashFlow.Query(startDate: DateOnly.FromDateTime(DateTime.Today).AddDays(1), endDate: DateOnly.FromDateTime(DateTime.Today));

        // Assert
        act.Should().Throw<InvalidDateRangeException>();
    }

    [Fact]
    public void CreateQueryAndAssignValues()
    {
        // Act
        var query = new GetCashFlow.Query(startDate: DateOnly.FromDateTime(DateTime.Today), endDate: DateOnly.FromDateTime(DateTime.Today));

        // Assert
        query.StartDate.Should().Be(DateOnly.FromDateTime(DateTime.Today));
        query.EndDate.Should().Be(DateOnly.FromDateTime(DateTime.Today));
    }
}
