namespace MoneyFox.Tests.Core.Commands.Payments.CreateRecurringPayments
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.Commands.Payments.CreateRecurringPayments;
    using MoneyFox.Core.Common.Extensions;
    using MoneyFox.Infrastructure.Persistence;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class CreateRecurringPaymentsCommandTests
    {
        private readonly AppDbContext context;
        private readonly CreateRecurringPaymentsCommand.Handler handler;

        public CreateRecurringPaymentsCommandTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            handler = new CreateRecurringPaymentsCommand.Handler(context);
        }

        [Fact]
        public async Task PaymentsClearedAndSaved()
        {
            // Arrange
            var payment = new Payment(date: DateTime.Now.AddDays(-1), amount: 166, type: PaymentType.Expense, chargedAccount: new Account("Foo"));
            payment.AddRecurringPayment(recurrence: PaymentRecurrence.Daily, isLastDayOfMonth: false);
            context.AddRange(payment);
            await context.SaveChangesAsync();

            // Act
            await handler.Handle(request: new CreateRecurringPaymentsCommand(), cancellationToken: default);
            var loadedPayments = context.Payments.ToList();

            // Assert
            loadedPayments.Should().HaveCount(2);
            loadedPayments.ForEach(x => x.Amount.Should().Be(166));
        }

        [Theory]
        [InlineData(PaymentRecurrence.Monthly, 1, true)]
        [InlineData(PaymentRecurrence.Monthly, 1, false)]
        [InlineData(PaymentRecurrence.Bimonthly, 2, true)]
        [InlineData(PaymentRecurrence.Bimonthly, 2, false)]
        [InlineData(PaymentRecurrence.Quarterly, 3, true)]
        [InlineData(PaymentRecurrence.Quarterly, 3, false)]
        [InlineData(PaymentRecurrence.Biannually, 6, true)]
        [InlineData(PaymentRecurrence.Biannually, 6, false)]
        [InlineData(PaymentRecurrence.Yearly, 12, true)]
        [InlineData(PaymentRecurrence.Yearly, 12, false)]
        public async Task LastDayOfMonthValidation(PaymentRecurrence recurrence, int paymentDateMonthsBack, bool isLastDayOfMonth)
        {
            // Arrange
            var payment = new Payment(
                date: DateTime.Today.AddMonths(-paymentDateMonthsBack).GetFirstDayOfMonth(),
                amount: 333,
                type: PaymentType.Expense,
                chargedAccount: new Account("Foo"));

            payment.AddRecurringPayment(recurrence: recurrence, isLastDayOfMonth: isLastDayOfMonth);
            context.AddRange(payment);
            await context.SaveChangesAsync();

            // Act
            await handler.Handle(request: new CreateRecurringPaymentsCommand(), cancellationToken: default);
            var loadedPayments = context.Payments.ToList();

            // Assert
            loadedPayments.Should().HaveCount(2);
            loadedPayments.ForEach(x => x.Amount.Should().Be(333));
            loadedPayments[1].Date.Should().Be(isLastDayOfMonth ? DateTime.Today.GetLastDayOfMonth() : DateTime.Today.GetFirstDayOfMonth());
        }
    }

}
