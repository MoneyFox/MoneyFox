namespace MoneyFox.Tests.Core.ApplicationCore.Queries.Statistics
{

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.ApplicationCore.Queries.Statistics;
    using MoneyFox.Core.Common.Helpers;
    using MoneyFox.Core.Resources;
    using MoneyFox.Infrastructure.Persistence;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("CultureCollection")]
    public class GetCashFlowQueryHandlerTests
    {
        private readonly AppDbContext context;
        private readonly GetCashFlowQueryHandler getCashFlowQueryHandler;

        public GetCashFlowQueryHandlerTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            getCashFlowQueryHandler = new GetCashFlowQueryHandler(context);
        }

        [Fact]
        public async Task GetValues_CorrectSums()
        {
            // Arrange
            context.AddRange(
                new List<Payment>
                {
                    new Payment(date: DateTime.Today, amount: 60, type: PaymentType.Income, chargedAccount: new Account("Foo1")),
                    new Payment(date: DateTime.Today, amount: 20, type: PaymentType.Income, chargedAccount: new Account("Foo2")),
                    new Payment(date: DateTime.Today, amount: 50, type: PaymentType.Expense, chargedAccount: new Account("Foo3")),
                    new Payment(date: DateTime.Today, amount: 40, type: PaymentType.Expense, chargedAccount: new Account("Foo3"))
                });

            await context.SaveChangesAsync();

            // Act
            var result = await getCashFlowQueryHandler.Handle(
                request: new GetCashFlowQuery { StartDate = DateTime.Today.AddDays(-3), EndDate = DateTime.Today.AddDays(3) },
                cancellationToken: default);

            // Assert
            result[0].Value.Should().Be(80);
            result[1].Value.Should().Be(90);
            result[2].Value.Should().Be(-10);
        }

        [Fact]
        public async Task GetValues_CorrectColors()
        {
            // Arrange

            // Act
            var result = await getCashFlowQueryHandler.Handle(
                request: new GetCashFlowQuery { StartDate = DateTime.Today.AddDays(-3), EndDate = DateTime.Today.AddDays(3) },
                cancellationToken: default);

            // Assert
            result[0].Color.Should().Be("#9bcd9b");
            result[1].Color.Should().Be("#cd3700");
            result[2].Color.Should().Be("#87cefa");
        }

        [Fact]
        public async Task GetValues_CorrectLabels()
        {
            // Arrange

            // Act
            var result = await getCashFlowQueryHandler.Handle(
                request: new GetCashFlowQuery { StartDate = DateTime.Today.AddDays(-3), EndDate = DateTime.Today.AddDays(3) },
                cancellationToken: default);

            // Assert
            result[0].Label.Should().Be(Strings.RevenueLabel);
            result[1].Label.Should().Be(Strings.ExpenseLabel);
            result[2].Label.Should().Be(Strings.IncreaseLabel);
        }

        [Fact]
        public async Task GetValues_USVersion_CorrectNegativeSign()
        {
            // Arrange
            context.AddRange(
                new List<Payment> { new Payment(date: DateTime.Today, amount: 40, type: PaymentType.Expense, chargedAccount: new Account("Foo3")) });

            await context.SaveChangesAsync();
            var cultureInfo = new CultureInfo("en-US");
            CultureHelper.CurrentCulture = cultureInfo;

            // Act
            var result = await getCashFlowQueryHandler.Handle(
                request: new GetCashFlowQuery { StartDate = DateTime.Today.AddDays(-3), EndDate = DateTime.Today.AddDays(3) },
                cancellationToken: default);

            // Assert
            // We have to test here for Mac since they have a different standard sign than Windows.
            result[2].ValueLabel[0].Should().Be(RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? '-' : '(');
            CultureHelper.CurrentCulture = CultureInfo.CurrentCulture;
        }

        [Theory]
        [InlineData("en-US", '$')]
        [InlineData("de-CH", 'C')]
        public async Task GetValues_CorrectCurrency(string culture, char expectedCurrencySymbol)
        {
            // Arrange
            var cultureInfo = new CultureInfo(culture);
            CultureHelper.CurrentCulture = cultureInfo;

            // Act
            var result = await getCashFlowQueryHandler.Handle(
                request: new GetCashFlowQuery { StartDate = DateTime.Today.AddDays(-3), EndDate = DateTime.Today.AddDays(3) },
                cancellationToken: default);

            // Assert
            result[0].ValueLabel[0].Should().Be(expectedCurrencySymbol);
            result[1].ValueLabel[0].Should().Be(expectedCurrencySymbol);
            result[2].ValueLabel[0].Should().Be(expectedCurrencySymbol);
        }
    }

}
