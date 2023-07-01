namespace MoneyFox.Core.Tests.Queries.Statistics.GetCashFlowQueryHandler;

using FluentAssertions;
using MoneyFox.Core.Queries.Statistics;

public class GetCashFlowQueryTests
{
    [Fact]
    public void ThrowsException_WhenStartDateIsAfterEndDate()
    {
        // Act
        var act = () => new GetCashFlow.Query(
            StartDate: DateOnly.FromDateTime(DateTime.Today).AddDays(1),
            EndDate: DateOnly.FromDateTime(DateTime.Today));

        // Assert
        act.Should().Throw<InvalidDateRangeException>();
    }

    [Fact]
    public void CreateQueryAndAssignValues()
    {
        // Act
        var query = new GetCashFlow.Query(StartDate: DateOnly.FromDateTime(DateTime.Today), EndDate: DateOnly.FromDateTime(DateTime.Today));

        // Assert
        query.StartDate.Should().Be(DateOnly.FromDateTime(DateTime.Today));
        query.EndDate.Should().Be(DateOnly.FromDateTime(DateTime.Today));
    }
}
