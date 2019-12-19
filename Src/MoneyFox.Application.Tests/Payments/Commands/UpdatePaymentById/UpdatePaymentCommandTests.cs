using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Payments.Commands.UpdatePayment;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using Should;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
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
            await new UpdatePaymentCommand.Handler(contextAdapterMock.Object)
                .Handle(new UpdatePaymentCommand(payment1.Id,
                                                 payment1.Date,
                                                 payment1.Amount,
                                                 payment1.IsCleared,
                                                 payment1.Type,
                                                 payment1.Note,
                                                 payment1.IsRecurring,
                                                 payment1.Category != null
                                                     ? payment1.Category.Id
                                                     : 0,
                                                 payment1.ChargedAccount != null
                                                     ? payment1.ChargedAccount.Id
                                                     : 0,
                                                 payment1.TargetAccount != null
                                                     ? payment1.TargetAccount.Id
                                                     : 0,
                                                 false),
                        default);

            // Assert
            (await context.Payments.FindAsync(payment1.Id)).Amount.ShouldEqual(payment1.Amount);
        }
    }
}
