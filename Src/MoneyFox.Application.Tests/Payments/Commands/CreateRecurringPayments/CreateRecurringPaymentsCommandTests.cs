using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Payments.Commands.CreateRecurringPayments;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using Should;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Application.Tests.Payments.Commands.CreateRecurringPayments
{
    [ExcludeFromCodeCoverage]
    public class CreateRecurringPaymentsCommandTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public CreateRecurringPaymentsCommandTests()
        {
            context = InMemoryEfCoreContextFactory.Create();

            contextAdapterMock = new Mock<IContextAdapter>();
            contextAdapterMock.SetupGet(x => x.Context).Returns(context);
        }

        public void Dispose()
        {
            SQLiteEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task PaymentsClearedAndSaved()
        {
            // Arrange
            var payment = new Payment(DateTime.Now.AddDays(-1), 166, PaymentType.Expense, new Account("Foo"));
            payment.AddRecurringPayment(PaymentRecurrence.Daily);

            context.AddRange(payment);
            await context.SaveChangesAsync();

            // Act
            await new CreateRecurringPaymentsCommand.Handler(contextAdapterMock.Object).Handle(new CreateRecurringPaymentsCommand(), default);
            var loadedPayments = context.Payments.ToList();

            // Assert
            loadedPayments.Count.ShouldEqual(2);
            loadedPayments.ForEach(x => x.Amount.ShouldEqual(166));
        }
    }
}
