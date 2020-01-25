using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Statistics;
using MoneyFox.Application.Statistics.Queries.GetCategorySpreading;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Statistics.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetCategorySpreadingQueryTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetCategorySpreadingQueryTests()
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
        public async Task GetValues_CorrectSums()
        {
            // Arrange
            var testCat1 = new Category("Ausgehen");
            var testCat2 = new Category("Rent");
            var testCat3 = new Category("Food");

            var account = new Account("test");

            var paymentList = new List<Payment>
            {
                new Payment(DateTime.Today, 60, PaymentType.Income, account, category: testCat1),
                new Payment(DateTime.Today, 90, PaymentType.Expense, account, category: testCat1),
                new Payment(DateTime.Today, 10, PaymentType.Expense, account, category: testCat3),
                new Payment(DateTime.Today, 90, PaymentType.Expense, account, category: testCat2)
            };

            context.Payments.AddRange(paymentList);
            context.SaveChanges();

            // Act
            List<StatisticEntry> result =
                (await new GetCategorySpreadingQueryHandler(contextAdapterMock.Object).Handle(new GetCategorySpreadingQuery
                {
                    StartDate = DateTime.Today.AddDays(-3),
                    EndDate = DateTime.Today.AddDays(3)
                }, default))
               .ToList();

            // Assert
            result.Count.ShouldEqual(3);
            result[0].Value.ShouldEqual(90);
            result[1].Value.ShouldEqual(30);
            result[2].Value.ShouldEqual(10);
        }

        [Fact]
        public async Task GetValues_IgnoreSingleIncomes()
        {
            // Arrange
            var testCat1 = new Category("Ausgehen");
            var testCat2 = new Category("Rent");
            var testCat3 = new Category("Food");

            var account = new Account("test");
            var paymentList = new List<Payment>
            {
                new Payment(DateTime.Today, 60, PaymentType.Income, account, category: testCat1),
                new Payment(DateTime.Today, 90, PaymentType.Expense, account, category: testCat2),
                new Payment(DateTime.Today, 10, PaymentType.Expense, account, category: testCat3)
            };


            context.Payments.AddRange(paymentList);
            context.SaveChanges();

            // Act
            List<StatisticEntry> result =
                (await new GetCategorySpreadingQueryHandler(contextAdapterMock.Object).Handle(new GetCategorySpreadingQuery
                {
                    StartDate = DateTime.Today.AddDays(-3),
                    EndDate = DateTime.Today.AddDays(3)
                }, default))
               .ToList();

            // Assert
            result.Count.ShouldEqual(2);
        }

        [Fact]
        public async Task GetValues_CorrectLabel()
        {
            // Arrange
            var testCat1 = new Category("Ausgehen");
            var testCat2 = new Category("Rent");

            var account = new Account("test");
            var paymentList = new List<Payment>
            {
                new Payment(DateTime.Today, 90, PaymentType.Expense, account, category: testCat1),
                new Payment(DateTime.Today, 10, PaymentType.Expense, account, category: testCat2)
            };


            context.Payments.AddRange(paymentList);
            context.SaveChanges();

            // Act
            List<StatisticEntry> result =
                (await new GetCategorySpreadingQueryHandler(contextAdapterMock.Object).Handle(new GetCategorySpreadingQuery
                {
                    StartDate = DateTime.Today.AddDays(-3),
                    EndDate = DateTime.Today.AddDays(3)
                }, default))
               .ToList();

            // Assert
            result[0].Label.ShouldEqual(testCat1.Name);
            result[1].Label.ShouldEqual(testCat2.Name);
        }

        [Fact]
        public async Task GetValues_CorrectColor()
        {
            // Arrange
            var account = new Account("test");
            var paymentList = new List<Payment>
            {
                new Payment(DateTime.Today, 10, PaymentType.Expense, account, category: new Category("a")),
                new Payment(DateTime.Today, 10, PaymentType.Expense, account, category: new Category("b")),
                new Payment(DateTime.Today, 10, PaymentType.Expense, account, category: new Category("c")),
                new Payment(DateTime.Today, 10, PaymentType.Expense, account, category: new Category("d")),
                new Payment(DateTime.Today, 10, PaymentType.Expense, account, category: new Category("e")),
                new Payment(DateTime.Today, 10, PaymentType.Expense, account, category: new Category("f")),
                new Payment(DateTime.Today, 10, PaymentType.Expense, account, category: new Category("g"))
            };

            context.Payments.AddRange(paymentList);
            context.SaveChanges();

            // Act
            List<StatisticEntry> result =
                (await new GetCategorySpreadingQueryHandler(contextAdapterMock.Object).Handle(new GetCategorySpreadingQuery
                {
                    StartDate = DateTime.Today.AddDays(-3),
                    EndDate = DateTime.Today.AddDays(3)
                }, default))
               .ToList();

            // Assert
            result[0].Color.ShouldEqual("#266489");
            result[1].Color.ShouldEqual("#68B9C0");
            result[2].Color.ShouldEqual("#90D585");
            result[3].Color.ShouldEqual("#F3C151");
            result[4].Color.ShouldEqual("#F37F64");
            result[5].Color.ShouldEqual("#424856");
            result[6].Color.ShouldEqual("#8F97A4");
        }

        [Theory]
        [InlineData("en-US", '$')]
        [InlineData("de-CH", 'C')]
        public async Task GetValues_CorrectCurrency(string culture, char expectedCurrencySymbol)
        {
            // Arrange
            var account = new Account("test");
            var paymentList = new List<Payment>
            {
                new Payment(DateTime.Today, 10, PaymentType.Expense, account, category: new Category("a")),
                new Payment(DateTime.Today, 10, PaymentType.Expense, account, category: new Category("b")),
                new Payment(DateTime.Today, 10, PaymentType.Expense, account, category: new Category("c"))
            };

            context.Payments.AddRange(paymentList);
            context.SaveChanges();

            var cultureInfo = new CultureInfo(culture);
            CultureHelper.CurrentCulture = cultureInfo;

            // Act
            List<StatisticEntry> result =
                (await new GetCategorySpreadingQueryHandler(contextAdapterMock.Object).Handle(new GetCategorySpreadingQuery
                {
                    StartDate = DateTime.Today.AddDays(-3),
                    EndDate = DateTime.Today.AddDays(3)
                }, default))
               .ToList();

            // Assert
            result[0].ValueLabel[0].ShouldEqual(expectedCurrencySymbol);
            result[1].ValueLabel[0].ShouldEqual(expectedCurrencySymbol);
            result[2].ValueLabel[0].ShouldEqual(expectedCurrencySymbol);
        }
    }
}
