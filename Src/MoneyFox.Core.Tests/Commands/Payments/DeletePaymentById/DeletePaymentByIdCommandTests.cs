namespace MoneyFox.Core.Tests.Commands.Payments.DeletePaymentById
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.ApplicationCore.Domain.Exceptions;
    using MoneyFox.Core.Commands.Payments.DeletePaymentById;
    using MoneyFox.Infrastructure.Persistence;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class DeletePaymentByIdCommandTests
    {
        private readonly AppDbContext context;
        private readonly DeletePaymentByIdCommand.Handler handler;

        public DeletePaymentByIdCommandTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            handler = new DeletePaymentByIdCommand.Handler(context);
        }

        [Fact]
        public async Task ThrowExceptionWhenPaymentNotFound()
        {
            // Act / Assert
            // Arrange
            await Assert.ThrowsAsync<PaymentNotFoundException>(
                async () => await handler.Handle(request: new DeletePaymentByIdCommand(12), cancellationToken: default));
        }

        [Fact]
        public async Task DeletePayment_PaymentDeleted()
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
            await handler.Handle(request: new DeletePaymentByIdCommand(payment1.Id), cancellationToken: default);

            // Assert
            Assert.Empty(context.Payments);
        }
    }

}
