using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Payments.Queries.GetPaymentById;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Payments.Query.GetPaymentById
{
    [ExcludeFromCodeCoverage]
    public class GetPaymentByIdQueryTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetPaymentByIdQueryTests()
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
        public async Task GetCategory_CategoryNotFound()
        {
            // Arrange

            // Act
            Payment result = await new GetPaymentByIdQuery.Handler(contextAdapterMock.Object).Handle(new GetPaymentByIdQuery(999), default);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public async Task GetCategory_CategoryFound()
        {
            // Arrange
            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, new Account("test", 80));
            await context.AddAsync(payment1);
            await context.SaveChangesAsync();

            // Act
            Payment result = await new GetPaymentByIdQuery.Handler(contextAdapterMock.Object).Handle(new GetPaymentByIdQuery(payment1.Id), default);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldEqual(payment1.Id);
        }
    }
}
