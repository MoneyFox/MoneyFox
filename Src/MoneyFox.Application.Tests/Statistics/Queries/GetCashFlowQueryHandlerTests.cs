using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Statistics.Queries;
using MoneyFox.Application.Tests.Infrastructure;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Statistics.Queries
{
    [Collection("QueryCollection")]
    [ExcludeFromCodeCoverage]
    public class GetCashFlowQueryHandlerTests
    {
        private readonly EfCoreContext context;

        public GetCashFlowQueryHandlerTests(QueryTestFixture fixture)
        {
            context = fixture.Context;

            context.AddRange(new List<Payment>
                            {
                                new Payment(DateTime.Today, 60, PaymentType.Income, new Account("Foo1")),
                                new Payment(DateTime.Today, 70, PaymentType.Income, new Account("Foo2")),
                                new Payment(DateTime.Today, 50, PaymentType.Expense, new Account("Foo3")),
                                new Payment(DateTime.Today, 40, PaymentType.Expense, new Account("Foo3"))
                            });
            context.SaveChanges();
        }

        [Fact]
        public async Task GetValues_CorrectSums()
        {
            // Arrange

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
    }
}
