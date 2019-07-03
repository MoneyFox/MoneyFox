using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.Application.Statistics.Queries.GetCategorySpreading;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Statistics.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetCategorySpreadingQueryHandlerTests : IDisposable
    {
        private readonly EfCoreContext context;

        public GetCategorySpreadingQueryHandlerTests()
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

            var account = new Account("test");

            var paymentList = new List<Payment>
            {
                new Payment(DateTime.Today, 60, PaymentType.Income, account, category:testCat1),
                new Payment(DateTime.Today, 90, PaymentType.Expense, account, category:testCat1),
                new Payment(DateTime.Today, 10, PaymentType.Expense, account, category:testCat3),
                new Payment(DateTime.Today, 90, PaymentType.Expense, account, category:testCat2)
            };

            context.Payments.AddRange(paymentList);
            context.SaveChanges();

            // Act
            var result = (await new GetCategorySpreadingQueryHandler(context).Handle(new GetCategorySpreadingQuery
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
                new Payment(DateTime.Today, 60, PaymentType.Income, account, category:testCat1),
                new Payment(DateTime.Today, 90, PaymentType.Expense, account, category:testCat2),
                new Payment(DateTime.Today, 10, PaymentType.Expense, account, category:testCat3)
            };


            context.Payments.AddRange(paymentList);
            context.SaveChanges();

            // Act
            var result = (await new GetCategorySpreadingQueryHandler(context).Handle(new GetCategorySpreadingQuery
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
                new Payment(DateTime.Today, 90, PaymentType.Expense, account, category:testCat1),
                new Payment(DateTime.Today, 10, PaymentType.Expense, account, category:testCat2)
            };


            context.Payments.AddRange(paymentList);
            context.SaveChanges();

            // Act
            var result = (await new GetCategorySpreadingQueryHandler(context).Handle(new GetCategorySpreadingQuery
                {
                    StartDate = DateTime.Today.AddDays(-3),
                    EndDate = DateTime.Today.AddDays(3)
                }, default))
                .ToList();

            // Assert
            result[0].Label.ShouldEqual(testCat1.Name);
            result[1].Label.ShouldEqual(testCat2.Name);
        }
    }
}
