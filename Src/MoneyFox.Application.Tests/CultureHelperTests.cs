using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Xunit;

namespace MoneyFox.Application.Tests
{
    [ExcludeFromCodeCoverage]
    public class CultureHelperTests
    {
        [Fact]
        public void ValueCorrectInitialized()
        {
            // Arrange
            // Act
            // Assert
            CultureHelper.CurrentCulture.Should().Be(CultureInfo.CurrentCulture);
        }
    }
}
