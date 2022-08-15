namespace MoneyFox.Tests.Core.ApplicationCore.Queries.Payments.GetPaymentsForCategory
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
    using MoneyFox.Core.ApplicationCore.Queries;
    using MoneyFox.Infrastructure.Persistence;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetPaymentsForCategoryQueryHandlerTests
    {
        private readonly AppDbContext context;
        private readonly GetPaymentsForCategoryQuery.Handler handler;

        public GetPaymentsForCategoryQueryHandlerTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            handler = new GetPaymentsForCategoryQuery.Handler(context);
        }

        [Fact]
        public async Task CorrectPaymentsSelected()
        {
            // Arrange
            var account = new Account("asdf");
            var category = new Category("Test");
            var payment1 = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: account);
            var payment2 = new Payment(
                date: DateTime.Now,
                amount: 30,
                type: PaymentType.Expense,
                chargedAccount: account,
                category: category);

            var payment3 = new Payment(
                date: DateTime.Now,
                amount: 40,
                type: PaymentType.Expense,
                chargedAccount: account,
                category: category);

            context.Add(payment1);
            context.Add(payment2);
            context.Add(payment3);
            await context.SaveChangesAsync();

            // Act
            var result = await handler.Handle(
                request: new GetPaymentsForCategoryQuery(
                    categoryId: category.Id,
                    dateRangeFrom: DateTime.Now.AddDays(-1),
                    dateRangeTo: DateTime.Now.AddDays(1)),
                cancellationToken: default);

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task CorrectPaymentsSelectedWithNoCategory()
        {
            // Arrange
            var account = new Account("asdf");
            var category = new Category("Test");
            var payment1 = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: account);
            var payment2 = new Payment(
                date: DateTime.Now,
                amount: 30,
                type: PaymentType.Expense,
                chargedAccount: account,
                category: category);

            var payment3 = new Payment(
                date: DateTime.Now,
                amount: 40,
                type: PaymentType.Expense,
                chargedAccount: account,
                category: category);

            context.Add(payment1);
            context.Add(payment2);
            context.Add(payment3);
            await context.SaveChangesAsync();

            // Act
            var result = await handler.Handle(
                request: new GetPaymentsForCategoryQuery(categoryId: 0, dateRangeFrom: DateTime.Now.AddDays(-1), dateRangeTo: DateTime.Now.AddDays(1)),
                cancellationToken: default);

            // Assert
            result.Should().ContainSingle();
        }
    }

}
