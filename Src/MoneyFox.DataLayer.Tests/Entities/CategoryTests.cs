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
    }
}
