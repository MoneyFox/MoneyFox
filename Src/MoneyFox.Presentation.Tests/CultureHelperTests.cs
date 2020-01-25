using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using MoneyFox.Application;
using Should;
using Xunit;

namespace MoneyFox.Presentation.Tests
{
    [ExcludeFromCodeCoverage]
    public class CultureHelperTests
    {
        [Fact]
        public void CorrectDefaultValues()
        {
            // Arrange
            // Act
            // Assert
            CultureHelper.CurrentCulture.ShouldEqual(CultureInfo.CurrentCulture);
        }
    }
}
