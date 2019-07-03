using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Resources;
using MoneyFox.Application.Statistics.Queries.GetCashFlow;
using MoneyFox.Application.Tests.Infrastructure;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Statistics.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetCashFlowQueryHandlerTests : IClassFixture<QueryTestFixture>
    {
        private readonly EfCoreContext context;

        public GetCashFlowQueryHandlerTests(QueryTestFixture fixture)
        {
            context = fixture.Context;
        }

        [Fact]
        public async Task GetValues_CorrectSums()
        {
            // Arrange
            context.AddRange(new List<Payment>
            {
                new Payment(DateTime.Today, 60, PaymentType.Income, new Account("Foo1")),
                new Payment(DateTime.Today, 70, PaymentType.Income, new Account("Foo2")),
                new Payment(DateTime.Today, 50, PaymentType.Expense, new Account("Foo3")),
                new Payment(DateTime.Today, 40, PaymentType.Expense, new Account("Foo3"))
            });
            context.SaveChanges();
            
            // Act
            var result = await new GetCashFlowQueryHandler(context).Handle(new GetCashFlowQuery
            {
                StartDate = DateTime.Today.AddDays(-3),
                EndDate = DateTime.Today.AddDays(3)
            }, default);

            // Assert
            result[0].Value.ShouldEqual(130);
            result[1].Value.ShouldEqual(90);
            result[2].Value.ShouldEqual(40);
        }

        [Fact]
        public async Task GetValues_CorrectColors()
        {
            // Arrange

            // Act
            var result = await new GetCashFlowQueryHandler(context).Handle(new GetCashFlowQuery
            {
                StartDate = DateTime.Today.AddDays(-3),
                EndDate = DateTime.Today.AddDays(3)
            }, default);

            // Assert
            result[0].Color.ShouldEqual("#9bcd9b");
            result[1].Color.ShouldEqual("#cd3700");
            result[2].Color.ShouldEqual("#87cefa");
        }

        [Fact]
        public async Task GetValues_CorrectLabels()
        {
            // Arrange

            // Act
            var result = await new GetCashFlowQueryHandler(context).Handle(new GetCashFlowQuery
            {
                StartDate = DateTime.Today.AddDays(-3),
                EndDate = DateTime.Today.AddDays(3)
            }, default);

            // Assert
            result[0].Label.ShouldEqual(Strings.RevenueLabel);
            result[1].Label.ShouldEqual(Strings.ExpenseLabel);
            result[2].Label.ShouldEqual(Strings.IncreaseLabel);
        }
    }
}
