using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Payments.Commands.UpdatePayment;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Payments.Commands.UpdatePaymentById
{
    [ExcludeFromCodeCoverage]
    public class UpdatePaymentCommandTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public UpdatePaymentCommandTests()
        {
            context = InMemoryEfCoreContextFactory.Create();

            contextAdapterMock = new Mock<IContextAdapter>();
            contextAdapterMock.SetupGet(x => x.Context).Returns(context);
        }

        public void Dispose()
        {
            InMemoryEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task UpdatePayment_PaymentFound()
        {
            // Arrange
            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, new Account("test", 80));
            await context.AddAsync(payment1);
            await context.SaveChangesAsync();

            payment1.UpdatePayment(payment1.Date, 100, payment1.Type, payment1.ChargedAccount);

            // Act
            await new UpdatePaymentCommand.Handler(contextAdapterMock.Object).Handle(new UpdatePaymentCommand(payment1), default);

            // Assert
            (await context.Payments.FindAsync(payment1.Id)).Amount.ShouldEqual(payment1.Amount);
        }
    }
}
