namespace MoneyFox.Core.Tests.Queries.Statistics
{
    using Common.Interfaces;
    using Core.Aggregates;
    using Core.Aggregates.Payments;
    using Core.Queries.Statistics;
    using FluentAssertions;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("CultureCollection")]
    public class GetCategoryProgressionHandlerTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetCategoryProgressionHandlerTests()
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
        public async Task CalculateCorrectSums()
        {
            // Arrange
            var account = new Account("Foo1");
            var category = new Category("abcd");
            context.AddRange(
                new List<Payment>
                {
                    new Payment(DateTime.Today, 60, PaymentType.Income, account, category: category),
                    new Payment(DateTime.Today, 20, PaymentType.Expense, account, category: category),
                    new Payment(DateTime.Today.AddMonths(-1), 50, PaymentType.Expense, account, category: category),
                    new Payment(DateTime.Today.AddMonths(-2), 40, PaymentType.Expense, account, category: category)
                });
            context.Add(account);
            context.Add(category);
            context.SaveChanges();

            // Act
            IImmutableList<StatisticEntry> result =
                await new GetCategoryProgressionHandler(contextAdapterMock.Object).Handle(
                    new GetCategoryProgressionQuery(
                        category.Id,
                        DateTime.Today.AddYears(-1),
                        DateTime.Today.AddDays(3)),
                    default);

            // Assert
            result[12].Value.Should().Be(40);
            result[11].Value.Should().Be(-50);
            result[10].Value.Should().Be(-40);
        }

        [Fact]
        public async Task GetValues_CorrectSums()
        {
            // Arrange
            var account = new Account("Foo1");
            var category = new Category("abcd");
            context.AddRange(
                new List<Payment>
                {
                    new Payment(DateTime.Today, 60, PaymentType.Income, account, category: category),
                    new Payment(DateTime.Today, 20, PaymentType.Expense, account, category: category),
                    new Payment(DateTime.Today.AddMonths(-1), 50, PaymentType.Expense, account, category: category),
                    new Payment(DateTime.Today.AddMonths(-2), 40, PaymentType.Expense, account, category: category)
                });
            context.Add(account);
            context.Add(category);

            context.SaveChanges();

            // Act
            List<StatisticEntry> result = await new GetAccountProgressionHandler(contextAdapterMock.Object).Handle(
                new GetAccountProgressionQuery(
                    account.Id,
                    DateTime.Today.AddYears(-1),
                    DateTime.Today.AddDays(3)),
                default);

            // Assert
            result[0].Color.Should().Be("#87cefa");
            result[1].Color.Should().Be("#cd3700");
            result[2].Color.Should().Be("#cd3700");
        }
    }
}