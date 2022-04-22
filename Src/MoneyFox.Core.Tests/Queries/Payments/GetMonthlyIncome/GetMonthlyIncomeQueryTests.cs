namespace MoneyFox.Core.Tests.Queries.Payments.GetMonthlyIncome
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using ApplicationCore.Domain.Aggregates.AccountAggregate;
    using ApplicationCore.Queries;
    using Common;
    using Common.Interfaces;
    using Core._Pending_;
    using FluentAssertions;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using NSubstitute;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetMonthlyIncomeQueryTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly IContextAdapter contextAdapter;

        public GetMonthlyIncomeQueryTests()
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

        protected virtual void Dispose(bool disposing)
        {
            InMemoryAppDbContextFactory.Destroy(context);
        }

        [Fact]
        public async Task ReturnCorrectAmount()
        {
            // Arrange
            var systemDateHelper = Substitute.For<ISystemDateHelper>();
            systemDateHelper.Today.Returns(new DateTime(year: 2020, month: 09, day: 05));
            var account = new Account(name: "test", initalBalance: 80);
            var payment1 = new Payment(date: new DateTime(year: 2020, month: 09, day: 10), amount: 50, type: PaymentType.Income, chargedAccount: account);
            var payment2 = new Payment(date: new DateTime(year: 2020, month: 09, day: 18), amount: 20, type: PaymentType.Income, chargedAccount: account);
            var payment3 = new Payment(date: new DateTime(year: 2020, month: 09, day: 4), amount: 30, type: PaymentType.Expense, chargedAccount: account);
            await context.AddAsync(payment1);
            await context.AddAsync(payment2);
            await context.AddAsync(payment3);
            await context.SaveChangesAsync();

            // Act
            var sum = await new GetMonthlyIncomeQuery.Handler(contextAdapter: contextAdapter, systemDateHelper: systemDateHelper).Handle(
                request: new GetMonthlyIncomeQuery(),
                cancellationToken: default);

            // Assert
            sum.Should().Be(70);
        }
    }

}
