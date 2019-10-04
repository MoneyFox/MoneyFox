using System;
using System.Threading.Tasks;
using MoneyFox.Application.Payments.Commands.DeletePaymentById;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Payments.Commands.DeletePaymentById
{
    public class DeletePaymentByIdCommandTests : IDisposable
    {
        private readonly EfCoreContext context;

        public DeletePaymentByIdCommandTests()
        {
            context = TestEfCoreContextFactory.Create();
        }

        public void Dispose()
        {
            TestEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetPayment_PaymentFound()
        {
            // Arrange
            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, new Account("test", 80));
            await context.AddAsync(payment1);
            await context.SaveChangesAsync();

            // Act
            await new DeletePaymentByIdCommand.Handler(context).Handle(new DeletePaymentByIdCommand(payment1.Id), default);

            // Assert
            Assert.Empty(context.Payments);
        }
    }
}
