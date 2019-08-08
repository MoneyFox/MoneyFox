using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MoneyFox.BusinessDbAccess.StatisticDataProvider;
using MoneyFox.BusinessLogic.StatisticDataProvider;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Foundation;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.BusinessLogic.Tests.StatisticDataProvider
{
    [ExcludeFromCodeCoverage]
    public class CategorySpreadingProviderTests
    {
        [Fact]
        public async Task GetValues_NullDependency_NullReferenceException()
        {
            await Assert.ThrowsAsync<NullReferenceException>(
                () => new CategorySpreadingDataProvider(null).GetValues(DateTime.Today, DateTime.Today));
        }

        [Fact]
        public async Task GetValues_CorrectSums()
        {
            // Arrange
            var testCat1 = new Category("Ausgehen");
            var testCat2 = new Category("Rent");
            var testCat3 = new Category("Food");

            var account = new Account("test");

            var paymentList = new List<Payment>
            {
                new Payment(DateTime.Today, 60, PaymentType.Income, account, category:testCat1),
                new Payment(DateTime.Today, 90, PaymentType.Expense, account, category:testCat1),
                new Payment(DateTime.Today, 10, PaymentType.Expense, account, category:testCat3),
                new Payment(DateTime.Today, 90, PaymentType.Expense, account, category:testCat2)
            };

            var statisticDbAccess = new Mock<IStatisticDbAccess>();
            statisticDbAccess.Setup(x => x.GetPaymentsWithoutTransfer(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(paymentList));

            // Act
            var provider = new CategorySpreadingDataProvider(statisticDbAccess.Object);
            var result =  (await provider.GetValues(DateTime.Today.AddDays(-3), DateTime.Today.AddDays(3))).ToList();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(90, result[0].Value);
            Assert.Equal(30, result[1].Value);
            Assert.Equal(10, result[2].Value);
        }

        [Theory]
        [InlineData("en-US", '$')]
        [InlineData("de-CH", 'C')]
        public async Task GetValues_CorrectCurrency(string culture, char expectedCurrencySymbol)
        {
            // Arrange
            var cultureInfo = new CultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            var testCat2 = new Category("Rent");

            var account = new Account("test");

            var paymentList = new List<Payment>
            {
                new Payment(DateTime.Today, 90, PaymentType.Expense, account, category:testCat2)
            };

            var statisticDbAccess = new Mock<IStatisticDbAccess>();
            statisticDbAccess.Setup(x => x.GetPaymentsWithoutTransfer(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                             .Returns(Task.FromResult(paymentList));

            // Act
            var provider = new CategorySpreadingDataProvider(statisticDbAccess.Object);
            var result = (await provider.GetValues(DateTime.Today.AddDays(-3), DateTime.Today.AddDays(3))).ToList();

            // Assert
            result[0].ValueLabel[0].ShouldEqual(expectedCurrencySymbol);
        }
    }
}