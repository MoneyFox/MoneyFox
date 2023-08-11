namespace MoneyFox.Core.Tests.Queries;

using Core.Queries;
using Domain.Tests.TestFramework;
using FluentAssertions;

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
    public async Task ReturnsCorrectNumberOfPayments_WhenMultiplePaymentsAvailable()
    {
        // Arrange
        var expense = new TestData.ClearedExpense { Category = new TestData.ClearedExpense.ExpenseCategory() };
        Context.RegisterPayment(expense);
        var payment = new TestData.ClearedIncome { Category = new TestData.ClearedIncome.IncomeCategory() };
        var dbPayment = Context.RegisterPayment(payment);

        // Act
        var query = new GetNumberOfPaymentsAssignedToCategory.Query(dbPayment.Category!.Id);
        var result = await handler.Handle(request: query, cancellationToken: CancellationToken.None);

        // Assert
        result.Should().Be(1);
    }
}
