namespace MoneyFox.Core.Tests.Features;

using Core.Features.CategoryCreation;
using Domain.Tests.TestFramework;
using Microsoft.EntityFrameworkCore;
using static Domain.Tests.TestFramework.CategoryAssertion;

public class CreateCategoryTests : InMemoryTestBase
{
    [Fact]
    public async Task CategoryAdded_WhenValidValuesPassed()
    {
        // Arrange
        var testCategory = new TestData.CategoryBeverages();

        // Act
        var command = new CreateCategory.Command(Name: testCategory.Name, Note: testCategory.Note, RequireNote: testCategory.RequireNote);
        await new CreateCategory.Handler(Context).Handle(command: command, cancellationToken: CancellationToken.None);

        // Assert
        var loadedCategory = await Context.Categories.FirstAsync();
        AssertCategory(actual: loadedCategory, expected: testCategory);
    }
}
