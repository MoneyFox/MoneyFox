using System;
using MoneyFox.Presentation.Converter;
using Should;
using Xunit;

namespace MoneyFox.Presentation.Tests.Converter
{
    public class DateVisibilityConverterTests
    {
        [Fact]
        public void Convert_False()
        {
            ((bool)new DateVisibilityConverter().Convert(new DateTime(), null, null, null)).ShouldBeFalse();
        }

        [Fact]
        public void Convert()
        {
            ((bool) new DateVisibilityConverter().Convert(DateTime.Now, null, null, null)).ShouldBeTrue();
        }
    }
}
