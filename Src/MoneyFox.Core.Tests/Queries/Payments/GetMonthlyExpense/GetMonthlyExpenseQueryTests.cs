namespace MoneyFox.Core.Tests.Queries.Payments.GetMonthlyExpense
{
    using Common.Interfaces;
    using Core._Pending_;
    using Core.Aggregates;
    using Core.Aggregates.Payments;
    using Core.Queries.Payments.GetMonthlyExpense;
    using FluentAssertions;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using NSubstitute;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetMonthlyExpenseQueryTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly IContextAdapter contextAdapter;

        public GetMonthlyExpenseQueryTests()
        {
            context = InMemoryAppDbContextFactory.Create();

            contextAdapter = Substitute.For<IContextAdapter>();
            contextAdapter.Context.Returns(context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) => InMemoryAppDbContextFactory.Destroy(context);

        [Fact]
        public async Task ReturnCorrectAmount()
        {
            // Arrange

            var systemDateHelper = Substitute.For<ISystemDateHelper>();
            systemDateHelper.Today.Returns(new DateTime(2020, 09, 05));

            var account = new Account("test", 80);

            var payment1 = new Payment(new DateTime(2020, 09, 03), 50, PaymentType.Expense, account);
            var payment2 = new Payment(new DateTime(2020, 09, 04), 20, PaymentType.Expense, account);
            var payment3 = new Payment(new DateTime(2020, 09, 04), 30, PaymentType.Income, account);

            await context.AddAsync(payment1);
            await context.AddAsync(payment2);
            await context.AddAsync(payment3);
            await context.SaveChangesAsync();

            // Act
            decimal sum =
                await new GetMonthlyExpenseQuery.Handler(contextAdapter, systemDateHelper).Handle(
                    new GetMonthlyExpenseQuery(),
                    default);

            // Assert
            sum.Should().Be(70);
        }
    }
}