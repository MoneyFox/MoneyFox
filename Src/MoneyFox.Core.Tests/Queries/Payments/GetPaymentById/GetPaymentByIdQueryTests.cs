namespace MoneyFox.Core.Tests.Queries.Payments.GetPaymentById
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using ApplicationCore.Domain.Aggregates.AccountAggregate;
    using ApplicationCore.Domain.Exceptions;
    using ApplicationCore.Queries;
    using Common.Interfaces;
    using FluentAssertions;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetPaymentByIdQueryTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetPaymentByIdQueryTests()
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
        public async Task GetCategory_CategoryNotFound()
        {
            // Act / Assert
            // Arrange
            await Assert.ThrowsAsync<PaymentNotFoundException>(
                async () => await new GetPaymentByIdQuery.Handler(contextAdapterMock.Object).Handle(
                    request: new GetPaymentByIdQuery(999),
                    cancellationToken: default));
        }

        [Fact]
        public async Task GetCategory_CategoryFound()
        {
            // Arrange
            var payment1 = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: new Account(name: "test", initalBalance: 80));
            await context.AddAsync(payment1);
            await context.SaveChangesAsync();

            // Act
            var result = await new GetPaymentByIdQuery.Handler(contextAdapterMock.Object).Handle(
                request: new GetPaymentByIdQuery(payment1.Id),
                cancellationToken: default);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(payment1.Id);
        }
    }

}
