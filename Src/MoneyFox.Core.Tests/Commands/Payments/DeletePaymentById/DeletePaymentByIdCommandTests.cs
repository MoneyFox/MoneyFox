using MoneyFox.Core._Pending_.Common;
using MoneyFox.Core._Pending_.Common.Facades;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core._Pending_.DbBackup;
using MoneyFox.Core._Pending_.Exceptions;
using MoneyFox.Core.Aggregates;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Core.Commands.Payments.DeletePaymentById;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.Tests.Infrastructure;
using MoneyFox.Infrastructure.Persistence;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Core.Tests.Commands.Payments.DeletePaymentById
{
    [ExcludeFromCodeCoverage]
    public class DeletePaymentByIdCommandTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public DeletePaymentByIdCommandTests()
        {
            context = InMemoryEfCoreContextFactory.Create();

            contextAdapterMock = new Mock<IContextAdapter>();
            contextAdapterMock.SetupGet(x => x.Context).Returns(context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) => InMemoryEfCoreContextFactory.Destroy(context);

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