using System;
using System.Diagnostics.CodeAnalysis;
using MoneyFox.DataLayer.Entities;
using Should;
using Xunit;

namespace MoneyFox.DataLayer.Tests.Entities
{
    [ExcludeFromCodeCoverage]
    public class AccountTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Ctor_NameEmpty_ArgumentNullException(string name)
        {
            // Arrange

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => new Account(name));
        }

        [Theory]
        [InlineData(-12, true)]
        [InlineData(12, false)]
        [InlineData(0, false)]
        public void Ctor_Balance_IsOverdrawnCorrectSet(double currentBalance, bool expectedIsOverdrawn)
        {
            // Arrange

            // Act / Assert
            var account = new Account("test", currentBalance);

            // Assert
            account.IsOverdrawn.ShouldEqual(expectedIsOverdrawn);
        }

        [Fact]
        public void Ctor_NoParams_DefaultValuesSet()
        {
            // Arrange
            const string testname = "test";

            // Act / Assert
            var account = new Account(testname);

            // Assert
            account.Name.ShouldEqual(testname);
            account.CurrentBalance.ShouldEqual(0);
            account.Note.ShouldBeEmpty();
            account.IsOverdrawn.ShouldBeFalse();
            account.IsExcluded.ShouldBeFalse();
        }

        [Fact]
        public void Ctor_Params_ValuesCorrectlySet()
        {
            // Arrange
            const string testname = "test";
            const double testBalance = 10;
            const string testnote = "foo";
            const bool testExcluded = true;

            // Act / Assert
            var account = new Account(testname, testBalance, testnote, testExcluded);

            // Assert
            account.Name.ShouldEqual(testname);
            account.CurrentBalance.ShouldEqual(testBalance);
            account.Note.ShouldEqual(testnote);
            account.IsExcluded.ShouldEqual(testExcluded);
            account.IsOverdrawn.ShouldBeFalse();
        }
    }
}
