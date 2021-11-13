using FluentAssertions;
using MoneyFox.Application.Common.Interfaces;
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
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Application.Tests.Statistics.Queries
{
    [ExcludeFromCodeCoverage]
    [Collection("CultureCollection")]
    public class GetAccountProgressionHandlerTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetAccountProgressionHandlerTests()
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
        public async Task CalculateCorrectSums()
        {
            // Arrange
            var account = new Account("Foo1");
            context.AddRange(new List<Payment>
            {
                new Payment(DateTime.Today, 60, PaymentType.Income, account),
                new Payment(DateTime.Today, 20, PaymentType.Expense, account),
                new Payment(DateTime.Today.AddMonths(-1), 50, PaymentType.Expense, account),
                new Payment(DateTime.Today.AddMonths(-2), 40, PaymentType.Expense, account)
            });
            context.Add(account);
            context.SaveChanges();

            // Act
            List<StatisticEntry> result = await new GetAccountProgressionHandler(contextAdapterMock.Object).Handle(
                new GetAccountProgressionQuery(account.Id,
                    DateTime.Today.AddYears(-1),
                    DateTime.Today.AddDays(3)), default);

            // Assert
            result[0].Value.Should().Be(40);
            result[1].Value.Should().Be(-50);
            result[2].Value.Should().Be(-40);
        }

        [Fact]
        public async Task GetValues_CorrectSums()
        {
            // Arrange
            var account = new Account("Foo1");
            context.AddRange(new List<Payment>
            {
                new Payment(DateTime.Today, 60, PaymentType.Income, account),
                new Payment(DateTime.Today, 20, PaymentType.Expense, account),
                new Payment(DateTime.Today.AddMonths(-1), 50, PaymentType.Expense, account),
                new Payment(DateTime.Today.AddMonths(-2), 40, PaymentType.Expense, account)
            });
            context.Add(account);
            context.SaveChanges();

            // Act
            List<StatisticEntry> result = await new GetAccountProgressionHandler(contextAdapterMock.Object).Handle(
                new GetAccountProgressionQuery(account.Id,
                    DateTime.Today.AddYears(-1),
                    DateTime.Today.AddDays(3)), default);

            // Assert
            result[0].Color.Should().Be("#87cefa");
            result[1].Color.Should().Be("#cd3700");
            result[2].Color.Should().Be("#cd3700");
        }
    }
}