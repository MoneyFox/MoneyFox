namespace MoneyFox.Core.Tests.Queries.Payments.GetPaymentsForAccountId
{
    using Core._Pending_.Common.Interfaces;
    using Core.Aggregates;
    using Core.Aggregates.Payments;
    using Core.Queries.Payments.GetPaymentsForAccountId;
    using FluentAssertions;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetPaymentsForAccountIdQueryTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetPaymentsForAccountIdQueryTests()
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
        public async Task GetPaymentsForAccountId_CorrectAccountId_PaymentFound()
        {
            // Arrange
            var account = new Account("test", 80);

            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, account);
            var payment2 = new Payment(DateTime.Now, 20, PaymentType.Expense, new Account("test", 80));
            await context.AddAsync(payment1);
            await context.AddAsync(payment2);
            await context.SaveChangesAsync();

            // Act
            List<Payment> result = await new GetPaymentsForAccountIdQuery.Handler(contextAdapterMock.Object).Handle(
                new GetPaymentsForAccountIdQuery(account.Id, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1)),
                default);

            // Assert
            result.First().Id.Should().Be(payment1.Id);
        }

        [Fact]
        public async Task GetPaymentsForAccountId_CorrectDateRange_PaymentFound()
        {
            // Arrange
            var account = new Account("test", 80);

            var payment1 = new Payment(DateTime.Now.AddDays(-2), 20, PaymentType.Expense, account);
            var payment2 = new Payment(DateTime.Now, 20, PaymentType.Expense, account);
            await context.AddAsync(payment1);
            await context.AddAsync(payment2);
            await context.SaveChangesAsync();

            // Act
            List<Payment> result = await new GetPaymentsForAccountIdQuery.Handler(contextAdapterMock.Object).Handle(
                new GetPaymentsForAccountIdQuery(account.Id, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1)),
                default);

            // Assert
            result.First().Id.Should().Be(payment2.Id);
        }

        [Theory]
        [InlineData(PaymentTypeFilter.All)]
        [InlineData(PaymentTypeFilter.Expense)]
        [InlineData(PaymentTypeFilter.Income)]
        [InlineData(PaymentTypeFilter.Transfer)]
        public async Task GetPaymentsForAccountId_CorrectPaymentType_PaymentFound(PaymentTypeFilter filteredPaymentType)
        {
            // Arrange
            var account = new Account("test", 80);
            var accountxfer = new Account("dest", 80);

            var payment1 = new Payment(DateTime.Now, 10, PaymentType.Expense, account);
            var payment2 = new Payment(DateTime.Now, 20, PaymentType.Income, account);
            var payment3 = new Payment(DateTime.Now, 30, PaymentType.Transfer, account, accountxfer);
            await context.AddAsync(payment1);
            await context.AddAsync(payment2);
            await context.AddAsync(payment3);
            await context.SaveChangesAsync();

            // Act
            List<Payment> result = await new GetPaymentsForAccountIdQuery.Handler(contextAdapterMock.Object).Handle(
                new GetPaymentsForAccountIdQuery(account.Id, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), default, default, filteredPaymentType),
                default);

            var expectedamount = filteredPaymentType switch
            {
                PaymentTypeFilter.Expense => 10,
                PaymentTypeFilter.Income => 20,
                PaymentTypeFilter.Transfer => 30,
                _ => 0
            };

            // Assert
            Assert.Equal(result.Count, filteredPaymentType == PaymentTypeFilter.All ? 3 : 1);

            if(filteredPaymentType != PaymentTypeFilter.All)
            {
                result.First().Amount.Should().Be(expectedamount);
            }
        }
    }
}