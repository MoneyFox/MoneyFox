namespace MoneyFox.Core.Tests._Pending_
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using Core._Pending_;
    using Core.Common;
    using FluentAssertions;
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
