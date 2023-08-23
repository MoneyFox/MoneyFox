namespace MoneyFox.Core.Tests.Queries.Payments.GetPaymentsForCategory;

using Core.Queries;

public class GetPaymentsForCategoryQueryTests
{
    [Fact]
    public void AssignCorrectlyInCtor()
    {
        // Arrange
        const int catId = 5234;
        var dateRangeFrom = DateTime.Now.AddDays(1);
        var dateRangeTo = DateTime.Now.AddDays(2);

        // Act
        var query = new GetPaymentsForCategorySummary.Query(CategoryId: catId, DateRangeFrom: dateRangeFrom, DateRangeTo: dateRangeTo);

        // Assert
        query.CategoryId.Should().Be(catId);
        query.DateRangeFrom.Should().Be(dateRangeFrom);
        query.DateRangeTo.Should().Be(dateRangeTo);
    }
}
