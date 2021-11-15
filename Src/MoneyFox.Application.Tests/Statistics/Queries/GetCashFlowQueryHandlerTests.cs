﻿using FluentAssertions;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.Application.Statistics;
using MoneyFox.Application.Statistics.Queries;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Infrastructure.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Application.Tests.Statistics.Queries
{
    [ExcludeFromCodeCoverage]
    [Collection("CultureCollection")]
    public class GetCashFlowQueryHandlerTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetCashFlowQueryHandlerTests()
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

        protected virtual void Dispose(bool disposing) => InMemoryEfCoreContextFactory.Destroy(context);

        [Fact]
        public async Task GetValues_CorrectSums()
        {
            // Arrange
            context.AddRange(
                new List<Payment>
                {
                    new Payment(DateTime.Today, 60, PaymentType.Income, new Account("Foo1")),
                    new Payment(DateTime.Today, 20, PaymentType.Income, new Account("Foo2")),
                    new Payment(DateTime.Today, 50, PaymentType.Expense, new Account("Foo3")),
                    new Payment(DateTime.Today, 40, PaymentType.Expense, new Account("Foo3"))
                });
            context.SaveChanges();

            // Act
            List<StatisticEntry> result = await new GetCashFlowQueryHandler(contextAdapterMock.Object).Handle(
                new GetCashFlowQuery {StartDate = DateTime.Today.AddDays(-3), EndDate = DateTime.Today.AddDays(3)},
                default);

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
            List<StatisticEntry> result = await new GetCashFlowQueryHandler(contextAdapterMock.Object).Handle(
                new GetCashFlowQuery {StartDate = DateTime.Today.AddDays(-3), EndDate = DateTime.Today.AddDays(3)},
                default);

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
            List<StatisticEntry> result = await new GetCashFlowQueryHandler(contextAdapterMock.Object).Handle(
                new GetCashFlowQuery {StartDate = DateTime.Today.AddDays(-3), EndDate = DateTime.Today.AddDays(3)},
                default);

            // Assert
            result[0].Label.Should().Be(Strings.RevenueLabel);
            result[1].Label.Should().Be(Strings.ExpenseLabel);
            result[2].Label.Should().Be(Strings.IncreaseLabel);
        }

        [Theory]
        [InlineData("de-CH", 3, '-')]
        public async Task GetValues_CorrectNegativeSign(string culture,
            int indexNegativeSign,
            char expectedNegativeSign)
        {
            // Arrange
            var cultureInfo = new CultureInfo(culture);
            CultureHelper.CurrentCulture = cultureInfo;

            context.AddRange(
                new List<Payment> {new Payment(DateTime.Today, 40, PaymentType.Expense, new Account("Foo3"))});
            context.SaveChanges();

            // Act
            List<StatisticEntry> result = await new GetCashFlowQueryHandler(contextAdapterMock.Object).Handle(
                new GetCashFlowQuery {StartDate = DateTime.Today.AddDays(-3), EndDate = DateTime.Today.AddDays(3)},
                default);

            // Assert
            result[2].ValueLabel[indexNegativeSign].Should().Be(expectedNegativeSign);

            CultureHelper.CurrentCulture = CultureInfo.CurrentCulture;
        }

        [Fact]
        public async Task GetValues_USVersion_CorrectNegativeSign()
        {
            // Arrange
            context.AddRange(
                new List<Payment> {new Payment(DateTime.Today, 40, PaymentType.Expense, new Account("Foo3"))});
            context.SaveChanges();

            var cultureInfo = new CultureInfo("en-US");
            CultureHelper.CurrentCulture = cultureInfo;

            // Act
            List<StatisticEntry> result = await new GetCashFlowQueryHandler(contextAdapterMock.Object).Handle(
                new GetCashFlowQuery {StartDate = DateTime.Today.AddDays(-3), EndDate = DateTime.Today.AddDays(3)},
                default);

            // Assert
            // We have to test here for Mac since they have a different standard sign than Windows.
            result[2]
                .ValueLabel[0]
                .Should()
                .Be(
                    RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                        ? '-'
                        : '(');

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
            List<StatisticEntry> result = await new GetCashFlowQueryHandler(contextAdapterMock.Object).Handle(
                new GetCashFlowQuery {StartDate = DateTime.Today.AddDays(-3), EndDate = DateTime.Today.AddDays(3)},
                default);

            // Assert
            result[0].ValueLabel[0].Should().Be(expectedCurrencySymbol);
            result[1].ValueLabel[0].Should().Be(expectedCurrencySymbol);
            result[2].ValueLabel[0].Should().Be(expectedCurrencySymbol);
        }
    }
}