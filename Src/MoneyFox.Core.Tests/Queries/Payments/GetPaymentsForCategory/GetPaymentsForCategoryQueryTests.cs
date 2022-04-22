namespace MoneyFox.Core.Tests.Queries.Payments.GetPaymentsForCategory
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using ApplicationCore.Queries;
    using FluentAssertions;
    using Xunit;

    [ExcludeFromCodeCoverage]
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
            var query = new GetPaymentsForCategoryQuery(categoryId: catId, dateRangeFrom: dateRangeFrom, dateRangeTo: dateRangeTo);

            // Assert
            query.CategoryId.Should().Be(catId);
            query.DateRangeFrom.Should().Be(dateRangeFrom);
            query.DateRangeTo.Should().Be(dateRangeTo);
        }
    }

}
