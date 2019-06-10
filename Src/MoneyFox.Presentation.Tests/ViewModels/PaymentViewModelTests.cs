using System;
using System.Diagnostics.CodeAnalysis;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.ServiceLayer.ViewModels;
using Should;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class PaymentViewModelTests
    {
        [Fact]
        public void Ctor_SetDefaults()
        {
            // Arrange

            // Act
            var vm = new PaymentViewModel();

            // Assert
            vm.IsRecurring.ShouldBeFalse();
            vm.Date.Date.ShouldEqual(DateTime.Today);
        }

        [Fact]
        public void IsRecurring_SetTrue_CreateRecurringViewModel()
        {
            // Arrange
            var vm = new PaymentViewModel();
            vm.RecurringPayment.ShouldBeNull();

            // Act
            vm.IsRecurring = true;

            // Assert.
            vm.RecurringPayment.ShouldNotBeNull();
        }

        [Fact]
        public void IsRecurring_SetFalse_RecurringViewModelSetNull()
        {
            // Arrange
            var vm = new PaymentViewModel {IsRecurring = true};
            vm.RecurringPayment.ShouldNotBeNull();

            // Act
            vm.IsRecurring = false;

            // Assert.
            vm.RecurringPayment.ShouldBeNull();
        }
    }
}
