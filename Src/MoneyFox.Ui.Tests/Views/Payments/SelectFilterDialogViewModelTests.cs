namespace MoneyFox.Ui.Tests.Views.Payments;

using FluentAssertions;
using Ui.Views.Payments;
using Xunit;

public sealed class SelectFilterDialogViewModelTests
{
    public class IsDateRangeValid
    {
        private readonly SelectFilterDialogViewModel vm = new();

        [Fact]
        public void ShouldReturnFalse_WhenStartDateAfterEndDate()
        {
            // Arrange
            vm.TimeRangeStart = DateTime.Today.AddDays(1);
            vm.TimeRangeEnd = DateTime.Today;

            // Act / Assert
            vm.IsDateRangeValid.Should().BeFalse();
        }

        [Fact]
        public void ShouldReturnTrue_WhenStartDateAfterEndDateAreSame()
        {
            // Arrange
            var now = DateTime.Now;
            vm.TimeRangeStart = now;
            vm.TimeRangeEnd = now;

            // Act / Assert
            vm.IsDateRangeValid.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnTrue_WhenEndDateIsAfterStartDate()
        {
            // Arrange
            vm.TimeRangeStart = DateTime.Today;
            vm.TimeRangeEnd = DateTime.Today.AddDays(1);

            // Act / Assert
            vm.IsDateRangeValid.Should().BeTrue();
        }
    }
}
