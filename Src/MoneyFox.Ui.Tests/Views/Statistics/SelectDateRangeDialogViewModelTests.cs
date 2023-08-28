namespace MoneyFox.Ui.Tests.Views.Statistics;

using Ui.Views.Statistics;

public sealed class SelectDateRangeDialogViewModelTests
{
    public class IsDateRangeValid
    {
        private readonly SelectDateRangeDialogViewModel vm = new();

        [Fact]
        public void ShouldReturnFalse_WhenStartDateAfterEndDate()
        {
            // Arrange
            vm.StartDate = DateTime.Today.AddDays(1);
            vm.EndDate = DateTime.Today;

            // Act / Assert
            vm.IsDateRangeValid.Should().BeFalse();
        }

        [Fact]
        public void ShouldReturnTrue_WhenStartDateAfterEndDateAreSame()
        {
            // Arrange
            var now = DateTime.Now;
            vm.StartDate = now;
            vm.EndDate = now;

            // Act / Assert
            vm.IsDateRangeValid.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnTrue_WhenEndDateIsAfterStartDate()
        {
            // Arrange
            vm.StartDate = DateTime.Today;
            vm.EndDate = DateTime.Today.AddDays(1);

            // Act / Assert
            vm.IsDateRangeValid.Should().BeTrue();
        }
    }
}
