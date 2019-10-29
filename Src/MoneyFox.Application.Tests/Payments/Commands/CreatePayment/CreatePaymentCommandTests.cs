using System;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Application.Payments.Commands.CreatePayment;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommandTests : IDisposable
    {
        private readonly EfCoreContext context;

        public CreatePaymentCommandTests()
        {
            context = InMemoryEfCoreContextFactory.Create();
        }

        public void Dispose()
        {
            InMemoryEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task CreatePayment_PaymentSaved()
        {
            // Arrange
            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, new Account("test", 80));

            // Act
            Unit result = await new CreatePaymentCommand.Handler(context).Handle(new CreatePaymentCommand(payment1), default);

            // Assert
            Assert.Single(context.Payments);
            (await context.Payments.FindAsync(payment1.Id)).ShouldNotBeNull();
        }

        [Fact]
        public async Task CreatePaymentWithRecurring_PaymentSaved()
        {
            // Arrange
            var account = new Account("test", 80);
            context.Add(account);

            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, account);

            payment1.AddRecurringPayment(PaymentRecurrence.Monthly);

            // Act
            Unit result = await new CreatePaymentCommand.Handler(context).Handle(new CreatePaymentCommand(payment1), default);

            // Assert
            Assert.Single(context.Payments);
            Assert.Single(context.RecurringPayments);
            (await context.Payments.FindAsync(payment1.Id)).ShouldNotBeNull();
            (await context.RecurringPayments.FindAsync(payment1.RecurringPayment.Id)).ShouldNotBeNull();
        }
    }
}
