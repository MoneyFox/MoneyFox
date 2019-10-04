using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Payments.Queries.GetPaymentById;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Payments.Query.GetPaymentById {
    [ExcludeFromCodeCoverage]
    public class GetPaymentByIdQueryTests : IDisposable {
        private readonly EfCoreContext context;

        public GetPaymentByIdQueryTests() {
            context = TestEfCoreContextFactory.Create();
        }

        public void Dispose() {
            TestEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetPayment_PaymentNotFound() {
            // Arrange

            // Act
            var result = await new GetPaymentByIdQuery.Handler(context).Handle(new GetPaymentByIdQuery(999), default);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public async Task GetPayment_PaymentFound() {
            // Arrange
            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, new Account("test", 80));
            await context.AddAsync(payment1);
            await context.SaveChangesAsync();

            // Act
            var result = await new GetPaymentByIdQuery.Handler(context).Handle(new GetPaymentByIdQuery(payment1.Id), default);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldEqual(payment1.Id);
        }
    }
}
