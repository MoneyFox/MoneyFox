namespace MoneyFox.Core.Tests.Features.CategoryCreation;

using Microsoft.EntityFrameworkCore;
using MoneyFox.Core.Features.CategoryCreation;
using MoneyFox.Domain.Tests.TestFramework;
using static Domain.Tests.TestFramework.CategoryAssertion;

public class CreateCategoryTests : InMemoryTestBase
{
    [Fact]
    public async Task CategoryAdded_WhenValidValuesPassed()
    {
        // Arrange
        var testCategory = new TestData.DefaultCategory();

        // Act
        var command = new CreateCategory.Command(Name: testCategory.Name, Note: testCategory.Note, RequireNote: testCategory.RequireNote);
        await new CreateCategory.Handler(Context).Handle(command: command, cancellationToken: CancellationToken.None);

        // Assert
        var loadedCategory = await Context.Categories.FirstAsync();
        AssertCategory(actual: loadedCategory, expected: testCategory);
    }
}
