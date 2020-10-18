using FluentAssertions;
using MoneyFox.Ui.Shared.Utilities;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace MoneyFox.Ui.Shared.Tests.Utilities
{
    [ExcludeFromCodeCoverage]
    public class HelperFunctionsTests
    {
        [Theory]
        [InlineData(6000000.45)]
        [InlineData(6000000)]
        [InlineData(6000000.4567)]
        public void FormatLargeNumbers_ValidString(decimal amount) => HelperFunctions.FormatLargeNumbers(amount).Should().Be(amount.ToString("N2"));
    }
}
