namespace MoneyFox.Domain.Tests.Aggregates;

using Domain.Aggregates.CategoryAggregate;
using FluentAssertions;

public class CategoryTests
{
    [Fact]
    public void Ctor_NameEmpty_ArgumentNullException()
    {
        // Act
        Action act = () => _ = new Category(string.Empty);

        // Arrange
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Ctor_NoParams_DefaultValuesSet()
    {
        // Arrange
        const string testName = "test";

        // Act
        var category = new Category(testName);

        // Assert
        category.Name.Should().Be(testName);
        category.Note.Should().BeNull();
        category.RequireNote.Should().BeFalse();
    }

    [Fact]
    public void Ctor_Params_ValuesCorrectlySet()
    {
        // Arrange
        const string testName = "test";
        const string testNote = "foo";

        // Act
        var category = new Category(name: testName, note: testNote, requireNote: true);

        // Assert
        category.Name.Should().Be(testName);
        category.Note.Should().Be(testNote);
        category.RequireNote.Should().BeTrue();
    }

    [Fact]
    public void UpdateData_NameEmpty_ArgumentNullException()
    {
        // Arrange
        var testCategory = new Category("Foo");

        // Act
        var act = () => testCategory.UpdateData(string.Empty);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void UpdateData_NoParams_DefaultValuesSet()
    {
        // Arrange
        const string testName = "test";
        var testCategory = new Category("Foo");

        // Act
        testCategory.UpdateData(testName);

        // Assert
        testCategory.Name.Should().Be(testName);
        testCategory.Note.Should().BeEmpty();
        testCategory.RequireNote.Should().BeFalse();
    }

    [Fact]
    public void UpdateData_Params_ValuesCorrectlySet()
    {
        // Arrange
        const string testName = "test";
        const string testNote = "foo";
        var testCategory = new Category("Foo");

        // Act
        testCategory.UpdateData(name: testName, note: testNote, requireNote: true);

        // Assert
        testCategory.Name.Should().Be(testName);
        testCategory.Note.Should().Be(testNote);
        testCategory.RequireNote.Should().BeTrue();
    }
}
