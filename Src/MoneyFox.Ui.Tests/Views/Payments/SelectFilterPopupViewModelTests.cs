namespace MoneyFox.Ui.Tests.Views.Payments;

using CommunityToolkit.Mvvm.Messaging;
using Ui.Views.Payments.PaymentList;

public sealed class SelectFilterPopupViewModelTests
{
    public class IsDateRangeValid
    {
        private readonly SelectFilterPopupViewModel vm = new();

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
