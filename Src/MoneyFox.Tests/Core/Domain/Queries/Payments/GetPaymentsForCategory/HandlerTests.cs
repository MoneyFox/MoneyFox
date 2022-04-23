namespace MoneyFox.Tests.Core.Domain.Queries.Payments.GetPaymentsForCategory
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
    using MoneyFox.Core.ApplicationCore.Queries;
    using MoneyFox.Core.Common.Interfaces;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class HandlerTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public HandlerTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            contextAdapterMock = new Mock<IContextAdapter>();
            contextAdapterMock.SetupGet(x => x.Context).Returns(context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            InMemoryAppDbContextFactory.Destroy(context);
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
            var result = await new GetPaymentsForCategoryQuery.Handler(contextAdapterMock.Object).Handle(
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
            var result = await new GetPaymentsForCategoryQuery.Handler(contextAdapterMock.Object).Handle(
                request: new GetPaymentsForCategoryQuery(categoryId: 0, dateRangeFrom: DateTime.Now.AddDays(-1), dateRangeTo: DateTime.Now.AddDays(1)),
                cancellationToken: default);

            // Assert
            result.Should().ContainSingle();
        }
    }

}
