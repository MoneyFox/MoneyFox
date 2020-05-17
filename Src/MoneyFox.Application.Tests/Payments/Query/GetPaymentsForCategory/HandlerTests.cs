using FluentAssertions;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Payments.Queries.GetPaymentsForCategory;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Application.Tests.Payments.Query.GetPaymentsForCategory
{
    [ExcludeFromCodeCoverage]
    public class HandlerTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public HandlerTests()
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
        public async Task CorrectPaymentsSelected()
        {
            // Arrange
            var account = new Account("asdf");
            var category = new Category("Test");
            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, account);
            var payment2 = new Payment(DateTime.Now, 30, PaymentType.Expense, account, category: category);
            var payment3 = new Payment(DateTime.Now, 40, PaymentType.Expense, account, category: category);

            context.Add(payment1);
            context.Add(payment2);
            context.Add(payment3);
            await context.SaveChangesAsync();

            // Act
            var result = await new GetPaymentsForCategoryQuery.Handler(contextAdapterMock.Object).Handle(
                new GetPaymentsForCategoryQuery(
                    category.Id,
                    DateTime.Now.AddDays(-1),
                    DateTime.Now.AddDays(1)), default);

            // Assert
            result.Count.Should().Be(2);
        }
    }
}
