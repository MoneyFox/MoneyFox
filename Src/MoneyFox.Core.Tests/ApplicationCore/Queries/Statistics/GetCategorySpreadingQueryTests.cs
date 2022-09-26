namespace MoneyFox.Tests.Core.ApplicationCore.Queries.Statistics
{

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
    using MoneyFox.Core.ApplicationCore.Queries.Statistics;
    using MoneyFox.Infrastructure.Persistence;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("CultureCollection")]
    public class GetCategorySpreadingQueryTests
    {
        private readonly AppDbContext context;
        private readonly GetCategorySpreadingQueryHandler handler;

        public GetCategorySpreadingQueryTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            handler = new GetCategorySpreadingQueryHandler(context);
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
                new Payment(
                    date: DateTime.Today,
                    amount: 60,
                    type: PaymentType.Income,
                    chargedAccount: account,
                    category: testCat1),
                new Payment(
                    date: DateTime.Today,
                    amount: 90,
                    type: PaymentType.Expense,
                    chargedAccount: account,
                    category: testCat1),
                new Payment(
                    date: DateTime.Today,
                    amount: 10,
                    type: PaymentType.Expense,
                    chargedAccount: account,
                    category: testCat3),
                new Payment(
                    date: DateTime.Today,
                    amount: 90,
                    type: PaymentType.Expense,
                    chargedAccount: account,
                    category: testCat2)
            };

            context.Payments.AddRange(paymentList);
            context.SaveChanges();

            // Act
            var result = (await handler.Handle(
                request: new GetCategorySpreadingQuery(startDate: DateTime.Today.AddDays(-3), endDate: DateTime.Today.AddDays(3)),
                cancellationToken: default)).ToList();

            // Assert
            result.Should().HaveCount(3);
            result[0].Value.Should().Be(90);
            result[1].Value.Should().Be(30);
            result[2].Value.Should().Be(10);
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
                new Payment(
                    date: DateTime.Today,
                    amount: 60,
                    type: PaymentType.Income,
                    chargedAccount: account,
                    category: testCat1),
                new Payment(
                    date: DateTime.Today,
                    amount: 90,
                    type: PaymentType.Expense,
                    chargedAccount: account,
                    category: testCat2),
                new Payment(
                    date: DateTime.Today,
                    amount: 10,
                    type: PaymentType.Expense,
                    chargedAccount: account,
                    category: testCat3)
            };

            context.Payments.AddRange(paymentList);
            context.SaveChanges();

            // Act
            var result = (await handler.Handle(
                request: new GetCategorySpreadingQuery(startDate: DateTime.Today.AddDays(-3), endDate: DateTime.Today.AddDays(3)),
                cancellationToken: default)).ToList();

            // Assert
            result.Should().HaveCount(2);
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
                new Payment(
                    date: DateTime.Today,
                    amount: 90,
                    type: PaymentType.Expense,
                    chargedAccount: account,
                    category: testCat1),
                new Payment(
                    date: DateTime.Today,
                    amount: 10,
                    type: PaymentType.Expense,
                    chargedAccount: account,
                    category: testCat2)
            };

            context.Payments.AddRange(paymentList);
            context.SaveChanges();

            // Act
            var result = (await handler.Handle(
                request: new GetCategorySpreadingQuery(startDate: DateTime.Today.AddDays(-3), endDate: DateTime.Today.AddDays(3)),
                cancellationToken: default)).ToList();

            // Assert
            result[0].Label.Should().Be(testCat1.Name);
            result[1].Label.Should().Be(testCat2.Name);
        }

        [Fact]
        public async Task GetValues_CorrectColor()
        {
            // Arrange
            var account = new Account("test");
            var paymentList = new List<Payment>
            {
                new Payment(
                    date: DateTime.Today,
                    amount: 10,
                    type: PaymentType.Expense,
                    chargedAccount: account,
                    category: new Category("a")),
                new Payment(
                    date: DateTime.Today,
                    amount: 10,
                    type: PaymentType.Expense,
                    chargedAccount: account,
                    category: new Category("b")),
                new Payment(
                    date: DateTime.Today,
                    amount: 10,
                    type: PaymentType.Expense,
                    chargedAccount: account,
                    category: new Category("c")),
                new Payment(
                    date: DateTime.Today,
                    amount: 10,
                    type: PaymentType.Expense,
                    chargedAccount: account,
                    category: new Category("d")),
                new Payment(
                    date: DateTime.Today,
                    amount: 10,
                    type: PaymentType.Expense,
                    chargedAccount: account,
                    category: new Category("e")),
                new Payment(
                    date: DateTime.Today,
                    amount: 10,
                    type: PaymentType.Expense,
                    chargedAccount: account,
                    category: new Category("f")),
                new Payment(
                    date: DateTime.Today,
                    amount: 10,
                    type: PaymentType.Expense,
                    chargedAccount: account,
                    category: new Category("g"))
            };

            context.Payments.AddRange(paymentList);
            context.SaveChanges();

            // Act
            var result = (await handler.Handle(
                request: new GetCategorySpreadingQuery(startDate: DateTime.Today.AddDays(-3), endDate: DateTime.Today.AddDays(3)),
                cancellationToken: default)).ToList();

            // Assert
            result[0].Color.Should().Be("#266489");
            result[1].Color.Should().Be("#68B9C0");
            result[2].Color.Should().Be("#90D585");
            result[3].Color.Should().Be("#F3C151");
            result[4].Color.Should().Be("#F37F64");
            result[5].Color.Should().Be("#424856");
            result[6].Color.Should().Be("#8F97A4");
        }

        [Fact]
        public async Task CorrectSumsForIncome()
        {
            // Arrange
            var testCat1 = new Category("Ausgehen");
            var testCat2 = new Category("Rent");
            var account = new Account("test");
            var paymentList = new List<Payment>
            {
                new Payment(
                    date: DateTime.Today,
                    amount: 90,
                    type: PaymentType.Income,
                    chargedAccount: account,
                    category: testCat1),
                new Payment(
                    date: DateTime.Today,
                    amount: 60,
                    type: PaymentType.Expense,
                    chargedAccount: account,
                    category: testCat1),
                new Payment(
                    date: DateTime.Today,
                    amount: 10,
                    type: PaymentType.Expense,
                    chargedAccount: account,
                    category: testCat2),
                new Payment(
                    date: DateTime.Today,
                    amount: 90,
                    type: PaymentType.Income,
                    chargedAccount: account,
                    category: testCat2)
            };

            context.Payments.AddRange(paymentList);
            context.SaveChanges();

            // Act
            var result = (await handler.Handle(
                request: new GetCategorySpreadingQuery(
                    startDate: DateTime.Today.AddDays(-3),
                    endDate: DateTime.Today.AddDays(3),
                    paymentType: PaymentType.Income),
                cancellationToken: default)).ToList();

            // Assert
            result.Should().HaveCount(2);
            result[0].Value.Should().Be(80);
            result[1].Value.Should().Be(30);
        }
    }

}
