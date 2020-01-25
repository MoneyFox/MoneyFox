using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Payments.Commands.ClearPayments;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Payments.Commands.ClearPayments
{
    [ExcludeFromCodeCoverage]
    public class ClearPaymentsCommandTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public ClearPaymentsCommandTests()
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
        public async Task PaymentsClearedCorrectly()
        {
            // Arrange
            var paymentList = new List<Payment>
            {
                new Payment(DateTime.Now.AddDays(1), 100, PaymentType.Expense, new Account("Foo")),
                new Payment(DateTime.Now, 100, PaymentType.Expense, new Account("Foo")),
                new Payment(DateTime.Now.AddDays(-1), 100, PaymentType.Expense, new Account("Foo"))
            };

            context.AddRange(paymentList);
            await context.SaveChangesAsync();

            // Act
            await new ClearPaymentsCommand.Handler(contextAdapterMock.Object).Handle(new ClearPaymentsCommand(), default);

            // Assert
            paymentList[0].IsCleared.ShouldBeFalse();
            paymentList[1].IsCleared.ShouldBeTrue();
            paymentList[2].IsCleared.ShouldBeTrue();
        }

        [Fact]
        public async Task PaymentsClearedAndSaved()
        {
            // Arrange
            var paymentList = new List<Payment>
            {
                new Payment(DateTime.Now.AddDays(1), 100, PaymentType.Expense, new Account("Foo")),
                new Payment(DateTime.Now, 100, PaymentType.Expense, new Account("Foo")),
                new Payment(DateTime.Now.AddDays(-1), 100, PaymentType.Expense, new Account("Foo"))
            };

            context.AddRange(paymentList);
            await context.SaveChangesAsync();

            // Act
            await new ClearPaymentsCommand.Handler(contextAdapterMock.Object).Handle(new ClearPaymentsCommand(), default);
            List<Payment> loadedPayments = context.Payments.ToList();

            // Assert
            loadedPayments[0].IsCleared.ShouldBeFalse();
            loadedPayments[1].IsCleared.ShouldBeTrue();
            loadedPayments[2].IsCleared.ShouldBeTrue();
        }
    }
}
