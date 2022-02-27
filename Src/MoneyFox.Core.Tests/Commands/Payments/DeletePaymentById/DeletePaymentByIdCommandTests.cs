namespace MoneyFox.Core.Tests.Commands.Payments.DeletePaymentById
{
    using Core._Pending_.Common.Interfaces;
    using Core._Pending_.Exceptions;
    using Core.Aggregates;
    using Core.Aggregates.Payments;
    using Core.Commands.Payments.DeletePaymentById;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
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

        protected virtual void Dispose(bool disposing) => InMemoryAppDbContextFactory.Destroy(context);

        [Fact]
        public async Task ThrowExceptionWhenPaymentNotFound() =>
            // Arrange
            // Act / Assert
            await Assert.ThrowsAsync<PaymentNotFoundException>(
                async ()
                    => await new DeletePaymentByIdCommand.Handler(
                            contextAdapterMock.Object)
                        .Handle(new DeletePaymentByIdCommand(12), default));

        [Fact]
        public async Task DeletePayment_PaymentDeleted()
        {
            // Arrange
            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, new Account("test", 80));
            await context.AddAsync(payment1);
            await context.SaveChangesAsync();

            // Act
            await new DeletePaymentByIdCommand.Handler(
                    contextAdapterMock.Object)
                .Handle(new DeletePaymentByIdCommand(payment1.Id), default);

            // Assert
            Assert.Empty(context.Payments);
        }
    }
}