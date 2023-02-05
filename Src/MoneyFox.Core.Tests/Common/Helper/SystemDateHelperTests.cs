namespace MoneyFox.Core.Tests.Common.Helper;

using Core.Common;
using FluentAssertions;

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
