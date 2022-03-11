namespace MoneyFox.Core.Tests.Queries.Payments.GetPaymentsForCategory
{
    using Common.Interfaces;
    using Core.Aggregates;
    using Core.Aggregates.Payments;
    using Core.Queries.Payments.GetPaymentsForCategory;
    using FluentAssertions;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
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

        protected virtual void Dispose(bool disposing) => InMemoryAppDbContextFactory.Destroy(context);

        [Fact]
        public async Task CorrectPaymentsSelected()
        {
            // Arrange
            var account = new Account("asdf");
            var category = new Category("Test");
            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, account);
            var payment2 = new Payment(DateTime.Now, 30, PaymentType.Expense, account, category: category);
            var payment3 = new Payment(DateTime.Now, 40, PaymentType.Expense, account, category: category);

            context.Add(payment1);
            context.Add(payment2);
            context.Add(payment3);
            await context.SaveChangesAsync();

            // Act
            List<Payment> result = await new GetPaymentsForCategoryQuery.Handler(contextAdapterMock.Object).Handle(
                new GetPaymentsForCategoryQuery(
                    category.Id,
                    DateTime.Now.AddDays(-1),
                    DateTime.Now.AddDays(1)),
                default);

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task CorrectPaymentsSelectedWithNoCategory()
        {
            // Arrange
            var account = new Account("asdf");
            var category = new Category("Test");
            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, account);
            var payment2 = new Payment(DateTime.Now, 30, PaymentType.Expense, account, category: category);
            var payment3 = new Payment(DateTime.Now, 40, PaymentType.Expense, account, category: category);

            context.Add(payment1);
            context.Add(payment2);
            context.Add(payment3);
            await context.SaveChangesAsync();

            // Act
            List<Payment> result = await new GetPaymentsForCategoryQuery.Handler(contextAdapterMock.Object).Handle(
                new GetPaymentsForCategoryQuery(
                    0,
                    DateTime.Now.AddDays(-1),
                    DateTime.Now.AddDays(1)),
                default);

            // Assert
            result.Should().ContainSingle();
        }
    }
}