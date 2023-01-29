namespace MoneyFox.Ui.Tests.Utilities;

using Common.Utilities;
using FluentAssertions;
using Xunit;

public class DecimalFormatterTests
{
    [Theory]
    [InlineData(6000000.45)]
    [InlineData(6000000)]
    [InlineData(6000000.4567)]
    public void FormatLargeNumbers_ValidString(decimal amount)
    {
        _ = DecimalFormatter.AsLargeNumber(amount).Should().Be(amount.ToString("N2"));
    }
}
