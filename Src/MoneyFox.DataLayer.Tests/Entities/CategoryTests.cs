using System;
using System.Diagnostics.CodeAnalysis;
using MoneyFox.DataLayer.Entities;
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
            const string testname = "test";

            // Act / Assert
            var category = new Category(testname);

            // Assert
            category.Name.ShouldEqual(testname);
            category.Note.ShouldBeEmpty();
        }

        [Fact]
        public void Ctor_Params_ValuesCorrectlySet()
        {
            // Arrange
            const string testname = "test";
            const string testnote = "foo";

            // Act / Assert
            var category = new Category(testname, testnote);

            // Assert
            category.Name.ShouldEqual(testname);
            category.Note.ShouldEqual(testnote);
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
            const string testname = "test";

            var testCategory = new Category("Foo");


            // Act / Assert
            testCategory.UpdateData(testname);

            // Assert
            testCategory.Name.ShouldEqual(testname);
            testCategory.Note.ShouldBeEmpty();
        }

        [Fact]
        public void UpdateData_Params_ValuesCorrectlySet()
        {
            // Arrange
            const string testname = "test";
            const string testnote = "foo";

            var testCategory = new Category("Foo");

            // Act / Assert
            testCategory.UpdateData(testname, testnote);

            // Assert
            testCategory.Name.ShouldEqual(testname);
            testCategory.Note.ShouldEqual(testnote);
        }
    }
}
