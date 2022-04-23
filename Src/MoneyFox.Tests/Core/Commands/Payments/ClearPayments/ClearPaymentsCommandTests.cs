namespace MoneyFox.Tests.Core.Commands.Payments.ClearPayments
{

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.Commands.Payments.ClearPayments;
    using MoneyFox.Core.Common.Interfaces;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class ClearPaymentsCommandTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public ClearPaymentsCommandTests()
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

        protected virtual void Dispose(bool disposing)
        {
            InMemoryAppDbContextFactory.Destroy(context);
        }

        [Fact]
        public async Task PaymentsClearedCorrectly()
        {
            // Arrange
            var paymentList = new List<Payment>
            {
                new Payment(date: DateTime.Now.AddDays(1), amount: 100, type: PaymentType.Expense, chargedAccount: new Account("Foo")),
                new Payment(date: DateTime.Now, amount: 100, type: PaymentType.Expense, chargedAccount: new Account("Foo")),
                new Payment(date: DateTime.Now.AddDays(-1), amount: 100, type: PaymentType.Expense, chargedAccount: new Account("Foo"))
            };

            context.AddRange(paymentList);
            await context.SaveChangesAsync();

            // Act
            await new ClearPaymentsCommand.Handler(contextAdapterMock.Object).Handle(request: new ClearPaymentsCommand(), cancellationToken: default);

            // Assert
            paymentList[0].IsCleared.Should().BeFalse();
            paymentList[1].IsCleared.Should().BeTrue();
            paymentList[2].IsCleared.Should().BeTrue();
        }

        [Fact]
        public async Task PaymentsClearedAndSaved()
        {
            // Arrange
            var paymentList = new List<Payment>
            {
                new Payment(date: DateTime.Now.AddDays(1), amount: 100, type: PaymentType.Expense, chargedAccount: new Account("Foo")),
                new Payment(date: DateTime.Now, amount: 100, type: PaymentType.Expense, chargedAccount: new Account("Foo")),
                new Payment(date: DateTime.Now.AddDays(-1), amount: 100, type: PaymentType.Expense, chargedAccount: new Account("Foo"))
            };

            context.AddRange(paymentList);
            await context.SaveChangesAsync();

            // Act
            await new ClearPaymentsCommand.Handler(contextAdapterMock.Object).Handle(request: new ClearPaymentsCommand(), cancellationToken: default);
            var loadedPayments = context.Payments.ToList();

            // Assert
            loadedPayments[0].IsCleared.Should().BeFalse();
            loadedPayments[1].IsCleared.Should().BeTrue();
            loadedPayments[2].IsCleared.Should().BeTrue();
        }
    }

}
