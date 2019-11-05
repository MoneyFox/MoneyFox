using System;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Application.Payments.Commands.CreatePayment;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using MoneyFox.Presentation.ViewModels;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommandTests : IDisposable
    {
        private readonly EfCoreContext context;

        public CreatePaymentCommandTests()
        {
            context = SQLiteEfCoreContextFactory.Create();
        }

        public void Dispose()
        {
            SQLiteEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task CreatePayment_PaymentSaved()
        {
            // Arrange
            var account = new Account("test", 80); 
            context.Add(account);

            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, account);

            // Act
            await new CreatePaymentCommand.Handler(context).Handle(new CreatePaymentCommand(payment1), default);

            // Assert
            Assert.Single(context.Payments);
            (await context.Payments.FindAsync(payment1.Id)).ShouldNotBeNull();
        }

        [Theory]
        [InlineData(PaymentType.Expense, 60)]
        [InlineData(PaymentType.Income, 100)]
        public async Task CreatePayment_AccountCurrentBalanceUpdated(PaymentType paymentType, decimal newCurrentBalance)
        {
            // Arrange
            var account = new Account("test", 80); 
            context.Add(account);
            await context.SaveChangesAsync();

            var payment1 = new Payment(DateTime.Now, 20, paymentType, account);

            // Act
            await new CreatePaymentCommand.Handler(context).Handle(new CreatePaymentCommand(payment1), default);

            // Assert
            var loadedAccount = await context.Accounts.FindAsync(account.Id);
            loadedAccount.ShouldNotBeNull();
            loadedAccount.CurrentBalance.ShouldEqual(newCurrentBalance);
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
