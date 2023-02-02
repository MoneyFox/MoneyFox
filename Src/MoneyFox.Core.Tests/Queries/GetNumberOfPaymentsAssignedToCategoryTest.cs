namespace MoneyFox.Core.Tests.Queries;

using Core.Common.Interfaces;
using Domain.Tests.TestFramework;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetNumberOfPaymentsAssignedToCategory
{
    public record Query(int CategoryId) : IRequest<int>;

    public class Handler : IRequestHandler<Query, int>
    {
        private readonly IAppDbContext dbContext;

        public Handler(IAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            return await dbContext.Payments
                .Where(p => p.Category != null)
                .Where(p => p.Category!.Id == request.CategoryId)
                .CountAsync(cancellationToken);
        }
    }
}

public class GetNumberOfPaymentsAssignedToCategoryTest : InMemoryTestBase
{
    private readonly GetNumberOfPaymentsAssignedToCategory.Handler handler;

    public GetNumberOfPaymentsAssignedToCategoryTest()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task ReturnsZeroWhenNoPaymentsFoundForPassedId()
    {
        // Arrange
        var testCategory = new TestData.DefaultCategory();
        Context.RegisterCategory(testCategory);

        // Act
        var query = new GetNumberOfPaymentsAssignedToCategory.Query(testCategory.Id);
        var result = await handler.Handle(request: query, cancellationToken: CancellationToken.None);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public async Task ReturnsZeroWhenCategoryWasNotFound()
    {
        // Act
        var query = new GetNumberOfPaymentsAssignedToCategory.Query(999);
        var result = await handler.Handle(request: query, cancellationToken: CancellationToken.None);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public async Task ReturnsCorrectNumberOfPayments_WhenSinglePayment()
    {
        // Arrange
        var payment = new TestData.DefaultExpense();
        var dbPayment = Context.RegisterPayment(payment);

        // Act
        var query = new GetNumberOfPaymentsAssignedToCategory.Query(dbPayment.Category!.Id);
        var result = await handler.Handle(request: query, cancellationToken: CancellationToken.None);

        // Assert
        result.Should().Be(1);
    }

    [Fact]
    public async Task ReturnsCorrectNumberOfPayments_WhenMultiplePaymentsAvailable()
    {
        // Arrange
        var expense = new TestData.DefaultExpense { Category = new TestData.DefaultExpense.ExpenseCategory() };
        var dbExpense = Context.RegisterPayment(expense);
        var payment = new TestData.DefaultIncome { Category = new TestData.DefaultIncome.IncomeCategory() };
        var dbPayment = Context.RegisterPayment(payment);

        // Act
        var query = new GetNumberOfPaymentsAssignedToCategory.Query(dbPayment.Category!.Id);
        var result = await handler.Handle(request: query, cancellationToken: CancellationToken.None);

        // Assert
        result.Should().Be(1);
    }
}
