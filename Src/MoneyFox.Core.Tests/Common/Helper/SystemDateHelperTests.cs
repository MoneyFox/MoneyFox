namespace MoneyFox.Core.Tests.Common.Helper;

using System.Diagnostics.CodeAnalysis;
using Core.Common.Helpers;
using FluentAssertions;

[ExcludeFromCodeCoverage]
public class SystemDateHelperTests
{
    [Fact]
    public void ValueCorrectInitialized()
    {
        // Arrange
        // Act
        var systemDateHelper = new SystemDateHelper();

        // Assert
        systemDateHelper.Today.Should().Be(DateTime.Today);
    }
}
