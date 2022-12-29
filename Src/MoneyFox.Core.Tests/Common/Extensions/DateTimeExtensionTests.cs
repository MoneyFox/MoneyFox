namespace MoneyFox.Core.Tests.Common.Extensions;

using System.Diagnostics.CodeAnalysis;
using Core.Common.Extensions;

[ExcludeFromCodeCoverage]
public class DateTimeExtensionTests
{
    [Fact]
    public void GetFirstDayOfMonth()
    {
        Assert.Equal(expected: new(year: 2017, month: 03, day: 01), actual: new DateTime(year: 2017, month: 03, day: 29).GetFirstDayOfMonth());
    }

    [Theory]
    [InlineData(02, 28)]
    [InlineData(01, 31)]
    [InlineData(04, 30)]
    public void GetLastDayOfMonth(int month, int expectedDay)
    {
        Assert.Equal(expected: new(year: 2017, month: month, day: expectedDay), actual: new DateTime(year: 2017, month: month, day: 15).GetLastDayOfMonth());
    }

    [Theory]
    [InlineData(-7)]
    [InlineData(0)]
    [InlineData(7)]
    public void GetDaysFromToday(int dayOffset)
    {
        Assert.Equal(expected: dayOffset, actual: DateTime.Today.AddDays(dayOffset).GetDaysFromToday());
    }
}
