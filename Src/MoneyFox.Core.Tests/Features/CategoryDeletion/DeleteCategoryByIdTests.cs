namespace MoneyFox.Core.Tests.Features.CategoryDeletion;

using Microsoft.EntityFrameworkCore;
using MoneyFox.Core.Features.CategoryDeletion;
using MoneyFox.Domain.Tests.TestFramework;

public class DeleteCategoryByIdCommandTests : InMemoryTestBase
{
    private readonly DeleteCategoryById.Handler handler;

    public DeleteCategoryByIdCommandTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task DeleteCategoryWithPassedId()
    {
        // Arrange
        var testCategory = new TestData.CategoryBeverages();
        Context.RegisterCategory(testCategory: testCategory);

        // Act
        await handler.Handle(command: new(testCategory.Id), cancellationToken: default);

        // Assert
        (await Context.Categories.SingleOrDefaultAsync(x => x.Id == testCategory.Id)).Should().BeNull();
    }

    [Fact]
    public async Task DoesNothingWhenCategoryNotFound()
    {
        // Arrange
        var testCategory = new TestData.CategoryBeverages();
        Context.RegisterCategory(testCategory: testCategory);

        // Act
        await handler.Handle(command: new(99), cancellationToken: default);

        // Assert
        (await Context.Categories.SingleOrDefaultAsync(x => x.Id == testCategory.Id)).Should().NotBeNull();
    }

    [Fact]
    public async Task RemoveCategoryFromPaymentOnDelete()
    {
        // Arrange
        var expense = new TestData.ClearedExpense();
        var dbExpense = Context.RegisterPayment(testPayment: expense);
        var income = new TestData.ClearedIncome();
        var dbIncome = Context.RegisterPayment(testPayment: income);

        // Act
        await handler.Handle(command: new(dbExpense.Category!.Id), cancellationToken: default);

        // Assert
        var unassignedPayment = await Context.Payments.SingleAsync(x => x.Id == dbExpense.Id);
        unassignedPayment.Category.Should().BeNull();
        var unmodifiedPayment = await Context.Payments.SingleAsync(x => x.Id == dbIncome.Id);
        unmodifiedPayment.Category.Should().NotBeNull();
    }
}
