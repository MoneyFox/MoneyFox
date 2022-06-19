namespace MoneyFox.Tests.Core.ApplicationCore.Queries.Payments.GetPaymentById
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.ApplicationCore.Domain.Exceptions;
    using MoneyFox.Core.ApplicationCore.Queries;
    using MoneyFox.Infrastructure.Persistence;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetPaymentByIdQueryTests
    {
        private readonly AppDbContext context;
        private readonly GetPaymentByIdQuery.Handler handler;

        public GetPaymentByIdQueryTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            handler = new GetPaymentByIdQuery.Handler(context);
        }

        [Fact]
        public async Task GetCategory_CategoryNotFound()
        {
            // Act / Assert
            // Arrange
            await Assert.ThrowsAsync<PaymentNotFoundException>(
                async () => await handler.Handle(request: new GetPaymentByIdQuery(999), cancellationToken: default));
        }

        [Fact]
        public async Task GetCategory_CategoryFound()
        {
            // Arrange
            var payment1 = new Payment(
                date: DateTime.Now,
                amount: 20,
                type: PaymentType.Expense,
                chargedAccount: new Account(name: "test", initialBalance: 80));

            await context.AddAsync(payment1);
            await context.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request: new GetPaymentByIdQuery(payment1.Id), cancellationToken: default);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(payment1.Id);
        }
    }

}
