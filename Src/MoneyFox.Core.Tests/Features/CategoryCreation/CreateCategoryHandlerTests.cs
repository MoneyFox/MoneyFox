namespace MoneyFox.Core.Tests.Features.CategoryCreation;

using Core.Features.CategoryCreation;
using Domain.Tests.TestFramework;
using Microsoft.EntityFrameworkCore;
using static Domain.Tests.TestFramework.CategoryAssertion;

public class CreateCategoryHandlerTests : InMemoryTestBase
{
    [Fact]
    public async Task CategoryAdded_WhenValidValuesPassed()
    {
        // Arrange
        var testCategory = new TestData.DefaultCategory();

        // Act
        var command = new CreateCategory.Command(name: testCategory.Name, note: testCategory.Note, requireNote: testCategory.RequireNote);
        await new CreateCategory.Handler(Context).Handle(request: command, cancellationToken: CancellationToken.None);

        // Assert
        var loadedCategory = await Context.Categories.FirstAsync();
        AssertCategory(actual: loadedCategory!, expected: testCategory);
    }
}
