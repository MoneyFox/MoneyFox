namespace MoneyFox.Core.Tests.Queries.Statistics.GetCategorySpreading;

using Core.Queries.Statistics;
using Domain.Aggregates.AccountAggregate;
using FluentAssertions;

public class GetCategorySpreadingQueryTests
{
    [Fact]
    public void ThrowsException_WhenStartDateIsAfterEndDate()
    {
        // Act
        var act = () => new GetCategorySpreading.Query(
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
        var query = new GetCategorySpreading.Query(
            startDate: DateOnly.FromDateTime(DateTime.Today),
            endDate: DateOnly.FromDateTime(DateTime.Today),
            paymentType: PaymentType.Income,
            numberOfCategoriesToShow: 15);

        // Assert
        query.StartDate.Should().Be(DateOnly.FromDateTime(DateTime.Today));
        query.EndDate.Should().Be(DateOnly.FromDateTime(DateTime.Today));
        query.PaymentType.Should().Be(PaymentType.Income);
        query.NumberOfCategoriesToShow.Should().Be(15);
    }
}
