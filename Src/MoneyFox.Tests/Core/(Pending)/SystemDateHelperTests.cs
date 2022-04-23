namespace MoneyFox.Tests.Core._Pending_
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using FluentAssertions;
    using MoneyFox.Core.Common;
    using Xunit;

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

}
