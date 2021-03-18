using FluentAssertions;
using Microsoft.Toolkit.Uwp.UI.Controls;
using MoneyFox.Ui.Shared.PaymentSorting;
using MoneyFox.Uwp.Extensions;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace MoneyFox.Uwp.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class SortDirectionExtensionTests
    {
        [Theory]
        [InlineData(SortDirection.Ascending, DataGridSortDirection.Ascending)]
        [InlineData(SortDirection.Descending, DataGridSortDirection.Descending)]
        public void DirectionMappedCorrectly(SortDirection inputDirection, DataGridSortDirection expectedResult)
        {
            // Arrange
            // Act
            var result = inputDirection.ToDataGridDirection();

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}
