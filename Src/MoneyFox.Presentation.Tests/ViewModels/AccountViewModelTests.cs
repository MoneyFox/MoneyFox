using System.Diagnostics.CodeAnalysis;
using MoneyFox.Presentation.ViewModels;
using Should;
using Xunit;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class AccountViewModelTests
    {
        [Fact]
        public void Equals_Null_False()
        {
            // Arrange
            var testVm = new AccountViewModel();

            // Act
            bool result = testVm.Equals(null);

            // result
            result.ShouldBeFalse();
        }

        [Fact]
        public void Equals_OtherType_False()
        {
            // Arrange
            var testVm = new AccountViewModel();

            // Act
            // ReSharper disable once SuspiciousTypeConversion.Global : Just for this test.
            bool result = testVm.Equals(new CategoryViewModel());

            // result
            result.ShouldBeFalse();
        }

        [Fact]
        public void Equals_SameObject_True()
        {
            // Arrange
            var testVm = new AccountViewModel();

            // Act
            // ReSharper disable once EqualExpressionComparison : Just for this test.
            bool result = testVm.Equals(testVm);

            // result
            result.ShouldBeTrue();
        }

        [Fact]
        public void Equals_SameId_True()
        {
            // Arrange
            var testVm = new AccountViewModel
            {
                Id = 99
            };
            var compareVm = new AccountViewModel
            {
                Id = 99
            };

            // Act
            // ReSharper disable once SuspiciousTypeConversion.Global just for this test.
            bool result = testVm.Equals(compareVm);

            // result
            result.ShouldBeTrue();
        }

        [Fact]
        public void Equals_SameIdDifferentValues_True()
        {
            // Arrange
            var testVm = new AccountViewModel
            {
                Id = 99,
                Name = "Test1"
            };
            var compareVm = new AccountViewModel
            {
                Id = 99,
                Name = "Test2"
            };

            // Act
            // ReSharper disable once SuspiciousTypeConversion.Global just for this test.
            bool result = testVm.Equals(compareVm);

            // result
            result.ShouldBeTrue();
        }
    }
}
