using FluentAssertions;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace MoneyFox.Application.Tests
{
    [ExcludeFromCodeCoverage]
    public class SystemDateHelperTests
    {
        [Fact]
        public void ValueCorrectInitialized()
        {
            // Arrange
            // Act
            // Assert
            SystemDateHelper.Today.Should().Be(DateTime.Today);
        }
    }
}
