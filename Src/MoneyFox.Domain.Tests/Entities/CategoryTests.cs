using System;
using System.Diagnostics.CodeAnalysis;
using MoneyFox.Domain.Entities;
using Should;
using Xunit;

namespace MoneyFox.DataLayer.Tests.Entities
{
    [ExcludeFromCodeCoverage]
    public class CategoryTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Ctor_NameEmpty_ArgumentNullException(string name)
        {
            // Arrange

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => new Category(name));
        }

        [Fact]
        public void Ctor_NoParams_DefaultValuesSet()
        {
            // Arrange
            const string testName = "test";

            // Act / Assert
            var category = new Category(testName);

            // Assert
            category.Name.ShouldEqual(testName);
            category.Note.ShouldBeEmpty();
            category.ModificationDate.ShouldBeInRange(DateTime.Now.AddSeconds(-1), DateTime.Now);
            category.CreationTime.ShouldBeInRange(DateTime.Now.AddSeconds(-1), DateTime.Now);
        }

        [Fact]
        public void Ctor_Params_ValuesCorrectlySet()
        {
            // Arrange
            const string testName = "test";
            const string testNote = "foo";

            // Act / Assert
            var category = new Category(testName, testNote);

            // Assert
            category.Name.ShouldEqual(testName);
            category.Note.ShouldEqual(testNote);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void UpdateData_NameEmpty_ArgumentNullException(string name)
        {
            // Arrange
            var testCategory = new Category("Foo");

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => testCategory.UpdateData(name));
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
            testCategory.Name.ShouldEqual(testName);
            testCategory.Note.ShouldBeEmpty();
            testCategory.ModificationDate.ShouldBeInRange(DateTime.Now.AddSeconds(-0.5), DateTime.Now);
        }

        [Fact]
        public void UpdateData_Params_ValuesCorrectlySet()
        {
            // Arrange
            const string testName = "test";
            const string testNote = "foo";

            var testCategory = new Category("Foo");

            // Act / Assert
            testCategory.UpdateData(testName, testNote);

            // Assert
            testCategory.Name.ShouldEqual(testName);
            testCategory.Note.ShouldEqual(testNote);
        }
    }
}
