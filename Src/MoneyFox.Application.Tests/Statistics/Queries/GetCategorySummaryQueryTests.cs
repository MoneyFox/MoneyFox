using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Resources;
using MoneyFox.Application.Statistics.Queries.GetCategorySummary;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Statistics.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetCategorySummaryQueryTests : IDisposable
    {
        private readonly EfCoreContext context;

        public GetCategorySummaryQueryTests()
        {
            context = TestEfCoreContextFactory.Create();
        }

        public void Dispose()
        {
            TestEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetValues_CorrectSums()
        {
            // Arrange
            var testCat1 = new Category("Ausgehen");
            var testCat2 = new Category("Rent");
            var testCat3 = new Category("Food");
            var testCat4 = new Category("Income");

            var account = new Account("test");

            var paymentList = new List<Payment>
            {
                new Payment(DateTime.Today, 60, PaymentType.Income, account, category: testCat1),
                new Payment(DateTime.Today, 90, PaymentType.Expense, account, category: testCat1),
                new Payment(DateTime.Today, 10, PaymentType.Expense, account, category: testCat3),
                new Payment(DateTime.Today, 90, PaymentType.Expense, account, category: testCat2),
                new Payment(DateTime.Today, 100, PaymentType.Income, account, category: testCat4)
            };

            context.Payments.AddRange(paymentList);
            context.SaveChanges();

            // Act
            CategorySummaryModel result = await new GetCategorySummaryQuery.GetCategorySummaryQueryHandler(context).Handle(new GetCategorySummaryQuery
            {
                StartDate = DateTime.Today.AddDays(-3),
                EndDate = DateTime.Today.AddDays(3)
            }, default);

            // Assert
            result.CategoryOverviewItems.Count.ShouldEqual(4);
            result.CategoryOverviewItems[0].Value.ShouldEqual(-90);
            result.CategoryOverviewItems[1].Value.ShouldEqual(-30);
            result.CategoryOverviewItems[2].Value.ShouldEqual(-10);
            result.CategoryOverviewItems[3].Value.ShouldEqual(100);
        }

        [Fact]
        public async Task GetValues_CorrectLabels()
        {
            // Arrange
            var testCat1 = new Category("Ausgehen");
            var testCat2 = new Category("Rent");
            var testCat3 = new Category("Food");
            var testCat4 = new Category("Income");

            var account = new Account("test");
            var paymentList = new List<Payment>
            {
                new Payment(DateTime.Today, 90, PaymentType.Expense, account, category: testCat1),
                new Payment(DateTime.Today, 90, PaymentType.Expense, account, category: testCat2),
                new Payment(DateTime.Today, 10, PaymentType.Expense, account, category: testCat3),
                new Payment(DateTime.Today, 100, PaymentType.Income, account, category: testCat4)
            };

            context.Payments.AddRange(paymentList);
            context.SaveChanges();

            // Act
            CategorySummaryModel result = await new GetCategorySummaryQuery.GetCategorySummaryQueryHandler(context).Handle(new GetCategorySummaryQuery
            {
                StartDate = DateTime.Today.AddDays(-3),
                EndDate = DateTime.Today.AddDays(3)
            }, default);

            // Assert
            result.CategoryOverviewItems[0].Label.ShouldEqual(testCat1.Name);
            result.CategoryOverviewItems[1].Label.ShouldEqual(testCat2.Name);
            result.CategoryOverviewItems[2].Label.ShouldEqual(testCat3.Name);
            result.CategoryOverviewItems[3].Label.ShouldEqual(testCat4.Name);
        }

        [Fact]
        public async Task GetValues_CorrectPercentage()
        {
            // Arrange
            var testCat1 = new Category("Ausgehen");
            var testCat2 = new Category("Ausgehen");
            var testCat3 = new Category("Income");

            var account = new Account("test");

            var paymentList = new List<Payment>
            {
                new Payment(DateTime.Today, 60, PaymentType.Expense, account, category: testCat1),
                new Payment(DateTime.Today, 90, PaymentType.Expense, account, category: testCat2),
                new Payment(DateTime.Today, 100, PaymentType.Income, account, category: testCat3)
            };

            context.Payments.AddRange(paymentList);
            context.SaveChanges();

            // Act
            CategorySummaryModel result = await new GetCategorySummaryQuery.GetCategorySummaryQueryHandler(context).Handle(new GetCategorySummaryQuery
            {
                StartDate = DateTime.Today.AddDays(-3),
                EndDate = DateTime.Today.AddDays(3)
            }, default);

            // Assert
            result.CategoryOverviewItems[0].Percentage.ShouldEqual(60);
            result.CategoryOverviewItems[1].Percentage.ShouldEqual(40);
            result.CategoryOverviewItems[2].Percentage.ShouldEqual(100);
        }

        [Fact]
        public async Task GetValues_NoCategory_CorrectValue()
        {
            // Arrange
            var account = new Account("test");

            var paymentList = new List<Payment>
            {
                new Payment(DateTime.Today, 60, PaymentType.Expense, account)
            };

            context.Payments.AddRange(paymentList);
            context.SaveChanges();

            // Act
            CategorySummaryModel result = await new GetCategorySummaryQuery.GetCategorySummaryQueryHandler(context).Handle(new GetCategorySummaryQuery
            {
                StartDate = DateTime.Today.AddDays(-3),
                EndDate = DateTime.Today.AddDays(3)
            }, default);

            // Assert
            result.CategoryOverviewItems[0].Value.ShouldEqual(-60);
        }

        [Fact]
        public async Task GetValues_NoCategory_CorrectLabel()
        {
            // Arrange
            var account = new Account("test");

            var paymentList = new List<Payment>
            {
                new Payment(DateTime.Today, 60, PaymentType.Expense, account)
            };

            context.Payments.AddRange(paymentList);
            context.SaveChanges();

            // Act
            CategorySummaryModel result = await new GetCategorySummaryQuery.GetCategorySummaryQueryHandler(context).Handle(new GetCategorySummaryQuery
            {
                StartDate = DateTime.Today.AddDays(-3),
                EndDate = DateTime.Today.AddDays(3)
            }, default);

            // Assert
            result.CategoryOverviewItems[0].Label.ShouldEqual(Strings.NoCategoryLabel);
        }
    }
}
