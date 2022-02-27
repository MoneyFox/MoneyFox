namespace MoneyFox.Core.Tests.Aggregates
{
    using Core.Aggregates.Payments;
    using FluentAssertions;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class CategoryTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Ctor_NameEmpty_ArgumentNullException(string name) =>
            // Arrange

            // Act / Assert
            Assert.Throws<ArgumentException>(() => new Category(name));

        [Fact]
        public void Ctor_NoParams_DefaultValuesSet()
        {
            // Arrange
            const string testName = "test";

            // Act / Assert
            var category = new Category(testName);

            // Assert
            category.Name.Should().Be(testName);
            category.Note.Should().BeEmpty();
            category.RequireNote.Should().BeFalse();
        }

        [Fact]
        public void Ctor_Params_ValuesCorrectlySet()
        {
            // Arrange
            const string testName = "test";
            const string testNote = "foo";

            // Act / Assert
            var category = new Category(testName, testNote, true);

            // Assert
            category.Name.Should().Be(testName);
            category.Note.Should().Be(testNote);
            category.RequireNote.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void UpdateData_NameEmpty_ArgumentNullException(string name)
        {
            // Arrange
            var testCategory = new Category("Foo");

            // Act / Assert
            Assert.Throws<ArgumentException>(() => testCategory.UpdateData(name));
        }

        [Fact]
        public void UpdateData_NoParams_DefaultValuesSet()
        {
            // Arrange
            const string testName = "test";

            var testCategory = new Category("Foo");

            // Act / Assert
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

            // Act / Assert
            testCategory.UpdateData(testName, testNote, true);

            // Assert
            testCategory.Name.Should().Be(testName);
            testCategory.Note.Should().Be(testNote);
            testCategory.RequireNote.Should().BeTrue();
        }
    }
}