namespace MoneyFox.Core.Tests.Queries.Statistics.GetCategoryProgression;

using Core.Queries.Statistics;

public class GetCategoryProgressionQueryTests
{
    [Fact]
    public void ThrowsException_WhenStartDateIsAfterEndDate()
    {
        // Act
        var act = () => new GetCategoryProgression.Query(
            categoryId: 3,
            startDate: DateOnly.FromDateTime(DateTime.Today).AddDays(1),
            endDate: DateOnly.FromDateTime(DateTime.Today));

        // Assert
        act.Should().Throw<InvalidDateRangeException>();
    }

    [Fact]
    public void CreateQueryAndAssignValues()
    {
        // Arrange
        var categoryId = 3;

        // Act
        var query = new GetCategoryProgression.Query(
            categoryId: categoryId,
            startDate: DateOnly.FromDateTime(DateTime.Today),
            endDate: DateOnly.FromDateTime(DateTime.Today));

        // Assert
        query.CategoryId.Should().Be(categoryId);
        query.StartDate.Should().Be(DateOnly.FromDateTime(DateTime.Today));
        query.EndDate.Should().Be(DateOnly.FromDateTime(DateTime.Today));
    }
}
