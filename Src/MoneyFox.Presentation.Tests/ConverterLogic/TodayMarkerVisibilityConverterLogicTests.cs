using System;
using MoneyFox.Presentation.ConverterLogic;
using Should;
using Xunit;

namespace MoneyFox.Presentation.Tests.ConverterLogic
{
    public class TodayMarkerVisibilityConverterLogicTests
    {
        [Fact]
        public void ShowMarker_Today_True()
        {
            // Arrange
            // Act
            bool result = TodayMarkerVisibilityConverterLogic.ShowMarker(DateTime.Now);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void ShowMarker_Tomorrow_True()
        {
            // Arrange
            // Act
            bool result = TodayMarkerVisibilityConverterLogic.ShowMarker(DateTime.Now.AddDays(1));

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void ShowMarker_Yesterday_True()
        {
            // Arrange
            // Act
            bool result = TodayMarkerVisibilityConverterLogic.ShowMarker(DateTime.Now.AddDays(-1));

            // Assert
            result.ShouldBeFalse();
        }
    }
}
