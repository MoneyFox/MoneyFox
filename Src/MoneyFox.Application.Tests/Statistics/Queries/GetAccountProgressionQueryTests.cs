using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.Application.Statistics;
using MoneyFox.Application.Statistics.Queries;
using MoneyFox.Application.Statistics.Queries.GetCashFlow;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using Should;
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
    public class GetAccountProgressionQueryTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetAccountProgressionQueryTests()
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
            Account account = new ("Foo1");
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
            List<StatisticEntry> result = await new GetAccountProgressionHandler(contextAdapterMock.Object).Handle(new GetAccountProgressionQuery
            {
                AccountId = account.Id,
                StartDate = DateTime.Today.AddYears(-1),
                EndDate = DateTime.Today.AddDays(3)
            }, default);

            // Assert
            result[0].Value.ShouldEqual(40);
            result[1].Value.ShouldEqual(-50);
            result[2].Value.ShouldEqual(-40);
        }
    }
}
