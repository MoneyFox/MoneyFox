namespace MoneyFox.Core.Tests.Commands.Payments.CreatePayment
{
    using Common.Interfaces;
    using Core.Aggregates;
    using Core.Aggregates.Payments;
    using Core.Commands.Payments.CreatePayment;
    using FluentAssertions;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class CreatePaymentCommandTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public CreatePaymentCommandTests()
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
        public async Task CreatePayment_PaymentSaved()
        {
            // Arrange
            var account = new Account("test", 80);
            context.Add(account);
            context.SaveChanges();

            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, account);

            // Act
            await new CreatePaymentCommand.Handler(
                    contextAdapterMock.Object)
                .Handle(new CreatePaymentCommand(payment1), default);

            // Assert
            Assert.Single(context.Payments);
            (await context.Payments.FindAsync(payment1.Id)).Should().NotBeNull();
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
            await new CreatePaymentCommand.Handler(
                contextAdapterMock.Object).Handle(new CreatePaymentCommand(payment1), default);

            // Assert
            Account loadedAccount = await context.Accounts.FindAsync(account.Id);
            loadedAccount.Should().NotBeNull();
            loadedAccount.CurrentBalance.Should().Be(newCurrentBalance);
        }

        [Fact]
        public async Task CreatePaymentWithRecurring_PaymentSaved()
        {
            // Arrange
            var account = new Account("test", 80);
            context.Add(account);
            context.SaveChanges();

            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, account);

            payment1.AddRecurringPayment(PaymentRecurrence.Monthly);

            // Act
            await new CreatePaymentCommand.Handler(
                contextAdapterMock.Object).Handle(new CreatePaymentCommand(payment1), default);

            // Assert
            Assert.Single(context.Payments);
            Assert.Single(context.RecurringPayments);
            (await context.Payments.FindAsync(payment1.Id)).Should().NotBeNull();
            (await context.RecurringPayments.FindAsync(payment1.RecurringPayment.Id)).Should().NotBeNull();
        }
    }
}