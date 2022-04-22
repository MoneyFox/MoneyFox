namespace MoneyFox.Core.Tests.Commands.Payments.DeletePaymentById
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using Core._Pending_.Exceptions;
    using Core.Aggregates;
    using Core.Aggregates.AccountAggregate;
    using Core.Commands.Payments.DeletePaymentById;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class DeletePaymentByIdCommandTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public DeletePaymentByIdCommandTests()
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
        public async Task ThrowExceptionWhenPaymentNotFound()
        {
            // Act / Assert
            // Arrange
            await Assert.ThrowsAsync<PaymentNotFoundException>(
                async () => await new DeletePaymentByIdCommand.Handler(contextAdapterMock.Object).Handle(
                    request: new DeletePaymentByIdCommand(12),
                    cancellationToken: default));
        }

        [Fact]
        public async Task DeletePayment_PaymentDeleted()
        {
            // Arrange
            var payment1 = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: new Account(name: "test", initalBalance: 80));
            await context.AddAsync(payment1);
            await context.SaveChangesAsync();

            // Act
            await new DeletePaymentByIdCommand.Handler(contextAdapterMock.Object).Handle(
                request: new DeletePaymentByIdCommand(payment1.Id),
                cancellationToken: default);

            // Assert
            Assert.Empty(context.Payments);
        }
    }

}
